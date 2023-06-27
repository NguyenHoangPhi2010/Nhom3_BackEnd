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
    public class HomeController : ControllerBase
    {
        private readonly EshopContext _context;

        public HomeController(EshopContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        [Route("GetProductHot")]
        public async Task<ActionResult<IEnumerable<Product>>> Gethot()
        {
           
            return await _context.Products.Include(p => p.ProductType).Include(p => p.ProductPromotion).Where(p => p.ProductHot == true).Take(8).ToListAsync();
        }
        [HttpGet]
        [Route("GetProductNew")]
        public async Task<ActionResult<IEnumerable<Product>>> GetNew()
        {

            return await _context.Products.Include(p => p.ProductType).Include(p => p.ProductPromotion).Where(p => p.ProductNew == true).Take(8).ToListAsync();
        }
        [HttpGet]
        [Route("GetProductHome")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string search)
        {
            if (search == null)
            {
                return NotFound();
            }
            var result = await _context.Products.Where(x => x.Name.Contains(search)).ToListAsync();
            return result;
        }
    }
}
