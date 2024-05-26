namespace WebStoreAPIWebApp.Models
{
    public class Status : Entity
    {
        public string Name { get; set; }
        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
    }
}
