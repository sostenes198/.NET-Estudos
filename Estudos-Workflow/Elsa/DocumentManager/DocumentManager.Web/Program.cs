using DocumentManager.Core;
using DocumentManager.Core.Options;
using DocumentManager.Persistence;
using DocumentManager.WorkFlows;
using Elsa;
using Elsa.Activities.Email.Options;
using Elsa.Activities.Http.Options;
using Elsa.Persistence.EntityFramework.Sqlite;
using Elsa.Server.Hangfire.Extensions;
using Hangfire;
using Hangfire.SQLite;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using Storage.Net;

namespace DocumentManager.Web;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();

        var dbConnectionString = builder.Configuration.GetConnectionString("Sqlite")!;

        AddHangfire();
        AddWorkflowServices();
        AddDomainServices();
        AddPersistenceServices();

        builder.Services.AddNotificationHandlersFrom<Program>();

        builder.Services.AddCors(cors => cors.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("Content-Disposition")));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseStaticFiles()
           .UseCors()
           .UseRouting()
           .UseHttpActivities()
           .UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers(); // Elsa API Endpoints are implemented as ASP.NET API controllers.
            });

        app.Run();

        void AddHangfire()
        {
            builder.Services
               .AddHangfire(config => config

                                // Use same SQLite database as Elsa for storing jobs. 
                               .UseSQLiteStorage(dbConnectionString)
                               .UseSimpleAssemblyNameTypeSerializer()

                                // Elsa uses NodaTime primitives, so Hangfire needs to be able to serialize them.
                               .UseRecommendedSerializerSettings(settings => settings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb)))
               .AddHangfireServer((sp, options) =>
                {
                    // Bind settings from configuration.
                    builder.Configuration.GetSection("Hangfire").Bind(options);

                    // Configure queues for Elsa workflow dispatchers.
                    options.ConfigureForElsaDispatchers(sp);
                });
        }

        void AddWorkflowServices()
        {
            builder.Services.AddWorkflowServices(dbContext => dbContext.UseSqlite(dbConnectionString));

            // Configure SMTP.
            builder.Services.Configure<SmtpOptions>(options => builder.Configuration.GetSection("Elsa:Smtp").Bind(options));

            // Configure HTTP activities.
            builder.Services.Configure<HttpActivityOptions>(options => builder.Configuration.GetSection("Elsa:Server").Bind(options));

            // Elsa API (to allow Elsa Dashboard to connect for checking workflow instances).
            builder.Services.AddElsaApiEndpoints();
        }

        void AddDomainServices()
        {
            builder.Services.AddDomainServices();

            // Configure Storage for DocumentStorage.
            builder.Services.Configure<DocumentStorageOptions>(options => options.BlobStorageFactory = () => StorageFactory.Blobs.DirectoryFiles(Path.Combine(builder.Environment.ContentRootPath, "App_Data/Uploads")));
        }

        void AddPersistenceServices()
        {
            builder.Services.AddDomainPersistence(dbConnectionString);
        }
    }
}