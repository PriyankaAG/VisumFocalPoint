using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Visum.Services.Mobile.Entities
{
    [DataContract]
    public class QuestionFaultException
    {
        public QuestionFaultException(string message, int code)
        {
            Code = code;
            Message = message;
        }

        [DataMember]
        public int Code { get; private set; }

        [DataMember]
        public string Message { get; private set; }
    }
}
