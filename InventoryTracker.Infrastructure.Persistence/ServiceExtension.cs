using InventoryTracker.Infrastructure.Persistence.Mock;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryTracker.Infrastructure.Persistence
{
    public static class ServiceExtensions
    {
        #region Extension Method
        public static void ConfigurePersistenceServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IDateTimeService, UtcDateTimeService>();
        }
        #endregion
    }
}
