using System.Collections.Generic;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Utils
{
    public static class Extensions
    {
        public static PickupTicketOrderDTO ToListPickupTicketOrder(this List<PickupTicketOrder> ticketOrder)
        {
            return new PickupTicketOrderDTO { Items = ticketOrder };
        }
        public static PickupTicketItemDTO ToPickupTicketItemDTO(this PickupTicketItem pickupTicket)
        {
            return new PickupTicketItemDTO { Item = pickupTicket };
        }
    }
}
