using System.ComponentModel.DataAnnotations.Schema;

namespace Feed_Bridge.Models.Entities
{
    public class Donation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ImgURL { get; set; }
        public DateOnly ExpirDate { get; set; }
        public decimal Quantity { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string? Description { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public List<Product> Product { get; set; } = new List<Product>();
        public Notification Notification { get; set; } = new Notification();
    }
}
