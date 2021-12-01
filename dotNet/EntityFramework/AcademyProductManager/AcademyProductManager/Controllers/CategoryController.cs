using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AcademyProductManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AcademyProductManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ProductManagerContext _dbContext;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ProductManagerContext dbContext, ILogger<CategoryController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get(CancellationToken cancellationToken)
        {
            var categories = await _dbContext.Categories
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Ok(categories);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Category>> Get([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var maybeCategory = await _dbContext.Categories
                .FirstOrDefaultAsync(category => category.Id == id, cancellationToken);

            return maybeCategory is { }
                ? (ActionResult)Ok(maybeCategory)
                : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category category, CancellationToken cancellationToken)
        {
            if (category.Id == default)
            {
                return UnprocessableEntity();
            }

            _dbContext.Categories.Add(category);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            if (id == default)
            {
                return UnprocessableEntity();
            }

            _dbContext.Categories.Remove(new Category
            {
                Id = id,
            });

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok();
        }
    }
}