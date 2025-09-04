using Microsoft.AspNetCore.Identity;

namespace Feed_Bridge.Models.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string ImgUrl { get; set; }

        public DateOnly BirthDate { get; set; }
        public StaticPage staticPage { get; set; }

        public List<Order> Orders { get; set; }
        public List<Support> Supports { get; set; }
        public List<Partener> parteners { get; set; } 
    }
}
