using FocalPoint.Data;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.ServiceDepartment.ViewModels
{
    public class WorkOrderFormViewModel : ThemeBaseViewModel
    {
        ObservableCollection<WorkOrder> recent;
        public ObservableCollection<WorkOrder> Recent
        {
            get => this.recent;
            private set
            {
                this.recent = value;
                OnPropertyChanged(nameof(Recent));
            }
        }

        private WorkOrder selectedWorkOrder;
        public WorkOrder SelectedWorkOrder
        {
            get { return selectedWorkOrder; }
            set
            {
                if (selectedWorkOrder != value)
                {
                    selectedWorkOrder = value;
                }
            }
        }

        public WorkOrderFormViewModel(): base("View Work Orders")
        {
            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();
            Task.Run(() =>
            {
                //_ = GetWorkOrdersInfo();
                _ = GetSearchedWorkOrdersInfo("");
            });
        }

        internal WorkOrder GetWorkOrderDetail(WorkOrder WO)
        {
            try
            {
                string WONo = WO.WONo.ToString();
                WorkOrder selectedWorkOrder = new WorkOrder();
                Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "WorkOrder/" + WONo));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));

                var response = ClientHTTP.GetAsync(uri).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    selectedWorkOrder = JsonConvert.DeserializeObject<WorkOrder>(content);
                }
                return selectedWorkOrder;
            }
            catch (Exception ex)
            {
                return new WorkOrder();
            }
        }

        HttpClient clientHttp;
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        //List<Vendor> vendList = new List<Data.DataLayer.Vendor>();
        private int StoreID = 0;
        private string SearchText = "";
        private int StartIdx = 0;
        private int MaxCnt = 100;
        public async Task<WorkOrders> GetWorkOrdersInfo()
        {
            Indicator = true;
            WorkOrders woCntAndList = null;
            try
            {

                Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "WorkOrders/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
                var stringContent = new StringContent(
                                          JsonConvert.SerializeObject(new { SearchText, StartIdx, MaxCnt }),
                                          Encoding.UTF8,
                                          "application/json");


                var response = await ClientHTTP.PostAsync(uri, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    woCntAndList = JsonConvert.DeserializeObject<WorkOrders>(content);
                    StartIdx = woCntAndList.TotalCnt;
                    if (recent == null)
                    {
                        Recent = new ObservableCollection<WorkOrder>(woCntAndList.List);
                    }
                    else
                    {
                        foreach (var workOrder in woCntAndList.List)
                        {
                            Recent.Add(workOrder);
                        }
                    }

                }
                return woCntAndList;
            }
            catch (Exception ex)
            { return woCntAndList; }
            finally
            {
                Indicator = false;
            }
        }

        internal async Task GetSearchedWorkOrdersInfo(string text)
        {
            Indicator = true;
            Recent?.Clear();
            SearchText = text;
            StartIdx = 0;
            StoreID = DataManager.Settings.HomeStore;
            WorkOrders woCntAndList = null;
            try
            {
                Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "WorkOrders/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
                var stringContent = new StringContent(
                                          JsonConvert.SerializeObject(new { StoreID, SearchText, StartIdx, MaxCnt }),
                                          Encoding.UTF8,
                                          "application/json");

                var response = await ClientHTTP.PostAsync(uri, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    woCntAndList = JsonConvert.DeserializeObject<WorkOrders>(content);
                    StartIdx = woCntAndList.TotalCnt;
                    Recent = new ObservableCollection<WorkOrder>(woCntAndList.List);

                    //if (recent == null)
                    //{
                    //    Recent = new ObservableCollection<WorkOrder>(woCntAndList.List);
                    //}
                    //else
                    //{
                    //    foreach (var workOrder in woCntAndList.List)
                    //    {
                    //        Recent.Add(workOrder);
                    //    }
                    //}

                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Indicator = false;
            }
        }
    }
}
