using DevExpress.XamarinForms.Core.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPoint.Data.API
{
    public class CompatibleResults
    {
        public int APILowVersion { get; set; }
        public int APIHighVersion { get; set; }
        public Version FocalPtVersion { get; set; }
        public string LocalIP { get; set; }
    }
}
