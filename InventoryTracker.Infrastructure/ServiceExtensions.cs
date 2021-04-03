using Microsoft.Extensions.DependencyInjection;


namespace InventoryTracker.Infrastructure
{
    public static class ServiceExtensions
    {
        #region Extension Method
        public static void ConfigureInfrastructureServices(this IServiceCollection services)
        {
            //Services Registration
            services.ConfigureSwaggerService();
            services.AddScoped<ValidateItemExistsFilter>();
        }
        #endregion        
    }
}
