using InventoryTracker.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InventoryTracker.Application
{
    public static class ServiceExtensions
    {
        #region Extension Method
        public static void ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IMapperHelper, MapperHelper>();
            services.ConfigurePersistenceServices();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMediatR(typeof(Save).Assembly);
        }
        #endregion
    }
}
