using Eshop.Data;
using Eshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;

namespace Eshop.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly EshopContext _context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;



        public AccountsController(EshopContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAccounts()
        //{
        //    var users = await _userManager.Users.ToListAsync();
        //    var accounts = users.Select(x => new ApplicationUser
        //    {
        //        Address = x.Address,
        //        Avatar= x.Avatar,
        //        FullName = x.FullName,
        //        Status = x.Status,
        //        UserName = x.UserName,
        //        Email = x.Email,
        //        PhoneNumber = x.PhoneNumber
        //    });

        //    return Ok(accounts);
        //}
        //lấy tất cả Accourts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAccounts()
        {
            var users = await _context.ApplicationUsers.ToListAsync();

            return Ok(users);
        }
        //lấy accounts lúc đăng nhập 
        [Authorize]
        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            var claims = HttpContext.User.Claims;
            if (claims == null)
            {
                return BadRequest();
            }
            var username = claims.FirstOrDefault(c => c.Type == "username").Value;
            string accountId = _context.ApplicationUsers
                .FirstOrDefault(a => a.UserName == username).Id;
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(m => m.Id == accountId);
            return Ok(user);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetAccount(string id)
        {
            if (_context.ApplicationUsers == null)
            {
                return NotFound();
            }
            var product = await _context.ApplicationUsers
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
        // PUT: api/accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(string id, ApplicationUser user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
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
        //Xóa account 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool AccountExists(string id)
        {
            return _context.ApplicationUsers.Any(e => e.Id == id);
        }

    }
}

