using FocalPoint.Components.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint
{
    public class PickupTicketEntityComponent : IPickupTicketEntityComponent
    {
        IAPICompnent apiComponent;

        const string PickupTicket = "PickupTicket/{0}";
        const string PickupTickets = "PickupTickets/";
        const string PickupTicketDetails = "PickupTickets/{0}";
        const string PickupTicketLock = "PickupTicket/Lock/{0}/{1}";
        const string PickupTicketItemCount = "PickupTicket/ItemCount";
        const string PickupTicketCounted = "PickupTicket/Counted";
        const string PickupTicketOrderItems = "PickupTicket/OrderItems/{0}";
        const string PickupTicketOrderCreate= "PickupTicket/OrderItems";

        public PickupTicketEntityComponent()
        {
            apiComponent = new APIComponent();
        }

        public async Task<PickupTicket> GetPickupTicket(string ticketNumber)
        {
            PickupTicket pickupTicket = null;
            try
            {
                pickupTicket = await apiComponent.GetAsync<PickupTicket>(string.Format(PickupTicket, ticketNumber));
                
            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return pickupTicket;
        }

        public async Task<List<PickupTicket>> GetPickupTickets()
        {
            List<PickupTicket> pickupTickets = null;
            try
            {
                pickupTickets = await apiComponent.GetAsync<List<PickupTicket>>(PickupTickets);
            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return pickupTickets;
        }

        public async Task<bool> LockPickupTicket(string ticketNumber, string apiLocked)
        {
            bool result = false;
            try
            {
                result = await apiComponent.GetAsync<bool>(string.Format(PickupTicketLock, ticketNumber, apiLocked));
            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return result;
        }

        public async Task<PickupTicket> GetPickupTicketDetails(string ticketNumber)
        {
            PickupTicket pickupTicket = null;
            try
            {
                pickupTicket = await apiComponent.GetAsync<PickupTicket>(string.Format(PickupTicketDetails, ticketNumber));
            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return pickupTicket;
        }

        public async Task<bool> PostPickupTicketItemCount(PickupTicketItem selectedDetail)
        {
            bool result = false;
            try
            {
                string requestContent = JsonConvert.SerializeObject(selectedDetail);
                result = await apiComponent.PostAsync<bool>(PickupTicketItemCount, requestContent);
            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return result;
        }

        public async Task<bool> PostPickupTicketCounted(PickupTicketCounted request)
        {
            bool result = false;
            try
            {
                string requestContent = JsonConvert.SerializeObject(request);
                result = await apiComponent.PostAsync<bool>(PickupTicketCounted, requestContent);
            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return result;
        }

        public async Task<List<PickupTicketOrder>> PickupTicketOrder(int puTNo)
        {

            List<PickupTicketOrder> pickupTicketOrders = null;
            try
            {
                pickupTicketOrders = await apiComponent.GetAsync<List<PickupTicketOrder>>(string.Format(PickupTicketOrderItems, puTNo));

            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return pickupTicketOrders;
        }

        async public Task<bool> PickupTicketCreate(ListPickupTicketOrder lstPickupTicketOrders)
        {
            bool result = false;
            try
            {
                string requestContent = JsonConvert.SerializeObject(lstPickupTicketOrders);
                result = await apiComponent.PostAsync<bool>(PickupTicketOrderCreate, requestContent);
            }
            catch (Exception ex)
            {
                //TODO: Log error
            }
            return result;
        }
    }
}
