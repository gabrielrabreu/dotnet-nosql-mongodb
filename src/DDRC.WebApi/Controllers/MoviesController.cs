using DDRC.WebApi.Contracts;
using DDRC.WebApi.Data;
using DDRC.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DDRC.WebApi.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MoviesController : ControllerBase
    {
        private readonly MongoDbContext _context;

        public MoviesController(MongoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = _context.Movies
                .Find(x => true)
                .ToList();

            var dtos = model.Select(x => new MovieForViewDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Actors = x.Actors.Select(x => x.Name).ToList(),
                Categories = x.Categories.Select(x => x.Name).ToList()
            });

            return Ok(dtos);
        }

        [HttpGet("{id:guid}")]
        public IActionResult Details(Guid id)
        {
            var model = _context.Movies
                .Find(x => x.Id == id)
                .SingleOrDefault();

            if (model == null) return NoContent();

            var dto = new MovieForViewDto
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Actors = model.Actors.Select(x => x.Name).ToList(),
                Categories = model.Categories.Select(x => x.Name).ToList()
            };

            return Ok(dto);
        }

        [HttpPost]
        public ActionResult Create([FromBody] MovieDto dto)
        {
            var actors = _context.Actors
                .Find(x => dto.Actors.Contains(x.Name))
                .ToList();

            var categories = _context.Categories
                .Find(x => dto.Categories.Contains(x.Name))
                .ToList();

            var model = new MovieModel
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                Actors = actors,
                Categories = categories
            };

            _context.Movies.InsertOne(model);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public ActionResult Edit(Guid id, [FromBody] MovieDto dto)
        {
            var model = _context.Movies
                .Find(x => x.Id == id)
                .SingleOrDefault();

            var actors = _context.Actors
                .Find(x => dto.Actors.Contains(x.Name))
                .ToList();

            var categories = _context.Categories
                .Find(x => dto.Categories.Contains(x.Name))
                .ToList();

            if (model == null) return BadRequest();

            model.Title = dto.Title;
            model.Description = dto.Description;
            model.Actors = actors;
            model.Categories = categories;

            _context.Movies.ReplaceOne(x => x.Id == id, model);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Delete(Guid id)
        {
            _context.Movies.DeleteOne(x => x.Id == id);

            return NoContent();
        }
    }
}
