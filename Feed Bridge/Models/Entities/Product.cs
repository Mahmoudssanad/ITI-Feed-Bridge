using Microsoft.CodeAnalysis.Options;

namespace Feed_Bridge.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImgURL { get; set; }

        public decimal Quantity { get; set; }

        public DateOnly ExpirDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<Cart> Carts {  get; set; }
        public List<Order> Orders {  get; set; }
    }
}
