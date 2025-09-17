using System.ComponentModel.DataAnnotations;

namespace Feed_Bridge.CutomVaildation
{
    public class AgeValidation : ValidationAttribute
    {
        private readonly DateOnly _birthdate;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // عشان اتاكد انها جايالي بالنوع اللي انا عاوزه value لل cast بنعمل 
            if (value is not DateOnly birthdate)
            {
                return new ValidationResult("Invaild Date syntax");
            }

            var today = DateTime.Today;

            var result = today.Year - birthdate.Year;

            if (result < 18)
            {
                return new ValidationResult("يجب الا يقل العمر عن 18");
            }

            return ValidationResult.Success;
        }
    }
}
