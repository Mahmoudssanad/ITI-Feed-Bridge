
using System.ComponentModel.DataAnnotations.Schema;

namespace Feed_Bridge.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? ImgURL { get; set; }

        public decimal Quantity { get; set; }

        public DateOnly ExpirDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        public List<ProductCart> ProductCarts { get; set; } = new List<ProductCart>();

        public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

        [ForeignKey("Donation")]
        public int DonationId { get; set; }
        public Donation Donation { get; set; }
    }
}
