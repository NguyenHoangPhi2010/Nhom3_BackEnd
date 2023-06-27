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
    public class ImportInvoiceDetailsController : ControllerBase
    {
        private readonly EshopContext _context;

        public ImportInvoiceDetailsController(EshopContext context)
        {
            _context = context;
        }

        // GET: api/ImportInvoiceDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImportInvoiceDetail>>> GetImportInvoiceDetails()
        {
            return await _context.ImportInvoiceDetails.ToListAsync();
        }

        // GET: api/ImportInvoiceDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImportInvoiceDetail>> GetImportInvoiceDetail(int id)
        {
            var importInvoiceDetail = await _context.ImportInvoiceDetails.FindAsync(id);

            if (importInvoiceDetail == null)
            {
                return NotFound();
            }

            return importInvoiceDetail;
        }

        // PUT: api/ImportInvoiceDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImportInvoiceDetail(int id, ImportInvoiceDetail importInvoiceDetail)
        {
            if (id != importInvoiceDetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(importInvoiceDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImportInvoiceDetailExists(id))
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

        // POST: api/ImportInvoiceDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ImportInvoiceDetail>> PostImportInvoiceDetail(ImportInvoiceDetail importInvoiceDetail)
        {
            _context.ImportInvoiceDetails.Add(importInvoiceDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImportInvoiceDetail", new { id = importInvoiceDetail.Id }, importInvoiceDetail);
        }

        // DELETE: api/ImportInvoiceDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImportInvoiceDetail(int id)
        {
            var importInvoiceDetail = await _context.ImportInvoiceDetails.FindAsync(id);
            if (importInvoiceDetail == null)
            {
                return NotFound();
            }

            _context.ImportInvoiceDetails.Remove(importInvoiceDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImportInvoiceDetailExists(int id)
        {
            return _context.ImportInvoiceDetails.Any(e => e.Id == id);
        }
    }
}
