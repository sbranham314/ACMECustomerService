using ACMECorpCustomerService.Data;
using ACMECorpCustomerService.Filters;
using ACMECorpCustomerService.Models;
using Azure.Security.KeyVault.Certificates;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;

namespace ACMECorpCustomerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class CustomerController : ControllerBase
    {

        private readonly ApplicationDBContext _context;

        public CustomerController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Customer>> Get()
        {
            return await _context.Customers.ToListAsync();
        }
   

        [HttpGet("id")]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByID(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            return customer == null ? NotFound() : Ok(customer);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByID), new {id = customer.Id}, customer);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, Customer customer)
        {
            if (id != customer.Id) return BadRequest();

            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var customerToDelete = await _context.Customers.FindAsync(id);
            if(customerToDelete == null) return NotFound();

            _context.Customers.Remove(customerToDelete);
            await _context.SaveChangesAsync();

            return NoContent();

        }
    }
}
