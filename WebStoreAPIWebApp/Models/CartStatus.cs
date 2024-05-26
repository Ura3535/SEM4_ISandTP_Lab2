namespace WebStoreAPIWebApp.Models
{
    public class CartStatus : Entity
    {
        public string Name { get; set; }
        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
    }
}
