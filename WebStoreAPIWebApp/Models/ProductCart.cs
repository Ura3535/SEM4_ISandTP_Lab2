namespace WebStoreAPIWebApp.Models
{
    public class ProductCart : Entity
    {
        public int ProductId { get; set; }
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public virtual Product? Product { get; set; }
        public virtual Cart? Cart { get; set; }
    }
}
