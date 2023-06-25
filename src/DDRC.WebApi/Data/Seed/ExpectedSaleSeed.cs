using Bogus;
using DDRC.WebApi.Models;
using MongoDB.Driver;

namespace DDRC.WebApi.Data.Seed
{
    public static class ExpectedSaleSeed
    {
        public static void SeedData(MongoDbContext context)
        {
            var videoStores = context.VideoStores
                .Find(x => true)
                .ToList();

            var movies = context.Movies
                .Find(x => true)
                .ToList();

            var currentDate = DateTime.UtcNow.Date;

            foreach (var videoStore in videoStores)
            {
                foreach (var movie in movies)
                {
                    for (var date = currentDate; date < currentDate.AddYears(1); date = date.AddDays(1))
                    {
                        var model = new ExpectedSaleModel()
                        {
                            Id = Guid.NewGuid(),
                            Date = date,
                            Amount = new Faker("en").Random.Int(0, 100),
                            VideoStore = videoStore,
                            Movie = movie
                        };

                        context.ExpectedSales.InsertOne(model);
                    }
                }
            }
        }
    }
}
