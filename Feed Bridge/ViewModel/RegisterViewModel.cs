using Feed_Bridge.CutomVaildation;
using System.ComponentModel.DataAnnotations;

namespace Feed_Bridge.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "الاسم مطلوب")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "كلمة السر غير متطابقة")]
        public string Password { get; set; }

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمة المرور غير متطابقة")]
        public string ConfirmPassword { get; set; }

        public string? PhoneNumber { get; set; }

        public DateOnly BirthDate { get; set; }

        public string? ImgUrl { get; set; }
    }
}
