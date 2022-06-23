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
            GetOrderInfo();
        }

        private List<string> notifications = new List<string>();
        private void GetOrderInfo()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                notifications = await ((NewQuickRentalMainPageViewModel)BindingContext).CreateNewOrder();
                if (notifications != null)
                {
                    if (notifications.Count > 0)
                        foreach (var notification in notifications)
                            await DisplayAlert("Notification", notification, "OK");
                }
                //else
                //{
                //    await DisplayAlert("Problem Creating an order", "There was a problem creating the order. Please try again with a better connection", "OK");
                //}
            });
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
                UpdateTheOrder(customer);
            });
            MessagingCenter.Subscribe<NewQuickRentalAddCustomerPage, Customer>(this, "CustomerSelected", async (sender, customer) =>
            {
                (BindingContext as NewQuickRentalMainPageViewModel).SelectedCustomer = customer;
                (BindingContext as NewQuickRentalMainPageViewModel).RefreshAllProperties();
                UpdateTheOrder(customer);
            });

        }

        public async void UpdateTheOrder(Customer customer)
        {

            var orderRefresh = await (BindingContext as NewQuickRentalMainPageViewModel).UpdateCust(customer);
            if (orderRefresh != null) return;
            if (orderRefresh.Answers != null && orderRefresh.Answers.Count > 0)
            {
                while (orderRefresh.Answers != null || orderRefresh.Answers.Count > 0)
                {
                    //display answer if key does not have a value update that value
                    //checkUpdate.Answers.
                    if (orderRefresh.Answers.Count > 0)
                    {
                        // KIRK REM KeyValuePair<int, string> question = new KeyValuePair<int, string>();
                        QuestionAnswer question = null;
                        foreach (var answer in orderRefresh.Answers)
                        {
                            //0 or 1 is not found
                            if (!(answer.Answer == "True" || answer.Answer == "False"))
                            {
                                question = answer;
                                break;
                            }
                        }
                        bool custOk = await DisplayAlert("Customer Options", question.Answer, "OK", "Cancel");
                        orderRefresh.Answers.Find(qa => qa.Code == question.Code).Answer = custOk.ToString();
                        // KIRK REM orderRefresh.Answers[question.Key] = custOk.ToString();
                        (BindingContext as NewQuickRentalMainPageViewModel).OrderUpdate = orderRefresh;
                        orderRefresh = await (BindingContext as NewQuickRentalMainPageViewModel).UpdateCust(customer);
                    }

                }

            }
            if (orderRefresh.Notifications.Count > 0)
            {
                foreach (var notification in orderRefresh.Notifications)
                {
                    await DisplayAlert("Customer Notification", notification, "OK");
                }
            }
            if (orderRefresh.CustomerMessage != null && orderRefresh.CustomerMessage.Length > 0)
            {
                await DisplayAlert("Customer Message", orderRefresh.CustomerMessage, "OK");
            }
            // await OpenDetailPage((orderRefresh));
        }

        private async void myPicker_ItemSelected(object sender, CustomControls.ItemSelectedEventArgs e)
        {
            var data = e.SelectedIndex;
            if (e.IsFirstRowPlaceholder && e.SelectedIndex != 0)
            {
                var selected = myPicker.ItemsSource[e.SelectedIndex];
                //AddDetailRentalView rentalView = new AddDetailRentalView(1);
                //await Navigation.PushAsync(rentalView);

                DisplayAlert("Great!!", $"You chose {selected}", "Cancel");
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            var a = myPicker.SelectedItem;
            var b = myPicker.SelectedIndex;
            (BindingContext as NewQuickRentalMainPageViewModel).SelectedItem = "Rentals";
        }

        private void LabelDropDownCustomControl_ItemSelected(object sender, CustomControls.ItemSelectedEventArgs e)
        {
            (BindingContext as NewQuickRentalMainPageViewModel).GetEndDateAndTimeValues();
        }
    }
}