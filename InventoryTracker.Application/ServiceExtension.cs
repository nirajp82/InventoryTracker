using InventoryTracker.Infrastructure.Persistence;
using InventoryTracker.Infrastructure.Persistence.Mock;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application
{
    public static class ServiceExtensions
    {
        #region Extension Method
        public static void ConfigureRepoServices(this IServiceCollection services)
        {
            services.AddScoped<IMapperHelper, MapperHelper>();
            services.ConfigurePersistenceServices();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMediatR(typeof(Save).Assembly);
        }
        #endregion
    }
}
