using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.Payments.Entity
{
    public class VoidRequest
    {
        public Payment Pay { get; set; }

        [JsonProperty(PropertyName = "Pay.PaymentVoid")]
        public bool PayVoid { get; set; }
    }
}
