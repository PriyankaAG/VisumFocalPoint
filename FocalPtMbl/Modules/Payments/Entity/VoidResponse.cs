using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.Payments.Entity
{
    public class VoidResponse
    {
        public string[] Notifications { get; set; }
        public Payment Payment { get; set; }
    }
}
