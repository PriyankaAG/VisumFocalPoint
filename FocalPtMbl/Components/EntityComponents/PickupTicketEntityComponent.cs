using FocalPoint.Components.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        const string PickupTicketItem = "PickupTicket/Item/{0}";
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
            PickupTicket pickupTicket;
            try
            {
                pickupTicket = await apiComponent.GetAsync<PickupTicket>(string.Format(PickupTicket, ticketNumber));
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return pickupTicket;
        }

        public async Task<List<PickupTicket>> GetPickupTickets()
        {
            List<PickupTicket> pickupTickets;
            try
            {
                pickupTickets = await apiComponent.GetAsync<List<PickupTicket>>(PickupTickets);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return pickupTickets;
        }

        public async Task<bool> LockPickupTicket(string ticketNumber, string apiLocked)
        {
            bool result;
            try
            {
                result = await apiComponent.GetAsync<bool>(string.Format(PickupTicketLock, ticketNumber, apiLocked));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<PickupTicket> GetPickupTicketDetails(string ticketNumber)
        {
            PickupTicket pickupTicket;
            try
            {
                pickupTicket = await apiComponent.GetAsync<PickupTicket>(string.Format(PickupTicketDetails, ticketNumber));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return pickupTicket;
        }

        public async Task<bool> PostPickupTicketItemCount(PickupTicketItem pickupTicket)
        {
            bool result;
            try
            {

                string requestContent = JsonConvert.SerializeObject(new { Item = pickupTicket });
                result = await apiComponent.PostAsync<bool>(PickupTicketItemCount, requestContent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<bool> PostPickupTicketCounted(PickupTicketCounted request)
        {
            bool result;
            try
            {
                string requestContent = JsonConvert.SerializeObject(request);
                result = await apiComponent.PostAsync<bool>(PickupTicketCounted, requestContent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<List<PickupTicketOrder>> GetPickupTicketOrder(int puTNo)
        {

            List<PickupTicketOrder> pickupTicketOrders;
            try
            {
                pickupTicketOrders = await apiComponent.GetAsync<List<PickupTicketOrder>>(string.Format(PickupTicketOrderItems, puTNo));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return pickupTicketOrders;
        }

        async public Task<bool> PostPickupTicketCreate(List<PickupTicketOrder> pickupTicketOrders)
        {
            bool result;
            try
            {
                string requestContent = JsonConvert.SerializeObject(new { Items = pickupTicketOrders });
                result = await apiComponent.PostAsync<bool>(PickupTicketOrderCreate, requestContent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<PickupTicketItem> GetPickupTicketItem(int puDtlNo)
        {
            PickupTicketItem pickupTicketItem;
            try
            {
                pickupTicketItem = await apiComponent.GetAsync<PickupTicketItem>(string.Format(PickupTicketItem, puDtlNo));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return pickupTicketItem;
        }
    }
}
