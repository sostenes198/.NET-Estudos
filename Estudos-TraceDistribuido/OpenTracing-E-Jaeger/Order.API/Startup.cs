using Common.Tracing.Jaeger;
using Common.Tracing.OpenTracing;

namespace Order.API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenTracing();
        services.AddOpenTracingHandler();
        services.AddJaegerEnv(Configuration);
            
        services.AddHttpClient("payment.api", c =>
            {
                c.BaseAddress = new Uri($"{Configuration["HOST_PAYMENT"]}");
            })
           .WithOpenTracingHeaderHandler();

        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}