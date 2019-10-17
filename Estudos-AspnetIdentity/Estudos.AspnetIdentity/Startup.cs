using Estudos.AspnetIdentity.Context;
using Estudos.AspnetIdentity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Estudos.AspnetIdentity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var connection = @"Data Source=SQNOT317\SQLEXPRESS2017;Initial Catalog=EstudosIdentity;user id=sa;password=539624eddas_;MultipleActiveResultSets=true";
            var migrationAssembly = typeof(Startup).Assembly.GetName().Name;
            services.AddDbContext<MyUserDbContext>(opt => 
                opt.UseSqlServer(connection, sql => sql.MigrationsAssembly(migrationAssembly)));

            services.AddIdentityCore<MyUser>(opt => { });
            services.AddScoped<IUserStore<MyUser>, UserOnlyStore<MyUser, MyUserDbContext>>();

            services.AddAuthentication("cookies")
                .AddCookie("cookies");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();

        }
    }
}