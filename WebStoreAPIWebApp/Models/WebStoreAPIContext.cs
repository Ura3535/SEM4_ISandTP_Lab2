using Microsoft.EntityFrameworkCore;

namespace WebStoreAPIWebApp.Models
{
    public class WebStoreAPIContext : DbContext
    {
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCart> ProductCarts { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }

        public WebStoreAPIContext(DbContextOptions<WebStoreAPIContext> options) 
        : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
