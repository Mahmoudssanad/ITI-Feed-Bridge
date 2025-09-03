using Microsoft.AspNetCore.Identity;

namespace Feed_Bridge.Models.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string ImgUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public DateOnly BirthDate { get; set; }
    }
}
