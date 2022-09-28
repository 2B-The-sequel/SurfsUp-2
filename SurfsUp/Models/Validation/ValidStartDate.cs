using System.ComponentModel.DataAnnotations;

namespace SurfsUp.Models.Validation
{
    public class ValidStartDate : ValidationAttribute
    {

        protected override ValidationResult
               IsValid(object value, ValidationContext validationContext)
        {
            DateTime _dateJoin = Convert.ToDateTime(value);
            if (_dateJoin > DateTime.Now)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult
                    ("Start rentaldate must be greater than current date.");
            }
        }
    }
}