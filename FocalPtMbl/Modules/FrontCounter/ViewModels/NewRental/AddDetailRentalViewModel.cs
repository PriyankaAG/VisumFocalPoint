using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.FrontCounter.ViewModels.NewRental
{
    public class AddDetailRentalViewModel: AddDetailCommonViewModel
    {
        public AddDetailRentalViewModel(string itemType) : base(itemType)
        {
            populateSearchInList();
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

        public override void populateSearchInList()
        {
            SearchInList = new String[4];
            SearchInList[0] = "Description";
            SearchInList[1] = "Equipment ID";
            SearchInList[2] = "Barcode";
            SearchInList[3] = "Serial Number";
            OnPropertyChanged(nameof(SearchInList));
        }

        public override async Task GetSearchedInfo(string text)
        {
            //update searchText
            if (text == null)
            {
                if (SelectedSearchIn == "Item Number")
                    text = "";
                else
                    text = "%";
            }
            if (SelectedSearchIn == "Top 20")
                text = "20";

            List<AvailabilityRent> customersCntAndList = null;
            try
            {
                Indicator = true;
                customersCntAndList = await NewQuickRentalEntityComponent.GetAvailabilityRentals(text, Utils.Utils.GetEnumValueFromDescription<AvailSearchIns>(SelectedSearchIn), CurrentOrder.OrderType, AvailSearchFors.Rentals);

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

        internal async Task<Tuple<OrderUpdate, QuestionFaultExceptiom, string>> AddItem(AvailabilityRent selItem, decimal numOfItems, Order curOrder, OrderUpdate myOrderUpdate, QuestionFaultExceptiom result)
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
                string errorMessage = string.Empty;
                var responseOrderUpdate = await NewQuickRentalEntityComponent.OrderAddRental(RentalItem);
                Indicator = false;
                if (responseOrderUpdate.IsSuccessStatusCode)
                {
                    string orderContent = responseOrderUpdate.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new JsonGenericDictionaryOrArrayConverter() } };

                    orderUpdate = JsonConvert.DeserializeObject<OrderUpdate>(orderContent, settings);
                    orderUpdate.Answers.Clear();
                    return new Tuple<OrderUpdate, QuestionFaultExceptiom, string>(orderUpdate, null, null);
                }
                if (responseOrderUpdate.StatusCode == System.Net.HttpStatusCode.ExpectationFailed)
                {
                    string readErrorContent = responseOrderUpdate.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new JsonGenericDictionaryOrArrayConverter() } };

                    result = JsonConvert.DeserializeObject<QuestionFaultExceptiom>(readErrorContent, settings);
                }
                else if (!responseOrderUpdate.IsSuccessStatusCode)
                {
                    errorMessage = await responseOrderUpdate.Content.ReadAsStringAsync();
                }
                return new Tuple<OrderUpdate, QuestionFaultExceptiom, string>(orderUpdate, result, errorMessage);
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
