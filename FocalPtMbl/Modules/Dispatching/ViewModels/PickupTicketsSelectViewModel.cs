using FocalPoint.Data;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.Dispatching.ViewModels
{
    public class PickupTicketsSelectViewModel : ThemeBaseViewModel
    {
        ObservableCollection<PickupTicket> recent;
        public ObservableCollection<PickupTicket> Recent
        {
            get => this.recent;
            private set
            {
                this.recent = value;
                OnPropertyChanged(nameof(Recent));
            }
        }
        HttpClient clientHttp;
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        public PickupTicket SelectedTicket { get; internal set; }

        public PickupTicketsSelectViewModel()
        {
            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();
        }
        internal void GetPickupTicketInfo()
        {
            try
            {
                List<PickupTicket> ticketCntAndList = null;

                Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "PickupTickets/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));

                //ClientHTTP.DefaultRequestHeaders.Add("Token", "987919a1-b105-4c16-99d8-9c8ec2b81dcf");//"3d2ad6f3-8f4a-4c47-8e8b-69f0b1a7ec08"); 70e2aad8-6216-48cc-ab13-3439970a189a
                var response = ClientHTTP.GetAsync(uri).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    ticketCntAndList = JsonConvert.DeserializeObject<List<PickupTicket>>(content);
                    if (recent == null)
                    {
                        Recent = new ObservableCollection<PickupTicket>(ticketCntAndList);
                    }
                    else
                    {
                        Recent.Clear();
                        foreach (var ticket in ticketCntAndList)
                        {
                            Recent.Add(ticket);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        internal void GetSearchedTicketInfo(object SearchText)
        {
            try
            {
                List<PickupTicket> ticketCntAndList = null;

                Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "PickupTickets/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));

                //ClientHTTP.DefaultRequestHeaders.Add("Token", "987919a1-b105-4c16-99d8-9c8ec2b81dcf");//"3d2ad6f3-8f4a-4c47-8e8b-69f0b1a7ec08"); 70e2aad8-6216-48cc-ab13-3439970a189a
                var response = ClientHTTP.GetAsync(uri).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    ticketCntAndList = JsonConvert.DeserializeObject<List<PickupTicket>>(content);
                    if (recent == null)
                    {
                        Recent = new ObservableCollection<PickupTicket>(ticketCntAndList);
                    }
                    else
                    {
                        Recent.Clear();
                        foreach (var ticket in ticketCntAndList)
                        {
                            Recent.Add(ticket);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        internal PickupTicket GetDetailedTicketInfo(PickupTicket SelectedTicket)
        {
            PickupTicket detailedTicket = new PickupTicket();
            string apiLocked = "True";
            bool isTrue = true;
            try
            {

                List<PickupTicket> ticketCntAndList = null;

                Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "PickupTicket/" + SelectedTicket.PuTNo.ToString()));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
                Uri uri2 = new Uri(string.Format(DataManager.Settings.ApiUri + "PickupTicketLock/" + SelectedTicket.PuTNo.ToString()+ "/"+ isTrue.ToString()));
                //ClientHTTP.DefaultRequestHeaders.Add("Token", "987919a1-b105-4c16-99d8-9c8ec2b81dcf");//"3d2ad6f3-8f4a-4c47-8e8b-69f0b1a7ec08"); 70e2aad8-6216-48cc-ab13-3439970a189a
                var response = ClientHTTP.GetAsync(uri).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    detailedTicket = JsonConvert.DeserializeObject<PickupTicket>(content);
                    //Lock Ticket
                        var response2 = ClientHTTP.GetAsync(uri2).GetAwaiter().GetResult();
                        if (response2.IsSuccessStatusCode)
                        {
                            string content2 = response2.Content.ReadAsStringAsync().Result;
                            bool isLocked = JsonConvert.DeserializeObject<bool>(content2);
                        if (isLocked)
                        {
                            apiLocked = "True";
                            return detailedTicket;
                        }
                        else
                            apiLocked = "False";
                        return new PickupTicket();
                        }
                }
                return new PickupTicket();
            }
            catch (Exception ex)
            {
                return new PickupTicket();
            }
        }
        internal void UnlockTicket(PickupTicket detailedTicket)
        {
            string apiLocked = "False";
            bool isLocked = true;
            try
            {

                Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "PickupTicketLock/" + detailedTicket.PuTNo.ToString() + "/" + apiLocked));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));

                //ClientHTTP.DefaultRequestHeaders.Add("Token", "987919a1-b105-4c16-99d8-9c8ec2b81dcf");//"3d2ad6f3-8f4a-4c47-8e8b-69f0b1a7ec08"); 70e2aad8-6216-48cc-ab13-3439970a189a
                var response = ClientHTTP.GetAsync(uri).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    isLocked = JsonConvert.DeserializeObject<bool>(content);
                    if (isLocked)
                        apiLocked = "True";
                    else
                        apiLocked = "false";

                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
