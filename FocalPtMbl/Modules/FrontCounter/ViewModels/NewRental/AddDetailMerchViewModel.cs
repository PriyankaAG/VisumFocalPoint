using FocalPoint.Components.EntityComponents;
using FocalPoint.Components.Interface;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.FrontCounter.ViewModels.NewRental
{
    public class AddDetailMerchViewModel : ThemeBaseViewModel
    {
        public AddDetailMerchViewModel() : base("Merchandise")
        {
            NewQuickRentalEntityComponent = new NewQuickRentalEntityComponent();
            populateSearchInList();
        }

        OrderUpdate orderUpdate;
        private string Search = "%";
        public Int16 SearchIn;

        Order _currentOrder;
        public Order CurrentOrder
        {
            get
            {
                return _currentOrder;
            }
            set
            {
                _currentOrder = value;
            }
        }

        public INewQuickRentalEntityComponent NewQuickRentalEntityComponent { get; set; }

        ObservableCollection<AvailabilityMerch> recent;
        public ObservableCollection<AvailabilityMerch> Recent
        {
            get => this.recent;
            private set
            {
                this.recent = value;
                OnPropertyChanged(nameof(Recent));
            }
        }

        ObservableCollection<string> searchInList;
        public ObservableCollection<string> SearchInList
        {
            get => this.searchInList;
            private set
            {
                this.searchInList = value;
            }
        }

        private string selectedSearchIn;
        public string SelectedSearchIn
        {
            get => selectedSearchIn;
            set
            {
                this.selectedSearchIn = value;
            }
        }

        private void populateSearchInList()
        {
            SearchInList = new ObservableCollection<string>();
            SearchInList.Add("Description");
            SearchInList.Add("UPC Number");
            SearchInList.Add("Part Number");
            SearchInList.Add("Extended");
        }

        internal async Task GetSearchedMerchInfo(string text)
        {
            try
            {
                //update searchText
                Search = text;

                if (string.IsNullOrEmpty(Search))
                    Search = "%";
                Indicator = true;
                SearchIn = Utils.Utils.GetEnumValueFromDescription<short>(SelectedSearchIn);
                List<AvailabilityMerch> merchCntAndList = null;
                merchCntAndList = await NewQuickRentalEntityComponent.GetAvailabilityMerchandise(Search, SearchIn);
                if (merchCntAndList != null)
                {
                    //StartIdx = customersCntAndList.TotalCnt;
                    if (recent == null)
                    {
                        Recent = new ObservableCollection<AvailabilityMerch>(merchCntAndList);
                    }
                    else
                    {
                        Recent.Clear();
                        foreach (var customer in merchCntAndList)
                        {
                            Recent.Add(customer);
                        }
                    }
                }
                else
                {
                    if (recent != null)
                        Recent.Clear();
                }
                Indicator = false;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Indicator = false;
            }
        }

        internal async Task<OrderUpdate> AddItem(AvailabilityMerch selItem, decimal numOfItems, Order curOrder)
        {
            try
            {
                orderUpdate = new OrderUpdate();
                //update searchText
                OrderAddItem MerchItem = new OrderAddItem();
                MerchItem.OrderNo = curOrder.OrderNo;
                MerchItem.AvailItem = selItem.AvailItem;
                MerchItem.Quantity = numOfItems;
                OrderUpdate OrderToUpDate = new OrderUpdate();
                var responseOrderUpdate = await NewQuickRentalEntityComponent.OrderAddMerchandise(MerchItem);
                if (responseOrderUpdate.IsSuccessStatusCode)
                {
                    string orderContent = responseOrderUpdate.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new JsonGenericDictionaryOrArrayConverter() } };
                    orderUpdate = JsonConvert.DeserializeObject<OrderUpdate>(orderContent, settings);
                    orderUpdate.Answers.Clear();
                    return orderUpdate;
                }
                if (responseOrderUpdate.StatusCode == System.Net.HttpStatusCode.ExpectationFailed)
                {
                    string readErrorContent = responseOrderUpdate.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new JsonGenericDictionaryOrArrayConverter() } };

                    QuestionFaultExceptiom questionFaultExceptiom = JsonConvert.DeserializeObject<QuestionFaultExceptiom>(readErrorContent, settings);

                    orderUpdate.Answers.Add(new QuestionAnswer(questionFaultExceptiom.Code, ""));

                    throw new Exception(questionFaultExceptiom.Code.ToString());
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "1005")
                    return new OrderUpdate();
                else return null;
            }
            return null;
        }
    }
}
