using Microsoft.AspNetCore.Identity;

namespace Feed_Bridge.Models.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string ImgUrl { get; set; }

        public DateOnly BirthDate { get; set; }
<<<<<<< HEAD
        public StaticPage staticPage { get; set; }

        public List<Order> Orders { get; set; }
        public List<Support> Supports { get; set; }
        public List<Partener> parteners { get; set; }
=======
>>>>>>> 4570d32ee069cf5a2c94847c58f5b8da98aefa16
    }
}
