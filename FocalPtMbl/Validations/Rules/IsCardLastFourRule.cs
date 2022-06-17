﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPoint.Validations.Rules
{
    class IsCardLastFourRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value as string;
            return !(str.Length < 4);
        }
    }
}
