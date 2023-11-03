using Microsoft.EntityFrameworkCore;
using ACMECorpCustomerService.Models;

namespace ACMECorpCustomerService.Data
{
    public class ApplicationDBContext : DbContext
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options) { 

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Item> Items { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
