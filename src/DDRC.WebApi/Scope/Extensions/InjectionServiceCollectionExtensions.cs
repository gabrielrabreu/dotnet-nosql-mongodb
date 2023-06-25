using DDRC.WebApi.Data;
using DDRC.WebApi.Reports;
using DDRC.WebApi.Settings;

namespace DDRC.WebApi.Scope.Extensions
{
    public static class InjectionServiceCollectionExtensions
    {
        public static void AddDDRCServices(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseSettingsSection = configuration.GetSection("DatabaseSettings");
            services.Configure<DatabaseSettings>(databaseSettingsSection);

            services.AddScoped<MongoDbContext>();

            services.AddScoped<IVideoStoreReport, VideoStoreReport>();
        }
    }
}
