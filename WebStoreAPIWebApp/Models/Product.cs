namespace WebStoreAPIWebApp.Models
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int Price { get; set; }

        public virtual ICollection<ProductCart> ProductCarts { get; set; } = new List<ProductCart>();
        public virtual Category Category { get; set; }
    }
}
