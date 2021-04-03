using Microsoft.Extensions.DependencyInjection;


namespace InventoryTracker.Infrastructure
{
    public static class ServiceExtensions
    {
        #region Extension Method
        public static void ConfigureInfrastructureServices(this IServiceCollection services)
        {
            //Services Registration
            RegisterServices(services);
        }
        #endregion


        #region Private Methods       
        private static void RegisterServices(IServiceCollection services)
        {
            services.ConfigureSwaggerService();
        }
        #endregion
    }
}
