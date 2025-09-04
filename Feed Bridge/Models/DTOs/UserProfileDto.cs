namespace Feed_Bridge.Models.DTOs
{
    public class UserProfileDto
    {
        public string ImgUrl { get; set; }
        public DateOnly BirthDate { get; set; }
        public string PhoneNumber { get; set; }  
        public string Email { get; set; }       
    }

    
    public class UpdateUserProfileDto
    {
        public string ImgUrl { get; set; }
        public DateOnly BirthDate { get; set; }
        public string PhoneNumber { get; set; }
    }
}
