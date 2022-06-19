using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FocalPoint.Modules.FrontCounter.ViewModels.NewRental;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views.NewRentals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewQuickRentalAddCustomerPage : ContentPage
    {
        NewQuickRentalAddCustomerViewModel theViewModel;
        public NewQuickRentalAddCustomerPage()
        {
            InitializeComponent();
            Title = "Add New Customer";
            theViewModel = new NewQuickRentalAddCustomerViewModel();
            BindingContext = theViewModel;

            theViewModel.FetchMasters().ContinueWith((a) =>
            {
                //Lets say we load default values
                Task.Delay(1000).ContinueWith((a) =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        theViewModel.SetDefaultValues();
                        _ = Task.Delay(2000).ContinueWith((a) =>
                          {
                              theViewModel.IsPageLoaded = true;
                          });
                    });
                });
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            theViewModel.FormatCustomerName();

            if (!theViewModel.IsPageLoaded) return;

            if (!theViewModel.ValidateData()) return;

            theViewModel.AddCustomer();
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}