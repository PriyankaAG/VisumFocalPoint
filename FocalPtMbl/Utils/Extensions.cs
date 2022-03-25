using System;
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
        public static string ToFormattedDate(this DateTime? date)
        {
            return date == null ? "" : ((DateTime)date).ToString("MM/dd/yyyy h:mm tt");
        }
        public static string ToFormattedDate(this DateTime date)
        {
            return date.ToString("MM/dd/yyyy h:mm tt");
        }
    }
}
