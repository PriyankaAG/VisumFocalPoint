using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FocalPoint.Modules.FrontCounter.ViewModels.NewRental;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views.NewRentals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewQuickRentalSelectCustomerPage : ContentPage
    {
        public NewQuickRentalSelectCustomerPage()
        {
            InitializeComponent();
            Title = "Select Customer";
            NewQuickRentalSelectCustomerViewModel theViewModel = new NewQuickRentalSelectCustomerViewModel();
            BindingContext = theViewModel;

            Device.BeginInvokeOnMainThread(async () =>
            {
                await theViewModel.GetCustomers();
            });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var addCustomerPage = new NewQuickRentalAddCustomerPage();
            this.Navigation.PushAsync(addCustomerPage);
        }

        private void ItemSelected(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var result = e.Item as Customer;
                MessagingCenter.Send(this, "CustomerSelected", result);
                this.Navigation.PopAsync();
            }
        }
    }
}