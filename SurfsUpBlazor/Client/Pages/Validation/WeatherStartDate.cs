using System.ComponentModel.DataAnnotations;

namespace SurfsUpBlazor.Client.Pages.Validation
{
    public class WeatherStartDate : ValidationAttribute
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
                        ("Start dato for vejr må ikke være før den nuværende dato");
                }
            }
        
    }
}
