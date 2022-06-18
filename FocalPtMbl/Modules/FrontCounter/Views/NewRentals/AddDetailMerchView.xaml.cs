using FocalPoint.Modules.FrontCounter.ViewModels.NewRental;
using System;
using System.Collections.Generic;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views.NewRentals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddDetailMerchView : ContentPage
    {
        public AddDetailMerchView()
        {
            InitializeComponent();
        }

        private AvailabilityMerch selItem;

        private Order currentOrder = new Order();
        public Order CurrentOrder
        {
            get { return currentOrder; }
            set
            {
                if (currentOrder != value)
                {
                    currentOrder = value;
                }
            }
        }

        private void TextEdit_Completed(object sender, EventArgs e)
        {
            ((AddDetailMerchViewModel)this.BindingContext).GetSearchedMerchInfo((sender as Entry).Text);
        }
        private void TextEdit_Cleared(object sender, EventArgs e)
        {
            ((AddDetailMerchViewModel)this.BindingContext).GetSearchedMerchInfo("");
        }

        private async void AddToOrder_Clicked(object sender, EventArgs e)
        {
            List<string> selectedSerials = new List<string>();
            string result = "0";

            //if serials exist popup a selection dialog select multi? 
            if (selItem != null)
            {
                //Check Serialized on selcted item
                if (selItem.AvailSerialized)
                {
                    //get the serial numbers
                    await Navigation.PushAsync(new SelectSerialOnlyView(selItem));
                }
                else
                {
                    result = await DisplayPromptAsync("Pick Quantity", "Enter in the Quantity", keyboard: Keyboard.Numeric);
                }

                if (result != null && Convert.ToDecimal(result) > 0)
                {
                    decimal numberOfItems = Convert.ToDecimal(result);
                    OrderUpdate UpdatedOrder = ((AddDetailMerchViewModel)this.BindingContext).AddItem(selItem, numberOfItems, CurrentOrder);
                    if (UpdatedOrder != null && UpdatedOrder.Order != null)
                    {
                        //TODO: Need to check usage
                        //MessagingCenter.Send<QuickOrderDetailsMerchView, OrderUpdate>(this, "Hi", UpdatedOrder);
                        await Navigation.PopAsync();
                    }
                    else if (UpdatedOrder.Order == null)
                    {
                        //ask questions ' Show Message and return no numbers for not assigning else return number to assign equal to qty
                        await Navigation.PushAsync(new SelectSerialOnlyView(selItem));
                    }
                    else
                    {
                        await DisplayAlert("Item not added", "Item not added", "ok");
                    }
                }
            }
            else
                await DisplayAlert("Select Item", "Please Search and select an Item.", "ok");
        }
    }
}