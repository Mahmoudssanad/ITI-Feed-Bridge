using Feed_Bridge.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Feed_Bridge.ViewModel
{
    public class DonationViewModel
    {
        [RegularExpression(@"^[\u0600-\u06FFa-zA-Z\s]{2,50}$", ErrorMessage = "الاسم يجب أن يتكون من اكتر من حرفين واقل من 50 حرف")]
        public string Name { get; set; }
        public IFormFile? Image { get; set; }

        [Display(Name = "تاريخ انتهاء الصلاحيه")]
        public DateOnly ExpirDate { get; set; }
        public decimal Quantity { get; set; }

        [RegularExpression(@"^[\u0600-\u06FFa-zA-Z\s]{2,50}$", ErrorMessage = "العنوان يجب أن يتكون من اكتر من حرفين واقل من 50 حرف")]
        public string Address { get; set; }

        [RegularExpression(@"^01[0-9]{9}$", ErrorMessage = "رقم الهاتف غير صحيح يجب ان يكون مكون من 11 رقم ويبدأ ب 01")]
        public string Phone { get; set; }
        public string? Description { get; set; }
        public ProductCategory Category { get; set; }
    }
}