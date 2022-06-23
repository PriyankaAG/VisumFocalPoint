using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FocalPoint.Modules.FrontCounter.ViewModels.NewRental;
using FocalPtMbl;
using FocalPoint.Utils;
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

        //Save button
        private async void Button_Clicked(object sender, EventArgs e)
        {
            theViewModel.FormatCustomerName();

            if (!theViewModel.IsPageLoaded) return;

            if (!theViewModel.ValidateData()) return;

            if (theViewModel.CustomerToAdd != null && !string.IsNullOrEmpty(theViewModel.CustomerToAdd.CustomerPhone))
            {
                //Check if Phone number is present
                var phNoReply = await theViewModel.CheckPhoneNumber();
                var nameCheck = await theViewModel.CheckCustomerName();
                var licCheck = await theViewModel.CheckLicenseNumber();

                phNoReply = phNoReply?.Replace("\\r\\n", Environment.NewLine);
                nameCheck = nameCheck?.Replace("\\r\\n", Environment.NewLine);
                licCheck = licCheck?.Replace("\\r\\n", Environment.NewLine);

                var phNoBool = true;
                var nameBool = true;
                var licBool = true;

                if (phNoReply.HasData())
                    phNoBool = await App.Current.MainPage.DisplayAlert("Continue ?", phNoReply + Environment.NewLine + Environment.NewLine + "Do you want to continue?", "Yes", "No");
                else
                    phNoBool = true;

                if (nameCheck.HasData() && phNoBool)
                    nameBool = await App.Current.MainPage.DisplayAlert("Continue ?", nameCheck + Environment.NewLine + Environment.NewLine + "Do you want to continue?", "Yes", "No");
                else
                    nameBool = true;

                if (licCheck.HasData() && nameBool)
                    licBool = await App.Current.MainPage.DisplayAlert("Continue ?", licCheck + Environment.NewLine + Environment.NewLine + "Do you want to continue?", "Yes", "No");
                else
                    licBool = true;

                if (phNoBool && nameBool && licBool)
                {
                    var newCustomer = await theViewModel.AddCustomer();
                    if (newCustomer != null)
                    {
                        await App.Current.MainPage.DisplayAlert("Successful", "Customer added successfully.", "Ok");

                        MessagingCenter.Send(this, "CustomerSelected", newCustomer);
                        Navigation.PopAsync();
                        Navigation.PopAsync();
                    }
                    else
                    {

                        MessagingCenter.Send(this, "CustomerSelected", theViewModel.CustomerToAdd);
                        Navigation.PopAsync();
                        Navigation.PopAsync();

                        await App.Current.MainPage.DisplayAlert("Failure", "Failed to add Customer.", "Ok");
                    }
                }

            }

        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}