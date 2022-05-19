using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace Visum.Services.Mobile.Entities
{
    public class DispatchDtl
    {
        public string OriginDtlType { get; set; }
        public decimal OriginDtlQty { get; set; }
        public string OriginDtlDscr { get; set; }
        public string OriginDtlDscr2 { get; set; }
        public decimal OriginDtlWeight { get; set; }
        public decimal OriginDtlHeight { get; set; }
        public decimal OriginDtlLength { get; set; }
        public decimal OriginDtlWidth { get; set; }
        public int OriginDtlNewLine { get; set; }
        public int OriginDtlNo { get; set; }
        public int OriginDtlOrderNo { get; set; }
    }
}
