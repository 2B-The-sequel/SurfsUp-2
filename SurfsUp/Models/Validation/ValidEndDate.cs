using System;
using System.ComponentModel.DataAnnotations;
using SurfsUp.Models;

namespace SurfsUp.Models.Validation
{
   
        public class ValidEndDate : ValidationAttribute
        {
            
            protected override ValidationResult
                    IsValid(object value, ValidationContext validationContext)
            {
                var model = (Models.Rental)validationContext.ObjectInstance;
                DateTime _endRental = Convert.ToDateTime(value);
                DateTime _startRental = Convert.ToDateTime(model.StartRental);
                TimeSpan span = _endRental.Subtract(_startRental);

                if (_startRental > _endRental)
                {
                    return new ValidationResult
                        ("End rentaldate must be greater than start rentaldate.");
                }
                else if (_endRental < DateTime.Now)
                {
                    return new ValidationResult
                        ("End rentaldate must be greater than current date.");
                }
                else if (span.Hours > 168)
                {
                    return new ValidationResult
                        ("A board cannot be rented for more than 7 days");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
        }
    }

