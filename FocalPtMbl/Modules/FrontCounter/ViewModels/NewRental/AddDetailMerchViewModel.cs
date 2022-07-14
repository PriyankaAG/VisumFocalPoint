using FocalPoint.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.FrontCounter.ViewModels.NewRental
{
    public class AddDetailMerchViewModel : AddDetailCommonViewModel
    {
        public AddDetailMerchViewModel() : base("Merchandise")
        {
            populateSearchInList();
        }

        private string Search = "%";

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

        public override void populateSearchInList()
        {
            SearchInList = new String[4];
            SearchInList[0] = "Description";
            SearchInList[1] = "UPC Number";
            SearchInList[2] = "Part Number";
            SearchInList[3] = "Extended";
        }

        public override async Task GetSearchedCustomersInfo(string text)
        {
            try
            {
                //update searchText
                Search = text;

                if (string.IsNullOrEmpty(Search))
                    Search = "%";
                Indicator = true;
                SearchIn = (short)Utils.Utils.GetEnumValueFromDescription<AvailSearchIns>(SelectedSearchIn);
                List<AvailabilityMerch> merchCntAndList = null;
                merchCntAndList = await NewQuickRentalEntityComponent.GetAvailabilityMerchandise(Search, SearchIn);
                if (merchCntAndList != null)
                {
                    //merchCntAndList = merchCntAndList.Where(x => x.AvailSerialized == true).ToList();
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
