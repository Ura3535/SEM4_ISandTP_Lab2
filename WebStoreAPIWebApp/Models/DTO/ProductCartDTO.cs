namespace WebStoreAPIWebApp.Models.DTO
{
    public class ProductCartDTO : Entity
    {
        public int ProductId { get; set; }
        public int CartId { get; set; }
        public int Quantity { get; set; }
    }
}
