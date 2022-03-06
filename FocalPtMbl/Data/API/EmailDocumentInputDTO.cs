using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPoint.Data.API
{
    public class EmailDocumentInputDTO
    {
        public int DocKind { get; set; }
        public int RecordID { get; set; }
        public string ToAddr { get; set; }
    }
}
