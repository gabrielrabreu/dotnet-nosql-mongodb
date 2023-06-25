using DDRC.WebApi.Contracts;
using DDRC.WebApi.Data;
using DDRC.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DDRC.WebApi.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly MongoDbContext _context;

        public CategoriesController(MongoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = _context.Categories
                .Find(x => true)
                .ToList();

            var result = model.Select(x => new CategoryForViewDto
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public IActionResult Details(Guid id)
        {
            var model = _context.Categories
                .Find(x => x.Id == id)
                .SingleOrDefault();

            if (model == null) return NoContent();

            var dto = new CategoryForViewDto
            {
                Id = model.Id,
                Name = model.Name
            };

            return Ok(dto);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CategoryDto dto)
        {
            var model = new CategoryModel
            {
                Name = dto.Name
            };

            _context.Categories.InsertOne(model);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public IActionResult Edit(Guid id, [FromBody] CategoryDto dto)
        {
            var model = _context.Categories
                .Find(x => x.Id == id)
                .SingleOrDefault();

            if (model == null) return BadRequest();

            model.Name = dto.Name;

            _context.Categories.ReplaceOne(x => x.Id == id, model);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            _context.Categories.DeleteOne(x => x.Id == id);

            return NoContent();
        }
    }
}
