using Bogus;
using DDRC.WebApi.Models;
using MongoDB.Driver;

namespace DDRC.WebApi.Data.Seed
{
    public static class MoviesSeed
    {
        public static void SeedData(MongoDbContext context)
        {
            var categories = context.Categories
                .Find(x => true)
                .ToList();

            var actors = context.Actors
                .Find(x => true)
                .ToList();

            for (var index = 0; index < 20; index++)
            {
                var model = new MovieModel()
                {
                    Id = Guid.NewGuid(),
                    Title = new Faker("en").Commerce.ProductName(),
                    Description = new Faker("en").Commerce.ProductDescription(),
                    Categories = new Faker("en").PickRandom(categories, new Faker("en").Random.Int(1, 3)).ToList(),
                    Actors = new Faker("en").PickRandom(actors, new Faker("en").Random.Int(2, 5)).ToList()
                };

                context.Movies.InsertOne(model);
            }
        }
    }
}
