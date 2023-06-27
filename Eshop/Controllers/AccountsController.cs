using Eshop.Data;
using Eshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;

namespace Eshop.Controllers
{
    [Route("api/[controller]")]
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
            _configuration=configuration;
        }
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
            string accountId = _context.ApplicationUsers.FirstOrDefault(a => a.UserName == username).Id;
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
        public async Task<IActionResult> PutAccount(string id,[FromBody] ApplicationUser model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            user.UserName= model.UserName;
            user.Email= model.Email;
            user.EmailConfirmed= model.EmailConfirmed;
            user.PhoneNumber= model.PhoneNumber;
            user.FullName= model.FullName;
            user.Address= model.Address;
            user.Status= model.Status;
            user.Avatar= model.Avatar;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            if (!string.IsNullOrEmpty(model.PasswordHash))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetResult = await _userManager.ResetPasswordAsync(user, token, model.PasswordHash);
                if (!resetResult.Succeeded)
                {
                    return BadRequest(resetResult.Errors);
                }
            }
            return Ok();
            //if (id != user.Id)
            //{
            //    return BadRequest();
            //}
            
            //_context.Entry(user).State = EntityState.Modified;
            
            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!AccountExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();
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
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.OldPassword))
            {
                return BadRequest(new { message = "Invalid username or password" });
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.Errors.FirstOrDefault()?.Description });
            }

            return Ok(user);
        }
        private bool AccountExists(string id)
        {
            return _context.ApplicationUsers.Any(e => e.Id == id);
        }

    }
}
