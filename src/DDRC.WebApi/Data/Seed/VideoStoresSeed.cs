﻿using Bogus;
using DDRC.WebApi.Models;

namespace DDRC.WebApi.Data.Seed
{
    public static class VideoStoresSeed
    {
        public static void SeedData(MongoDbContext context)
        {
            for (var index = 0; index < 5; index++)
            {
                var model = new VideoStoreModel()
                {
                    Id = Guid.NewGuid(),
                    Name = new Faker("en").Company.CompanyName()
                };

                context.VideoStores.InsertOne(model);
            }
        }
    }
}
