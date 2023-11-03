using ACMECorpCustomerService.Data;
using ACMECorpCustomerService.Filters;
using ACMECorpCustomerService.Models;
using Azure.Security.KeyVault.Certificates;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ACMECorpCustomerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class ItemController : ControllerBase
    {

        private readonly ApplicationDBContext _context;

        public ItemController(ApplicationDBContext context) => _context = context;

        [HttpGet]
        public async Task<IEnumerable<Item>> Get()
            => await _context.Items.ToListAsync();

        [HttpGet("id")]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByID(int id)
        {
            var item = await _context.Items.FindAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(Item item)
        {
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByID), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, Item item)
        {
            if (id != item.Id) return BadRequest();

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var customerToDelete = await _context.Items.FindAsync(id);
            if (customerToDelete == null) return NotFound();

            _context.Items.Remove(customerToDelete);
            await _context.SaveChangesAsync();

            return NoContent();

        }


    }
}
