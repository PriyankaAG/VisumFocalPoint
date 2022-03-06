using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;

namespace Visum.Services.Mobile.Entities
{
    [DataContract()]
    public class QuestionAnswer
    {
        public QuestionAnswer(int code, string answer)
        {
            Code = code;
            Answer = answer;
        }

        [DataMember]
        public int Code { get; set; }

        [DataMember]
        public string Answer { get; set; }
    }

    [DataContract()]
    public class OrderAddItem
    {
        [DataMember]
        public int OrderNo { get; set; }

        [DataMember]
        public int AvailItem { get; set; }

        [DataMember]
        public Decimal Quantity { get; set; }

        [DataMember]
        public Boolean AsKit { get; set; }

        [DataMember]
        public Double MeterOut { get; set; }

        [DataMember]
        public Boolean AsSalable { get; set; }

        [DataMember]
        public List<string> Serials { get; set; } = new List<string>();

        [DataMember]
        public List<QuestionAnswer> Answers { get; set; } = new List<QuestionAnswer>();

    }
}
