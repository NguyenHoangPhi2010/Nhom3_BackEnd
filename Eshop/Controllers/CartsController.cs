using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eshop.Data;
using Eshop.Models;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;

namespace Eshop.Controllers
{
    
    [Route("api/[controller]")]
    
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly EshopContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public CartsController(EshopContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Carts
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            var claims = HttpContext.User.Claims;
            if (claims == null)
            {
                return NotFound();
            }
            var username = claims.FirstOrDefault(c => c.Type == "username").Value;
            return await _context.Carts.Include(p => p.Product).Include(p =>p.Product.ProductPromotion).Include(p => p.ApplicationUser).Where(p => p.ApplicationUser.UserName == username).ToListAsync();
        }

        // GET: api/Carts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }

            return cart;
        }

        // PUT: api/Carts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, Cart cart)
        {
            if (id != cart.Id)
            {
                return BadRequest();
            }

            //_context.Entry(cart).State = EntityState.Modified;
            var item = await _context.Carts
               .Include(c => c.Product)
               .FirstOrDefaultAsync(m => m.Id == id);
            item.Quantity = cart.Quantity;
            if (item == null)
            {
                return NotFound();
            }
            if (item.Product.Stock < item.Quantity)
            {
                item.Quantity = item.Product.Stock;
                _context.Carts.Update(item);
                _context.SaveChanges();
                return BadRequest();
            }
            _context.Carts.Update(item);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
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

        // POST: api/Carts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            var claims = HttpContext.User.Claims;
            if (claims == null)
            {
                return BadRequest();
            }
            var username = claims.FirstOrDefault(c => c.Type == "username").Value;
            string accountId = _context.ApplicationUsers.FirstOrDefault(a => a.UserName == username).Id;
            var item = _context.Carts.Where(c => c.ApplicationUserId == accountId && c.ProductId == cart.ProductId).FirstOrDefault();
            if (item != null)
            {
                item.Quantity += cart.Quantity;
                _context.Carts.Update(item);
            }
            else
            {
                cart.ApplicationUserId = accountId;
                _context.Carts.Add(cart);
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCart", new { id = cart.Id }, cart);
        }
        


        // DELETE: api/Carts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
