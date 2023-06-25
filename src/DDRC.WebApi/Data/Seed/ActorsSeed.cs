using Bogus;
using DDRC.WebApi.Models;

namespace DDRC.WebApi.Data.Seed
{
    public static class ActorsSeed
    {
        public static void SeedData(MongoDbContext context)
        {
            for (var index = 0; index < 100; index++)
            {
                var model = new ActorModel()
                {
                    Id = Guid.NewGuid(),
                    Name = new Faker("en").Name.FullName()
                };

                context.Actors.InsertOne(model);
            }
        }
    }
}
