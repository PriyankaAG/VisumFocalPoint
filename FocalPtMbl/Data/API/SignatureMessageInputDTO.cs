using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPoint.Data.API
{
    public class SignatureMessageInputDTO
    {
        public int DocKind { get; set; }

        public int RecordID { get; set; }

        public string Stat { get; set; }
    }
}
