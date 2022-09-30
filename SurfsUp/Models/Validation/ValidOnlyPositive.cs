using System.ComponentModel.DataAnnotations;

namespace SurfsUp.Models.Validation
{
    public class ValidOnlyPositive : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is decimal t1)
                return t1 >= 0;
            else if (value is float t2)
                return t2 >= 0;
            else if (value is double t3)
                return t3 >= 0;
            else if (value is int t4)
                return t4 >= 0;
            else
                return false;
        }
    }
}
