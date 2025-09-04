using Microsoft.AspNetCore.Identity;

namespace Feed_Bridge.Models.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string? ImgUrl { get; set; }

        public DateOnly BirthDate { get; set; }
        public StaticPage StaticPage { get; set; } = new StaticPage();

        public List<Order> Orders { get; set; } = new  List<Order> { };
        public List<Support> Supports { get; set; } = new List<Support> { };
        public List<Partener> Parteners { get; set; } = new List<Partener> { };

        public List<Notification> Notification { get; set; } = new List<Notification>();
    }
}
