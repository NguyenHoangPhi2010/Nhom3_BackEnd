using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eshop.Data;
using Eshop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Eshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly EshopContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public InvoicesController(EshopContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Invoices
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
            var claims = HttpContext.User.Claims;
            if (claims == null)
            {
                return BadRequest();
            }
            var username = claims.FirstOrDefault(c => c.Type == "username").Value;
            return await _context.Invoices.Include(p => p.ApplicationUser).Where(p => p.ApplicationUser.UserName == username).ToListAsync();
        }

        // GET: api/Invoices/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Invoice>> GetInvoice(int id)
        {
            var invoice = await _context.Invoices.Include(p => p.ApplicationUser).FirstOrDefaultAsync(m => m.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            return invoice;
        }

        // PUT: api/Invoices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice(int id, Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return BadRequest();
            }

            _context.Entry(invoice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
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

        // POST: api/Invoices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
            var claims = HttpContext.User.Claims;
            if (claims == null)
            {
                return BadRequest();
            }
            var username = claims.FirstOrDefault(c => c.Type == "username").Value;
            var carts = _context.Carts.Include(c => c.ApplicationUser)
                                     .Include(c => c.Product)
                                     .Where(c => c.ApplicationUser.UserName == username);
            var accountId = _context.ApplicationUsers.FirstOrDefault(a => a.UserName == username).Id;
            var total = carts.Sum(c => c.Product.Price * c.Quantity);
            Invoice item = new Invoice
            {
                Code = DateTime.Now.ToString("yyyyMMddhhmmss"),
                ApplicationUserId = accountId,
                IssuedDate = DateTime.Now,
                ShippingAddress = invoice.ShippingAddress,
                ShippingPhone = invoice.ShippingPhone,
                Total = total,
                Status = 1
            };
            _context.Invoices.Add(item);
            _context.SaveChanges();
            foreach (var item2 in carts)
            {
                InvoiceDetail detail = new InvoiceDetail
                {
                    InvoiceId = item.Id,
                    ProductId = item2.ProductId,
                    Quantity = item2.Quantity,
                    UnitPrice = item2.Product.Price
                };
                _context.InvoiceDetails.Add(detail);
                _context.Carts.Remove(item2);

                item2.Product.Stock -= item2.Quantity;
                _context.Products.Update(item2.Product);
            }
            _context.SaveChanges();

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInvoice", new { id = invoice.Id }, invoice);
        }

        // DELETE: api/Invoices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }
    }
}
