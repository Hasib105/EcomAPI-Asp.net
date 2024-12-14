using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcomApi.Models; // Adjust the namespace if necessary
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly EcomApi.Data.ApplicationDbContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(EcomApi.Data.ApplicationDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            _logger.LogInformation("Fetching all products.");

            // Safeguard in case of a null database context
            if (_context.Products == null)
            {
                _logger.LogError("Database context is null. Cannot fetch products.");
                return Problem("Database connection is unavailable.");
            }

            var products = await _context.Products

                .Include(p => p.Images)  // Eagerly load related images (if applicable)
                .ToListAsync();

            return Ok(products);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            _logger.LogInformation($"Fetching product with ID {id}.");

            if (_context.Products == null)
            {
                _logger.LogError("Database context is null. Cannot fetch product.");
                return Problem("Database connection is unavailable.");
            }

            var product = await _context.Products
                .Include(p => p.Images)  // Include related images
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                _logger.LogWarning($"Product with ID {id} not found.");
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid product model state.");
                return BadRequest(ModelState);
            }

            // Check if CategoryId exists
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == product.CategoryId);
            if (!categoryExists)
            {
                _logger.LogWarning($"Category with ID {product.CategoryId} does not exist.");
                return BadRequest($"Invalid CategoryId: {product.CategoryId}");
            }

            _logger.LogInformation("Adding a new product: {ProductName}", product.Name);

            // Add product and save changes
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                _logger.LogWarning($"Product ID mismatch: {id} != {product.Id}");
                return BadRequest("Product ID in URL and body do not match.");
            }

            // Check if the category exists
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == product.CategoryId);
            if (!categoryExists)
            {
                _logger.LogWarning($"Category with ID {product.CategoryId} does not exist.");
                return BadRequest($"Invalid CategoryId: {product.CategoryId}");
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Product with ID {id} updated successfully.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    _logger.LogWarning($"Product with ID {id} not found for update.");
                    return NotFound();
                }
                else
                {
                    _logger.LogError($"Concurrency error occurred while updating product with ID {id}.");
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            _logger.LogInformation($"Attempting to delete product with ID {id}.");

            if (_context.Products == null)
            {
                _logger.LogError("Database context is null. Cannot delete product.");
                return Problem("Database connection is unavailable.");
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                _logger.LogWarning($"Product with ID {id} not found for deletion.");
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Product with ID {id} deleted successfully.");

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products?.Any(e => e.Id == id) ?? false;
        }
    }
}
