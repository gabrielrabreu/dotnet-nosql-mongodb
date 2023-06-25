using DDRC.WebApi.Contracts;
using DDRC.WebApi.Data;
using DDRC.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DDRC.WebApi.Controllers
{
    [ApiController]
    [Route("api/sales")]
    public class SalesController : ControllerBase
    {
        private readonly MongoDbContext _context;

        public SalesController(MongoDbContext context)
        {
            _context = context;
        }

        [HttpPost("fulfilled:import")]
        public IActionResult ImportFulfilled([FromBody] List<FulfilledSaleDto> dtos)
        {
            if (dtos.Any(x => x.Date >= DateTime.UtcNow.Date)) return BadRequest();

            var videoStores = _context.VideoStores
                .Find(x => true)
                .ToList();

            var movies = _context.Movies
                .Find(x => true)
                .ToList();

            foreach (var dto in dtos)
            {
                var videoStore = videoStores.SingleOrDefault(x => x.Name == dto.VideoStore);
                var movie = movies.SingleOrDefault(x => x.Title == dto.Movie);

                if (videoStore == null || movie == null) return BadRequest();

                var sale = new FulfilledSaleModel
                {
                    Id = Guid.NewGuid(),
                    Date = dto.Date,
                    VideoStore = videoStore,
                    Movie = movie
                };

                _context.FulfilledSales.InsertOne(sale);
            }

            return NoContent();
        }

        [HttpPost("expected:import")]
        public IActionResult ImportExpected([FromBody] List<ExpectedSaleDto> dtos)
        {
            if (dtos.Any(x => x.Date < DateTime.UtcNow.Date)) return BadRequest();

            var videoStores = _context.VideoStores
                .Find(x => true)
                .ToList();

            var movies = _context.Movies
                .Find(x => true)
                .ToList();

            foreach (var dto in dtos)
            {
                var videoStore = videoStores.SingleOrDefault(x => x.Name == dto.VideoStore);
                var movie = movies.SingleOrDefault(x => x.Title == dto.Movie);

                if (videoStore == null || movie == null) return BadRequest();

                var sale = new ExpectedSaleModel
                {
                    Id = Guid.NewGuid(),
                    Date = dto.Date,
                    Amount = dto.Amount,
                    VideoStore = videoStore,
                    Movie = movie
                };

                _context.ExpectedSales.InsertOne(sale);
            }

            return NoContent();
        }
    }
}
