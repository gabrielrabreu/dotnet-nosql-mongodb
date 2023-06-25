using Bogus;
using DDRC.WebApi.Models;
using MongoDB.Driver;

namespace DDRC.WebApi.Data.Seed
{
    public static class FulfilledSaleSeed
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
                    for (var date = currentDate.AddYears(-1); date < currentDate; date = date.AddDays(1))
                    {
                        for (var index = 0; index < new Faker("en").Random.Int(0, 5); index++)
                        {
                            var model = new FulfilledSaleModel()
                            {
                                Id = Guid.NewGuid(),
                                Date = new Faker("en").Date.Between(date, date.AddDays(1).AddMinutes(-1)),
                                VideoStore = videoStore,
                                Movie = movie
                            };

                            context.FulfilledSales.InsertOne(model);
                        }
                    }
                }
            }
        }
    }
}
