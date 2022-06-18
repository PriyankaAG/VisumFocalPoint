using FocalPoint.Components.EntityComponents;
using FocalPoint.Components.Interface;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.FrontCounter.ViewModels.NewRental
{
    public class AddDetailMerchViewModel : ThemeBaseViewModel
    {
        public AddDetailMerchViewModel()
        {
            NewQuickRentalEntityComponent = new NewQuickRentalEntityComponent();
        }

        OrderUpdate orderUpdate;
        private string Search = "%";
        private Int16 SearchIn = 1;

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

        internal void GetSearchedMerchInfo(string text)
        {
            try
            {
                //update searchText
                Search = text;
                List<AvailabilityMerch> merchCntAndList = null;

                merchCntAndList = NewQuickRentalEntityComponent.GetAvailabilityMerchandise(Search).GetAwaiter().GetResult();
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
            catch (Exception ex)
            {

            }
        }

        internal OrderUpdate AddItem(AvailabilityMerch selItem, decimal numOfItems, Order curOrder)
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
                var responseOrderUpdate = NewQuickRentalEntityComponent.OrderAddMerchandise(MerchItem).GetAwaiter().GetResult();
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
