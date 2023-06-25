using Bogus;
using DDRC.WebApi.Models;

namespace DDRC.WebApi.Data.Seed
{
    public static class CategoriesSeed
    {
        public static void SeedData(MongoDbContext context)
        {
            for (var index = 0; index < 15; index++)
            {
                var model = new CategoryModel()
                {
                    Id = Guid.NewGuid(),
                    Name = new Faker("en").Name.JobType()
                };

                context.Categories.InsertOne(model);
            }
        }
    }
}
