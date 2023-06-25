using DDRC.WebApi.Contracts;
using DDRC.WebApi.Data;
using DDRC.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DDRC.WebApi.Controllers
{
    [ApiController]
    [Route("api/video-stores")]
    public class VideoStoresController : ControllerBase
    {
        private readonly MongoDbContext _context;

        public VideoStoresController(MongoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = _context.VideoStores
                .Find(x => true)
                .ToList();

            var dtos = model.Select(x => new VideoStoreForViewDto
            {
                Id = x.Id,
                Name = x.Name
            });

            return Ok(dtos);
        }

        [HttpGet("{id:guid}")]
        public IActionResult Details(Guid id)
        {
            var model = _context.VideoStores
                .Find(x => x.Id == id)
                .SingleOrDefault();

            if (model == null) return NoContent();

            var dto = new VideoStoreForViewDto
            {
                Id = model.Id,
                Name = model.Name
            };

            return Ok(dto);
        }

        [HttpPost]
        public ActionResult Create([FromBody] VideoStoreDto dto)
        {
            var model = new VideoStoreModel
            {
                Id = Guid.NewGuid(),
                Name = dto.Name
            };

            _context.VideoStores.InsertOne(model);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public ActionResult Edit(Guid id, [FromBody] VideoStoreDto dto)
        {
            var model = _context.VideoStores
                .Find(x => x.Id == id)
                .SingleOrDefault();

            if (model == null) return BadRequest();

            model.Name = dto.Name;

            _context.VideoStores.ReplaceOne(x => x.Id == id, model);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Delete(Guid id)
        {
            _context.VideoStores.DeleteOne(x => x.Id == id);

            return NoContent();
        }
    }
}
