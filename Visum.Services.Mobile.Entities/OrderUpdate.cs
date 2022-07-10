using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Collections;

namespace Visum.Services.Mobile.Entities
{
    [DataContract()]
    public class OrderUpdate
    {
        [Serializable]
        public enum OrderSaveTypes
        {
            SaveOnly = 0,
            ExitOnly = 1,
            ExitAsQuote = 2
       }

        [DataMember]
        public Order Order { get; set; }

        [DataMember]
        public List<QuestionAnswer> Answers { get; set; }  = new List<QuestionAnswer>();

        [DataMember]
        public List<string> Notifications { get; set; } = new List<string>();

        [DataMember]
        public string CustomerMessage { get; set; }

        [DataMember]
        public OrderSaveTypes Save { get; set; }
    }
}
