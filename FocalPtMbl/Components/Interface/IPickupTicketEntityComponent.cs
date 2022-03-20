using System.Collections.Generic;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Components.Interface
{
    public interface IPickupTicketEntityComponent
    {
        Task<List<PickupTicket>> GetPickupTickets();
        Task<PickupTicket> GetPickupTicket(string ticketNumber);
        Task<bool> LockPickupTicket(string ticketNumber, string apiLocked);
        Task<PickupTicket> GetPickupTicketDetails(string ticketNumber);
        Task<bool> PostPickupTicketItemCount(PickupTicketItem selectedDetail);
        Task<bool> PostPickupTicketCounted(PickupTicketCounted request);
        Task<List<PickupTicketOrder>> PickupTicketOrder(int puTNo);
        Task<bool> PickupTicketCreate(ListPickupTicketOrder lstPickupTicketOrders);
    }
}
