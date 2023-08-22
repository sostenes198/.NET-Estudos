using System;
using EstudosMediator.Filters;
using EstudosMediator.Infra;
using EstudosMediator.PipelineBehavior;
using EstudosMediator.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EstudosMediator
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(config => config.Filters.Add(typeof(NotificationFilter)));
            services.AddSingleton<ICustomerRepository, CustomerRepository>();
            services.AddScoped<NotificationContext>();
            AddMediatr(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
        
        private static void AddMediatr(IServiceCollection services)
        {
            const string applicationAssemblyName = "EstudosMediator";
            var assembly = AppDomain.CurrentDomain.Load(applicationAssemblyName);

            AssemblyScanner
                .FindValidatorsInAssembly(assembly)
                .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorRequestBehavior<,>));

            services.AddMediatR(assembly);
        }
    }
}