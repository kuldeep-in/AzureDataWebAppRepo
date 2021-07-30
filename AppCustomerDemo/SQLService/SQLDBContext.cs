
namespace AppCustomerDemo.SQLService
{
    using Microsoft.EntityFrameworkCore;
    using AppCustomerDemo.Models;

    public class SQLDBContext : DbContext
    {
        public SQLDBContext(DbContextOptions<SQLDBContext> options) : base(options)
        {
        }

        public DbSet<Order> DataPortal_Orders { get; set; }
    }
}
