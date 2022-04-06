using System;
using System.Collections.Generic;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Utils
{
    public static class Extensions
    {
        public static string ToFormattedDate(this DateTime? date)
        {
            return date == null ? "" : ((DateTime)date).ToString("MM/dd/yyyy h:mm tt");
        }
        public static string ToFormattedDate(this DateTime date)
        {
            return date.ToString("MM/dd/yyyy h:mm tt");
        }
    }
}
