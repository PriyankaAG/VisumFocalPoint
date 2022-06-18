using FocalPoint.Components.EntityComponents;
using FocalPoint.Components.Interface;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.FrontCounter.ViewModels.NewRental
{
    class AddDetailRentalViewModel: ThemeBaseViewModel
    {
        public AddDetailRentalViewModel()
        {
            NewQuickRentalEntityComponent = new NewQuickRentalEntityComponent();
        }

        OrderUpdate orderUpdate;

        public INewQuickRentalEntityComponent NewQuickRentalEntityComponent { get; set; }

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

        internal OrderUpdate AddItem(AvailabilityRent selItem, decimal numOfItems, Order curOrder, OrderUpdate myOrderUpdate, out QuestionFaultExceptiom result)
        {
            // success no questions needed
            result = null;
            try
            {
                orderUpdate = new OrderUpdate();
                //update searchText
                OrderAddItem RentalItem = new OrderAddItem();
                RentalItem.OrderNo = curOrder.OrderNo;
                RentalItem.AvailItem = selItem.AvailItem;
                RentalItem.Quantity = numOfItems;
                if (myOrderUpdate != null)
                    RentalItem.Answers = myOrderUpdate.Answers;
                OrderUpdate OrderToUpDate = new OrderUpdate();
                var responseOrderUpdate = NewQuickRentalEntityComponent.OrderAddRental(RentalItem).GetAwaiter().GetResult();
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

                    result = JsonConvert.DeserializeObject<QuestionFaultExceptiom>(readErrorContent, settings);
                }
                return orderUpdate;
            }
            catch (Exception ex)
            {
                result = null;
                return null;
            }
        }
    }
}
