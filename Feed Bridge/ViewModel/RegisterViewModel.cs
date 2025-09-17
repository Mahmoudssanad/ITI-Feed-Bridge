using Feed_Bridge.CutomVaildation;
using System.ComponentModel.DataAnnotations;

namespace Feed_Bridge.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "الاسم مطلوب")]
        [Display(Name = "الاسم")]
        [RegularExpression(@"^[\u0600-\u06FFa-zA-Z\s]{2,50}$", ErrorMessage = "الاسم يجب أن يتكون من اكتر من حرفين واقل من 50 حرف")]
        public string UserName { get; set; }

        [Display(Name = "البريد الالكتروني")]
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "كلمه المرور")]
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]

        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "كلمة السر غير متطابقة")]
        [Display(Name = "تأكيد كلمه المرور")]
        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "رقم الهاتف")]
        [RegularExpression(@"^01[0-9]{9}$", ErrorMessage = "رقم الهاتف غير صحيح يجب ان يكون مكون من 11 رقم ويبدأ ب 01")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "تاريح الميلاد  مطلوب")]
        [Display(Name = "تاريخ الميلاد")]
        [AgeValidation]
        public DateOnly BirthDate { get; set; }

        [Display(Name = "الصورة الشخصية")]
        public IFormFile? ImgFile { get; set; }

        [Display(Name = "العنوان ")]
        public string? Address { get; set; }


    }
}