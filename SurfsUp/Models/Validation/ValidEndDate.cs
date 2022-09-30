using System;
using System.ComponentModel.DataAnnotations;
using SurfsUp.Models;

namespace SurfsUp.Models.Validation
{
        public class ValidEndDate : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var model = (Models.Rental)validationContext.ObjectInstance;
                DateTime _endRental = Convert.ToDateTime(value);
                DateTime _startRental = Convert.ToDateTime(model.StartRental);
                TimeSpan span = _endRental.Subtract(_startRental);

                if (_startRental > _endRental)
                {
                    return new ValidationResult
                        ("Slutdato for lejen skal være større end startdato");
                }
                else if (_endRental < DateTime.Now)
                {
                    return new ValidationResult
                        ("Slutdato for lejen skal være større end den nuværende dato");
                }
                else if (span.TotalHours > 168)
                {
                    return new ValidationResult
                        ("Et board kan ikke udlejes i mere end 7 dage");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
        }
    }

