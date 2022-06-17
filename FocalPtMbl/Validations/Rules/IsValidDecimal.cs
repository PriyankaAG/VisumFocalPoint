using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPoint.Validations.Rules
{
    class IsValidDecimal<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }
            var amt = value.ToString().Trim('$');
            return decimal.TryParse(amt, out decimal res) && res > 0;
        }
    }
}
