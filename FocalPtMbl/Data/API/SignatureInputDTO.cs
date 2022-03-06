using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPoint.Data.API
{
    public class SignatureInputDTO
    {
        public int DocKind { get; set; }

        public int RecordID { get; set; }

        public string Stat { get; set; }

        public int Format { get; set; }

        public string Signature { get; set; }

        public string Waiver { get; set; }
    }
}
