namespace WebStoreAPIWebApp.Models.DTO
{
    public class CartDTO : Entity
    {
        public int CustomerId { get; set; }
        public string DeliveryAddress { get; set; }
    }
}
