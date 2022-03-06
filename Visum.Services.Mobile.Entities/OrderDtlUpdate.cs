using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Visum.Services.Mobile.Entities
{
    public class OrderDtlUpdate
    {
        public OrderDtl  Detail { get; set; }
        public List<QuestionAnswer> Answers { get; set; }  = new List<QuestionAnswer>();
    }
}
