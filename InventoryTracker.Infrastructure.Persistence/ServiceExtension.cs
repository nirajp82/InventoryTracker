using InventoryTracker.Infrastructure.Persistence.Mock;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Infrastructure.Persistence
{
    public static class ServiceExtensions
    {
        #region Extension Method
        public static void ConfigureRepoServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
        #endregion
    }
}
