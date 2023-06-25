using DDRC.WebApi.Contracts;
using DDRC.WebApi.Data;
using DDRC.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DDRC.WebApi.Controllers
{
    [ApiController]
    [Route("api/stocks")]
    public class StockController : ControllerBase
    {
        private readonly MongoDbContext _context;

        public StockController(MongoDbContext context)
        {
            _context = context;
        }

        [HttpPost("import")]
        public IActionResult Import([FromBody] List<StockDto> dtos)
        {
            if (dtos.Any(x => x.Date != DateTime.UtcNow.Date)) return BadRequest();

            var movies = _context.Movies
                .Find(x => true)
                .ToList();

            foreach (var dto in dtos)
            {
                var movie = movies.SingleOrDefault(x => x.Title == dto.Movie);

                if (movie == null) return BadRequest();

                var stock = new StockModel
                {
                    Id = Guid.NewGuid(),
                    Date = dto.Date,
                    Amount = dto.Amount,
                    Movie = movie
                };

                _context.Stocks.InsertOne(stock);
            }

            return NoContent();
        }
    }
}
