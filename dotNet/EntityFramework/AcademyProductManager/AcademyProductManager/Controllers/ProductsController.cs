using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ProductsController : ControllerBase
    {
        private readonly ProductManagerContext _dbContext;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ProductManagerContext dbContext, ILogger<ProductsController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get(
            [FromQuery] Guid? categoryId,
            [FromQuery] string sellerName,
            CancellationToken cancellationToken,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 2)
        {
            IQueryable<Product> query = _dbContext.Products
                .AsNoTracking()
                .Include(product => product.Category);

            if (categoryId.HasValue)
            {
                query = query.Where(product => product.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrEmpty(sellerName))
            {
                query = query.Where(product => product.Seller.Name.Contains(sellerName));
            }

            var products = await query
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return Ok(products);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Product>> Get([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var maybeProduct = await _dbContext.Products
                .Include(product => product.Category)
                .FirstOrDefaultAsync(product => product.Id == id, cancellationToken);

            return maybeProduct is { }
                ? (ActionResult)Ok(maybeProduct)
                : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product, CancellationToken cancellationToken)
        {
            if (product.Id == default)
            {
                return UnprocessableEntity();
            }

            _dbContext.Products.Add(product);

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

            _dbContext.Products.Remove(new Product
            {
                Id = id,
            });

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok();
        }
    }
}
