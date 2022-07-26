using System;
using System.Collections.Generic;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Utils
{
    public static class Extensions
    {
        public static string ToFormattedDate(this DateTime? date)
        {
            return date == null ? "" : ((DateTime)date).ToString("MM/dd/yyyy hh:mm tt");
        }
        public static string ToFormattedDate(this DateTime date)
        {
            return date.ToString("MM/dd/yyyy hh:mm tt");
        }

        public static bool HasData(this string result)
        {
            return !string.IsNullOrEmpty(result) && !string.IsNullOrWhiteSpace(result);
        }

        public static bool IsFirstCharacterNumber(this string stringNumber)
        {
            return char.IsNumber(stringNumber.ToCharArray()[0]);
        }
    }
}
