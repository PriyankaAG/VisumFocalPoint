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
    public partial class NewQuickRentalMainPage : ContentPage
    {
        public NewQuickRentalMainPage()
        {
            InitializeComponent();
            BindingContext = new NewQuickRentalMainPageViewModel();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Subscribe<NewQuickRentalMainPage>(this, "Hi", (sender) =>
            {
            });

            this.Navigation.PushAsync(new NewQuickRentalSelectCustomerPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SubscribeEvents();
        }

        public void SubscribeEvents()
        {
            MessagingCenter.Unsubscribe<NewQuickRentalSelectCustomerPage, Customer >(this, "CustomerSelected");

            MessagingCenter.Subscribe<NewQuickRentalSelectCustomerPage, Customer>(this, "CustomerSelected", async (sender, customer) =>
            {
                (BindingContext as NewQuickRentalMainPageViewModel).SelectedCustomer = customer;
                (BindingContext as NewQuickRentalMainPageViewModel).RefreshAllProperties();
            });
        }
    }
}