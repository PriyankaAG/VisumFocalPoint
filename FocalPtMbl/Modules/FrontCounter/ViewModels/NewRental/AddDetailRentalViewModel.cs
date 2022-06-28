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
    class AddDetailRentalViewModel: ThemeBaseViewModel
    {
        public AddDetailRentalViewModel(string itemType, Int16 searchIn = 1) : base(itemType)
        {
            NewQuickRentalEntityComponent = new NewQuickRentalEntityComponent();
            SearchIn = searchIn;
            ItemType = itemType;
        }

        OrderUpdate orderUpdate;

        public INewQuickRentalEntityComponent NewQuickRentalEntityComponent { get; set; }

        public Int16 SearchIn;
        public string ItemType;

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

        ObservableCollection<AvailabilityRent> recent;
        public ObservableCollection<AvailabilityRent> Recent
        {
            get => this.recent;
            private set
            {
                this.recent = value;
                OnPropertyChanged(nameof(Recent));
            }
        }

        internal async Task GetSearchedCustomersInfo(string text)
        {
            //update searchText
            if (text == null)
                text = "%";
            List<AvailabilityRent> customersCntAndList = null;
            try
            {
                Indicator = true;
                if (ItemType == "Rate Table")
                    customersCntAndList = await NewQuickRentalEntityComponent.GetAvailabilityKits(text, SearchIn);
                else if (ItemType == "Rental Saleable")
                    customersCntAndList = await NewQuickRentalEntityComponent.GetAvailabilitySalable(text, SearchIn);
                else
                    customersCntAndList = await NewQuickRentalEntityComponent.GetAvailabilityRentals(text, SearchIn);

                if (customersCntAndList != null)
                {
                    if (recent == null)
                    {
                        Recent = new ObservableCollection<AvailabilityRent>(customersCntAndList);
                    }
                    else
                    {
                        Recent.Clear();
                        foreach (var customer in customersCntAndList)
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
                OnPropertyChanged(nameof(Recent));
            }
            catch (Exception ex)
            {
                //TODO: Log Error
            }
            finally
            {
                Indicator = false;
            }
        }

        internal async Task<Tuple<OrderUpdate, QuestionFaultExceptiom>> AddItem(AvailabilityRent selItem, decimal numOfItems, Order curOrder, OrderUpdate myOrderUpdate, QuestionFaultExceptiom result)
        {
            // success no questions needed
            result = null;
            try
            {
                Indicator = true;
                orderUpdate = new OrderUpdate();
                //update searchText
                OrderAddItem RentalItem = new OrderAddItem();
                RentalItem.OrderNo = curOrder.OrderNo;
                RentalItem.AvailItem = selItem.AvailItem;
                RentalItem.Quantity = numOfItems;
                if (myOrderUpdate != null)
                    RentalItem.Answers = myOrderUpdate.Answers;
                OrderUpdate OrderToUpDate = new OrderUpdate();
                var responseOrderUpdate = await NewQuickRentalEntityComponent.OrderAddRental(RentalItem);
                Indicator = false;
                if (responseOrderUpdate.IsSuccessStatusCode)
                {
                    string orderContent = responseOrderUpdate.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new JsonGenericDictionaryOrArrayConverter() } };

                    orderUpdate = JsonConvert.DeserializeObject<OrderUpdate>(orderContent, settings);
                    orderUpdate.Answers.Clear();
                    return new Tuple<OrderUpdate, QuestionFaultExceptiom>(orderUpdate, null);
                }
                if (responseOrderUpdate.StatusCode == System.Net.HttpStatusCode.ExpectationFailed)
                {
                    string readErrorContent = responseOrderUpdate.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new JsonGenericDictionaryOrArrayConverter() } };

                    result = JsonConvert.DeserializeObject<QuestionFaultExceptiom>(readErrorContent, settings);
                }
                return new Tuple<OrderUpdate, QuestionFaultExceptiom>(orderUpdate, result);
            }
            catch (Exception ex)
            {
                result = null;
                return null;
            }
            finally
            {
                Indicator = false;
            }
        }
    }
}
