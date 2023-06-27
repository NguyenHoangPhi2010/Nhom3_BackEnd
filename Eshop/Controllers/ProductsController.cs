using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eshop.Data;
using Eshop.Models;

namespace Eshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly EshopContext _context;

        public ProductsController(EshopContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts( )
        {
            return await _context.Products.Include(p => p.ProductType).Include(p =>p.ProductPromotion).ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.Include(p => p.ProductType).Include(p => p.ProductPromotion)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
        [HttpGet]
        [Route("FilteredProducts")]
        public async Task<ActionResult<IEnumerable<Product>>> FilterProduct(
        [FromQuery(Name = "brands")] string encodedBrands,
        //[FromQuery(Name = "cpus")] string encodedCPUs,
        //[FromQuery(Name = "exigencys")] string encodedExigencys,


        double minPrice,
        double maxPrice,
        int page = 1,
        int pageSize = 9)
        {
            var brands = string.IsNullOrEmpty(encodedBrands) ? new List<string>() : encodedBrands.Split(",").ToList();
            //var cpus = string.IsNullOrEmpty(encodedCPUs) ? new List<string>() : encodedCPUs.Split(",").ToList();
            //var exigencies = string.IsNullOrEmpty(encodedExigencys) ? new List<string>() : encodedExigencys.Split(",").ToList();


            var allProducts = _context.Products
                .Include(p => p.ProductType).Include(p => p.ProductPromotion)
                .Where(p =>
                    (!brands.Any() || brands.Contains(p.ProductType.Name)) &&
                    //(!cpus.Any() || cpus.Contains(p.CPU)) &&
                    (minPrice == 0 || p.Price >= minPrice) &&
                    (maxPrice == 0 || p.Price <= maxPrice));

            if (brands.Any())
            {
                foreach (var brand in brands)
                {
                    allProducts = allProducts.Where(p => p.ProductType.Name.Contains(brand));
                }

                var filteredProducts = await allProducts
                    .ToListAsync();

                var totalCount = filteredProducts.Count;
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                var paginatedProducts = filteredProducts.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    PaginatedProducts = paginatedProducts
                });
            }
            else
            {
                var filteredProducts = await allProducts
                    .ToListAsync();

                var totalCount = filteredProducts.Count;
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                var paginatedProducts = filteredProducts.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    PaginatedProducts = paginatedProducts
                });
            }
            
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
