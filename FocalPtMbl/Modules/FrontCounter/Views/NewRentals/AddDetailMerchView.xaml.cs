using FocalPoint.Modules.FrontCounter.ViewModels.NewRental;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var selectedItem = (e as TappedEventArgs).Parameter;
            selItem = (AvailabilityMerch)selectedItem;
        }

        private void AddToOrder_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
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
                        OrderUpdate UpdatedOrder = await ((AddDetailMerchViewModel)this.BindingContext).AddItem(selItem, numberOfItems, CurrentOrder);
                        if (UpdatedOrder != null && UpdatedOrder.Order != null)
                        {
                            MessagingCenter.Send<AddDetailMerchView, OrderUpdate>(this, "UpdateOrder", UpdatedOrder);
                            await Navigation.PopAsync();
                        }
                        else if (UpdatedOrder == null || UpdatedOrder.Order == null)
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
            });
        }

        private void Search_Tapped(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await ((AddDetailMerchViewModel)this.BindingContext).GetSearchedMerchInfo(SearchTextEditor.Text);
                }
                catch(Exception ex)
                {

                }
            });
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}