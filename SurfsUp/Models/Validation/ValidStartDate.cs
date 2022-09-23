using System.ComponentModel.DataAnnotations;

namespace SurfsUp.Models.Validation
{
    public class ValidStartDate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime _dateJoin = Convert.ToDateTime(value);
            return _dateJoin < DateTime.Now;
        }
    }
}