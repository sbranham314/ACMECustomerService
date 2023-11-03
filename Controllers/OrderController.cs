using ACMECorpCustomerService.Data;
using ACMECorpCustomerService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Azure.Security.KeyVault.Certificates;
using Microsoft.EntityFrameworkCore;
using ACMECorpCustomerService.Filters;

namespace ACMECorpCustomerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class OrderController : ControllerBase
    {

        private readonly ApplicationDBContext _context;

        public OrderController(ApplicationDBContext context) => _context = context;

        [HttpGet]
        public async Task<IEnumerable<Order>> Get()
            => await _context.Orders.ToListAsync();

        [HttpGet("id")]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByID(int id)
        {
            var customer = await _context.Orders.FindAsync(id);
            return customer == null ? NotFound() : Ok(customer);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByID), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, Order order)
        {
            if (id != order.Id) return BadRequest();

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var orderToDelete = await _context.Orders.FindAsync(id);
            if (orderToDelete == null) return NotFound();

            _context.Orders.Remove(orderToDelete);
            await _context.SaveChangesAsync();

            return NoContent();

        }
    }
}
