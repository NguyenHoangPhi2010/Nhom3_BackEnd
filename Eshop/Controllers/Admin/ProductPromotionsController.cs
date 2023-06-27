using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eshop.Data;
using Eshop.Models;

namespace Eshop.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class ProductPromotionsController : ControllerBase
    {
        private readonly EshopContext _context;

        public ProductPromotionsController(EshopContext context)
        {
            _context = context;
        }

        // GET: api/ProductPromotions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductPromotion>>> GetProductPromotions()
        {
            return await _context.ProductPromotions.ToListAsync();
        }

        // GET: api/ProductPromotions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductPromotion>> GetProductPromotion(int id)
        {
            var productPromotion = await _context.ProductPromotions.FindAsync(id);

            if (productPromotion == null)
            {
                return NotFound();
            }

            return productPromotion;
        }

        // PUT: api/ProductPromotions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductPromotion(int id, ProductPromotion productPromotion)
        {
            if (id != productPromotion.Id)
            {
                return BadRequest();
            }

            _context.Entry(productPromotion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductPromotionExists(id))
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

        // POST: api/ProductPromotions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductPromotion>> PostProductPromotion(ProductPromotion productPromotion)
        {
            _context.ProductPromotions.Add(productPromotion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductPromotion", new { id = productPromotion.Id }, productPromotion);
        }

        // DELETE: api/ProductPromotions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductPromotion(int id)
        {
            var productPromotion = await _context.ProductPromotions.FindAsync(id);
            if (productPromotion == null)
            {
                return NotFound();
            }

            _context.ProductPromotions.Remove(productPromotion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductPromotionExists(int id)
        {
            return _context.ProductPromotions.Any(e => e.Id == id);
        }
    }
}
