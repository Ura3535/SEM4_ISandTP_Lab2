namespace WebStoreAPIWebApp.Models
{
    public class Cart : Entity
    {
        public int CustomerId { get; set; }
        public string DeliveryAddress { get; set; }
        public int Price { get; set; }
        public int CartStatusId { get; set; }
        public virtual ICollection<ProductCart> ProductCarts { get; set; } = new List<ProductCart>();
        public virtual Customer? Customer { get; set; }
        public virtual CartStatus? CartStatus { get; set; }
    }
}
