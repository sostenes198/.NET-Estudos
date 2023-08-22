using System;
using Estudos.AspnetIdentity.Context;
using Estudos.AspnetIdentity.Models;
using Estudos.AspNetIdentityJwt.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
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

            var connection = @"Data Source=SQNOT317\SQLEXPRESS2017;Initial Catalog=EstudosIdentity;user id=sa;password=Abc123456_;MultipleActiveResultSets=true";
            var migrationAssembly = typeof(Startup).Assembly.GetName().Name;
            services.AddDbContext<MyUserDbContext>(opt =>
                opt.UseSqlServer(connection, sql => sql.MigrationsAssembly(migrationAssembly)));

            services.AddIdentity<MyUser, IdentityRole>(opt => 
                { 
                    opt.SignIn.RequireConfirmedEmail = true;
                    opt.Lockout.MaxFailedAccessAttempts = 3;
                    opt.Lockout.AllowedForNewUsers = true;
                })
                .AddEntityFrameworkStores<MyUserDbContext>()
                .AddRoleValidator<RoleValidator<Role>>()
                .AddRoleManager<RoleManager<Role>>()
                .AddSignInManager<SignInManager<User>>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserClaimsPrincipalFactory<MyUser>, MyUserClaimsPrincipalFactory>();

            services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(3));

            services.ConfigureApplicationCookie(options => options.LoginPath = "/home");

//            services.AddAuthentication("cookies")
//                .AddCookie("cookies");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}