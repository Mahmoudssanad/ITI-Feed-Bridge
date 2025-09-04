
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

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<Cart> Carts { get; set; } = new List<Cart>(); 
        public List<Order> Orders { get; set; } = new List<Order>();

        [ForeignKey("Donation")]
        public int DonationId { get; set; }
        public Donation Donation { get; set; } = new Donation();
    }
}
