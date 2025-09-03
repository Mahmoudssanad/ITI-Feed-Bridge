namespace Feed_Bridge.Models.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }

        public List<Order> Orders { get; set; }

        public List<Product> Products { get; set; }
    }
}
