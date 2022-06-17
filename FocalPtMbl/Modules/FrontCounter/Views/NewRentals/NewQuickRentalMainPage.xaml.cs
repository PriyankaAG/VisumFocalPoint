﻿using System;
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
            this.Navigation.PushAsync(new NewQuickRentalSelectCustomerPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SubscribeEvents();
        }

        public void SubscribeEvents()
        {
            MessagingCenter.Unsubscribe<NewQuickRentalSelectCustomerPage, Customer>(this, "CustomerSelected");

            MessagingCenter.Subscribe<NewQuickRentalSelectCustomerPage, Customer>(this, "CustomerSelected", async (sender, customer) =>
            {
                (BindingContext as NewQuickRentalMainPageViewModel).SelectedCustomer = customer;
                (BindingContext as NewQuickRentalMainPageViewModel).RefreshAllProperties();
            });
        }

        private void myPicker_ItemSelected(object sender, CustomControls.ItemSelectedEventArgs e)
        {
            var data = e.SelectedIndex;
            if (e.IsFirstRowPlaceholder && e.SelectedIndex != 0)
            {
                var selected = myPicker.ItemsSource[e.SelectedIndex];
                DisplayAlert("Great!!", $"You chose {selected}", "Cancel");
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            var a = myPicker.SelectedItem;
            var b = myPicker.SelectedIndex;
            (BindingContext as NewQuickRentalMainPageViewModel).SelectedItem = "Rentalsss";
        }
    }
}