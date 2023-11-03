using ACMECorpCustomerService.Data;
using ACMECorpCustomerService.Filters;
using ACMECorpCustomerService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ACMECorpCustomerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class OrderDetailController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public OrderDetailController(ApplicationDBContext context) => _context = context;

        [HttpGet]
        public async Task<IEnumerable<OrderDetail>> Get()
            => await _context.OrderDetails.ToListAsync();

        [HttpGet("id")]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByID(int id)
        {
            var customer = await _context.OrderDetails.FindAsync(id);
            return customer == null ? NotFound() : Ok(customer);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByID), new { id = orderDetail.Id }, orderDetail);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, OrderDetail orderDetail)
        {
            if (id != orderDetail.Id) return BadRequest();

            _context.Entry(orderDetail).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var orderDetailToDelete = await _context.OrderDetails.FindAsync(id);
            if (orderDetailToDelete == null) return NotFound();

            _context.OrderDetails.Remove(orderDetailToDelete);
            await _context.SaveChangesAsync();

            return NoContent();

        }
    }
}
