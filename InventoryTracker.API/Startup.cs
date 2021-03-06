using FluentValidation.AspNetCore;
using InventoryTracker.Application;
using InventoryTracker.Infrastructure;
using InventoryTracker.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryTracker.API
{
    public class Startup
    {
        #region Members
        private readonly IConfiguration _configuration;
        #endregion


        #region Constructor
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion


        // This method gets called by the runtime. Use this method to add services to the container.
        #region Public Methods
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureInfrastructureServices();
            services.ConfigureApplicationServices();

            services.AddControllers()
            .AddFluentValidation(cfg =>
            {
                cfg.RegisterValidatorsFromAssemblyContaining<SaveList.CommandValidator>();
            });

            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseHealthChecks("/health");
            app.UseRouting();
            app.ConfigureSwaggerMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        } 
        #endregion
    }
}
