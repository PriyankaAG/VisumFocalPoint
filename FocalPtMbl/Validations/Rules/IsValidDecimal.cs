using System;
using System.Collections.Generic;
using System.Text;
using FocalPoint.Utils;

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
            var newText = value.ToString();
            if (newText != null && !newText.IsFirstCharacterNumber())
                newText = newText.Substring(1);
            var amt = newText;
            return decimal.TryParse(amt, out decimal res) && res > 0;
        }
    }
}
