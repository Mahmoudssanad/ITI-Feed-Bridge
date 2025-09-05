using System.ComponentModel.DataAnnotations.Schema;

namespace Feed_Bridge.Models.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("Order")]
        public int? OrderId { get; set; }
        public Order Order { get; set; }

        [ForeignKey("Donation")]
        public int? DonationId { get; set; }
        public Donation? Donation { get; set; }

        [ForeignKey("Support")]
        public int? SupportId { get; set; }
        public Support Support { get; set; }


    }
}
