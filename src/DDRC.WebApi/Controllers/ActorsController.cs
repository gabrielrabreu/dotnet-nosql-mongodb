using DDRC.WebApi.Contracts;
using DDRC.WebApi.Data;
using DDRC.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DDRC.WebApi.Controllers
{
    [ApiController]
    [Route("api/actors")]
    public class ActorsController : ControllerBase
    {
        private readonly MongoDbContext _context;

        public ActorsController(MongoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = _context.Actors
                .Find(x => true)
                .ToList();

            var dtos = model.Select(x => new ActorForViewDto
            {
                Id = x.Id,
                Name = x.Name
            });

            return Ok(dtos);
        }

        [HttpGet("{id:guid}")]
        public IActionResult Details(Guid id)
        {
            var model = _context.Actors
                .Find(x => x.Id == id)
                .SingleOrDefault();

            if (model == null) return NoContent();

            var dto = new ActorForViewDto
            {
                Id = model.Id,
                Name = model.Name
            };

            return Ok(dto);
        }

        [HttpPost]
        public ActionResult Create([FromBody] ActorDto dto)
        {
            var model = new ActorModel
            {
                Id = Guid.NewGuid(),
                Name = dto.Name
            };

            _context.Actors.InsertOne(model);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public ActionResult Edit(Guid id, [FromBody] ActorDto dto)
        {
            var model = _context.Actors
                .Find(x => x.Id == id)
                .SingleOrDefault();

            if (model == null) return BadRequest();

            model.Name = dto.Name;

            _context.Actors.ReplaceOne(x => x.Id == id, model);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Delete(Guid id)
        {
            _context.Actors.DeleteOne(x => x.Id == id);

            return NoContent();
        }
    }
}
