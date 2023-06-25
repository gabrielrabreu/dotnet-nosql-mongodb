using DDRC.WebApi.Settings;
using Microsoft.Extensions.Options;

namespace DDRC.WebApi.Data.Seed
{
    public static class ContextSeed
    {
        public static void InitializeDatabase(this IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var databaseSettings = services.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            var logger = services.GetRequiredService<ILogger<MongoDbContext>>();

            if (databaseSettings != null && databaseSettings.RecreateDatabase)
            {
                var context = scope.ServiceProvider.GetRequiredService<MongoDbContext>();

                logger.LogInformation("Drop mongo collections");

                context.Actors.Database.DropCollection("Actors");
                context.Categories.Database.DropCollection("Categories");
                context.Movies.Database.DropCollection("Movies");
                context.VideoStores.Database.DropCollection("VideoStores");
                context.Stocks.Database.DropCollection("Stocks");
                context.FulfilledSales.Database.DropCollection("FulfilledSales");
                context.ExpectedSales.Database.DropCollection("ExpectedSales");
            }
        }

        public static void SeedData(this IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var databaseSettings = services.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            var logger = services.GetRequiredService<ILogger<MongoDbContext>>();

            if (databaseSettings != null && databaseSettings.SeedDatabase)
            {
                var context = scope.ServiceProvider.GetRequiredService<MongoDbContext>();

                logger.LogInformation("Seeding actors");
                ActorsSeed.SeedData(context);

                logger.LogInformation("Seeding categories");
                CategoriesSeed.SeedData(context);

                logger.LogInformation("Seeding movies");
                MoviesSeed.SeedData(context);

                logger.LogInformation("Seeding video stores");
                VideoStoresSeed.SeedData(context);

                logger.LogInformation("Seeding stocks");
                StocksSeed.SeedData(context);

                logger.LogInformation("Seeding fulfilled sales");
                FulfilledSaleSeed.SeedData(context);

                logger.LogInformation("Seeding expected sales");
                ExpectedSaleSeed.SeedData(context);
            }
        }
    }
}
