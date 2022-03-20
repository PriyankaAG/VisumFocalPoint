using System.Collections.Generic;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Utils
{
    public static class Extensions
    {
        public static ListPickupTicketOrder ToListPickupTicketOrder(this List<PickupTicketOrder> ticketOrder)
        {
            return new ListPickupTicketOrder { Items = ticketOrder };
        }
    }
}
