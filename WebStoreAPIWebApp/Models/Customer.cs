namespace WebStoreAPIWebApp.Models
{
    public class Customer : Entity
    {
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
    }
}
