using Feed_Bridge.CutomVaildation;
using System.ComponentModel.DataAnnotations;

namespace Feed_Bridge.ViewModel
{
    public class RegisterViewModel
    {
        [Display(Name = "الاسم")]
        public string Name { get; set; }

        [Display(Name = "رقم التليفون")]
        public string PhoneNumber { get; set; }

        [Display(Name = "تاريخ الميلاد")]
        [AgeValidation]
        public DateOnly BirthDate { get; set; }

        [Display(Name = "البريد الإلكتروني")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "كلمة السر")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "كلمة السر غير متطابقة")]
        public string Password { get; set; }

        [Display(Name = "تأكيد كلمة السر")]
        [Compare("Password", ErrorMessage = "كلمة السر غير متطابقة")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "تذكرني")]
        public bool RememberMe { get; set; }
    }
}
