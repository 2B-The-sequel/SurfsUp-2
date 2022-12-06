using SurfsUpLibrary.Models;
using System.ComponentModel.DataAnnotations;
using SurfsUpBlazor.Client.Pages;

namespace SurfsUpBlazor.Client.Pages.Validation
{
    public class WeatherEndDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (WeatherDate)validationContext.ObjectInstance;
            DateTime _endDate = Convert.ToDateTime(value);
            DateTime _startDate = Convert.ToDateTime(model.StartDate);
            TimeSpan span = _endDate.Subtract(_startDate);

            if (_startDate > _endDate)
            {
                return new ValidationResult
                    ("Slutdato for vejr skal være større end startdato");
            }
            else if (_endDate < DateTime.Now)
            {
                return new ValidationResult
                    ("Slutdato for vejr skal være større end den nuværende dato");
            }
            else if (span.TotalHours > 168)
            {
                return new ValidationResult
                    ("Vejret kan ikke vises i mere end for 7 dage");
            }
            else
            {
                return ValidationResult.Success;
            }
        }

    }
}
