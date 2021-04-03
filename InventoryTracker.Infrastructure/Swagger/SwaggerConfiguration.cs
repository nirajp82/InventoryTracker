using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace InventoryTracker.Infrastructure
{
    public static class SwaggerConfiguration
    {
        #region Members
        private const string _docName = "InventoryTracker";
        #endregion


        #region Public Methods
        public static void ConfigureSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(swaggerOptions =>
            {
                swaggerOptions.DescribeAllParametersInCamelCase();
                swaggerOptions.SwaggerDoc(_docName, new OpenApiInfo
                {
                    Title = "Inventory Tracker API",
                    Version = "Version 1",
                    Description = "A simple Inventory Tracker API",
                    Contact = new OpenApiContact
                    {
                        Name = "NPatel",
                        Email = "nijpatel90@hotmail.com",
                        Url = new Uri("https://github.com/nirajp82/"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "The MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT"),
                    }
                });
               
                //To avoid name conflict issues.
                swaggerOptions.CustomSchemaIds(s => $"{s.FullName}");
            });
        }

        public static void ConfigureSwaggerMiddleware(this IApplicationBuilder appBuilder)
        {
            appBuilder.UseSwagger();
            appBuilder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{_docName}/swagger.json", "RFP API");
            });
        }
        #endregion
    }
}