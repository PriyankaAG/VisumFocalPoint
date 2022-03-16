using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FocalPoint.Components.Common.Interface;
using FocalPoint.Components.Interface;
using Newtonsoft.Json;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Components.EntityComponents
{
    public class PickupTicketEntityComponent : IPickupTicketEntityComponent
    {
        IAPICompnent apiComponent;

        const string PickupTicket = "PickupTicket/{0}";
        const string PickupTickets = "PickupTickets/";
        const string PickupTicketDetails = "PickupTickets/{0}";
        const string PickupTicketLock = "PickupTicketLock/{0}/{1}";
        const string PickupTicketItemCount = "PickupTicketItemCount";
        const string PickupTicketCounted = "PickupTicket/Counted";

        public PickupTicketEntityComponent()
        {
            apiComponent = new APIComponent();
        }

        public async Task<PickupTicket> GetPickupTicket(string ticketNumber)
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await apiComponent.GetAsync(string.Format(PickupTicket, ticketNumber));
                if (httpResponseMessage?.IsSuccessStatusCode ?? false)
                {
                    string content = await httpResponseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<PickupTicket>(content);
                    return response;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<List<PickupTicket>> GetPickupTickets()
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await apiComponent.GetAsync(PickupTickets);
                if (httpResponseMessage?.IsSuccessStatusCode ?? false)
                {
                    string content = await httpResponseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<List<PickupTicket>>(content);
                    return response;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<bool> LockPickupTicket(string ticketNumber, string apiLocked)
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await apiComponent.GetAsync(string.Format(PickupTicketLock, ticketNumber, apiLocked));
                if (httpResponseMessage?.IsSuccessStatusCode ?? false)
                {
                    string content = await httpResponseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<bool>(content);
                    return response;
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public async Task<PickupTicket> GetPickupTicketDetails(string ticketNumber)
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await apiComponent.GetAsync(string.Format(PickupTicketDetails, ticketNumber));
                if (httpResponseMessage?.IsSuccessStatusCode ?? false)
                {
                    string content = await httpResponseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<PickupTicket>(content);
                    return response;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<bool> PostPickupTicketItemCount(PickupTicketItem selectedDetail)
        {
            try
            {
                string requestContent = JsonConvert.SerializeObject(selectedDetail);
                HttpResponseMessage httpResponseMessage = await apiComponent.PostAsyc(PickupTicketItemCount, requestContent);
                if (httpResponseMessage?.IsSuccessStatusCode ?? false)
                {
                    string content = await httpResponseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<bool>(content);
                    return response;
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public async Task<bool> PostPickupTicketCounted(string puTNo)
        {
            try
            {
                string requestContent = JsonConvert.SerializeObject(puTNo);
                HttpResponseMessage httpResponseMessage = await apiComponent.PostAsyc(PickupTicketCounted, requestContent);
                if (httpResponseMessage?.IsSuccessStatusCode ?? false)
                {
                    string content = await httpResponseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<bool>(content);
                    return response;
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }
    }
}
