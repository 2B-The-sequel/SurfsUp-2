using System.ComponentModel.DataAnnotations;

namespace SurfsUpLibrary.Models.Validation
{
    public class ValidStartDate : ValidationAttribute
    {

        protected override ValidationResult
               IsValid(object value, ValidationContext validationContext)
        {
            DateTime _dateJoin = Convert.ToDateTime(value);
            if (_dateJoin.Day == DateTime.Now.Day || _dateJoin >= DateTime.Now)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult
                    ("Start Lejedato skal være senere end nuværende dato");
            }
        }
    }
}