﻿using FocalPoint.Modules.FrontCounter.ViewModels.NewRental;
using FocalPoint.Modules.Payments.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views.NewRentals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewQuickRentalMainPage : ContentPage
    {
        NewQuickRentalMainPageViewModel theViewModel;
        public NewQuickRentalMainPage()
        {
            InitializeComponent();

            (Application.Current.MainPage as FlyoutPage).IsGestureEnabled = false;
            theViewModel = new NewQuickRentalMainPageViewModel();
            theViewModel.IsPageLoading = true;
            BindingContext = theViewModel;

            GetOrderInfo();

        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private List<string> notifications = new List<string>();
        private string selectedItem = "";

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

                //Lets say we load default values
                _ = Task.Delay(300).ContinueWith((a) =>
                  {
                      Device.BeginInvokeOnMainThread(async () =>
                      {
                          theViewModel.SetDefaultValues();
                          _ = Task.Delay(1000).ContinueWith((a) =>
                            {
                                theViewModel.IsPageLoading = false;
                            });
                      });
                  });
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

            theViewModel.SelectedItem = "Select Item Type";
            selectedItem = "Select Item Type";
        }

        public void SubscribeEvents()
        {
            MessagingCenter.Unsubscribe<NewQuickRentalSelectCustomerPage, Customer>(this, "CustomerSelected");
            MessagingCenter.Unsubscribe<NewQuickRentalAddCustomerPage, Customer>(this, "CustomerSelectedADD");
            MessagingCenter.Unsubscribe<OrderNotesView, Tuple<string, string>>(this, "NotesAdded");

            MessagingCenter.Subscribe<NewQuickRentalSelectCustomerPage, Customer>(this, "CustomerSelected", async (sender, customer) =>
            {
                (BindingContext as NewQuickRentalMainPageViewModel).SelectedCustomer = customer;
                (BindingContext as NewQuickRentalMainPageViewModel).RefreshAllProperties();
                UpdateTheOrder(customer);
            });
            MessagingCenter.Subscribe<NewQuickRentalAddCustomerPage, Customer>(this, "CustomerSelectedADD", async (sender, customer) =>
            {
                (BindingContext as NewQuickRentalMainPageViewModel).SelectedCustomer = customer;
                (BindingContext as NewQuickRentalMainPageViewModel).RefreshAllProperties();
                UpdateTheOrder(customer);
            });
            MessagingCenter.Subscribe<OrderNotesView, Tuple<string, string>>(this, "NotesAdded", async (sender, theNotes) =>
            {
                UpdateTheOrder((BindingContext as NewQuickRentalMainPageViewModel).SelectedCustomer, theNotes);
            });
        }

        public async void UpdateTheOrder(Customer customer, Tuple<string, string> theNotes = null)
        {
            var orderRefresh = await (BindingContext as NewQuickRentalMainPageViewModel).UpdateCust(customer, theNotes);
            AfterUpdate_OrderProcessing(orderRefresh);
        }

        public async void AfterUpdate_OrderProcessing(OrderUpdate orderRefresh)
        {
            try
            {
                if (orderRefresh == null) return;
                if (orderRefresh.Answers != null && orderRefresh.Answers.Count > 0)
                {
                    while (orderRefresh.Answers != null && orderRefresh.Answers.Count > 0)
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
                            orderRefresh = await (BindingContext as NewQuickRentalMainPageViewModel).UpdateCust(updateOrder: (BindingContext as NewQuickRentalMainPageViewModel).OrderUpdate);
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
                // await OpenDetailPage((orderRefresh));}
            }
            catch (Exception)
            {
            }
        }

        private async void myPicker_ItemSelected(object sender, CustomControls.ItemSelectedEventArgs e)
        {
            var data = e.SelectedIndex;
            if (e.IsFirstRowPlaceholder && e.SelectedIndex != 0)
            {
                selectedItem = myPicker.ItemsSource[e.SelectedIndex];
            }
            if (selectedItem == "" || selectedItem.ToLower() == "Select Item Type".ToLower())
            {
                //await DisplayAlert("Select Type", "Please select a search type", "OK");
                return;
            }
            if (selectedItem == "Merchandise")
            {
                AddDetailMerchView addDetailMerchView = new AddDetailMerchView();
                AddDetailMerchViewModel addDetailMerchViewModel = new AddDetailMerchViewModel();
                addDetailMerchViewModel.CurrentOrder = ((NewQuickRentalMainPageViewModel)BindingContext).CurrentOrder;
                addDetailMerchView.BindingContext = addDetailMerchViewModel;
                await Navigation.PushAsync(addDetailMerchView);
            }
            else
            {
                AddDetailRentalView addDetailRentalView = new AddDetailRentalView();
                AddDetailRentalViewModel addDetailRentalViewModel = new AddDetailRentalViewModel(selectedItem, 1);
                addDetailRentalViewModel.CurrentOrder = ((NewQuickRentalMainPageViewModel)BindingContext).CurrentOrder;
                addDetailRentalView.BindingContext = addDetailRentalViewModel;
                await Navigation.PushAsync(addDetailRentalView);
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            var a = myPicker.SelectedItem;
            var b = myPicker.SelectedIndex;
            (BindingContext as NewQuickRentalMainPageViewModel).SelectedItem = "Rentals";
        }

        private async void LabelDropDownCustomControl_ItemSelected(object sender, CustomControls.ItemSelectedEventArgs e)
        {
            if (theViewModel.IsPageLoading) return;

            (BindingContext as NewQuickRentalMainPageViewModel).GetEndDateAndTimeValues();

            var orderRefresh = await (BindingContext as NewQuickRentalMainPageViewModel).UpdateDateValues();
            AfterUpdate_OrderProcessing(orderRefresh);
        }

        private async void AddDetails_Clicked(object sender, EventArgs e)
        {
            if (selectedItem == "")
            {
                await DisplayAlert("Select Type", "Please select a search type", "OK");
                return;
            }
            if (selectedItem == "Merchandise")
            {
                AddDetailMerchView addDetailMerchView = new AddDetailMerchView();
                AddDetailMerchViewModel addDetailMerchViewModel = new AddDetailMerchViewModel();
                addDetailMerchViewModel.CurrentOrder = ((NewQuickRentalMainPageViewModel)BindingContext).CurrentOrder;
                addDetailMerchView.BindingContext = addDetailMerchViewModel;
                await Navigation.PushAsync(addDetailMerchView);
            }
            else
            {
                AddDetailRentalView addDetailRentalView = new AddDetailRentalView();
                AddDetailRentalViewModel addDetailRentalViewModel = new AddDetailRentalViewModel(selectedItem, 1);
                addDetailRentalViewModel.CurrentOrder = ((NewQuickRentalMainPageViewModel)BindingContext).CurrentOrder;
                addDetailRentalView.BindingContext = addDetailRentalViewModel;
                await Navigation.PushAsync(addDetailRentalView);
            }
        }

        private async void VoidOrder_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool doVoid = await DisplayAlert("Void", "Are you sure you want to void?", "OK", "Cancel");
                if (doVoid)
                {
                    if (await ((NewQuickRentalMainPageViewModel)BindingContext).VoidOrder())
                    {
                        //TODO: Navigate to the Main Page
                    }
                    else
                        await DisplayAlert("Void", "Void did not succeed try again", "OK");
                }


            (Application.Current.MainPage as FlyoutPage).IsGestureEnabled = true;
            }
            catch (Exception ex)
            {
                //TODO: log error
            }
        }

        private async void InternalNotes_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(QOInternalNV);
        }

        private async void PrintNotes_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(QOPrintNV);
        }

        private async void TotalBreakout_Clicked(object sender, EventArgs e)
        {
            // await Navigation.PushAsync(QOTotalBV);
        }

        private async void Payment_Clicked(object sender, EventArgs e)
        {
            ViewOrderEntityComponent viewOrderEntityComponent = new ViewOrderEntityComponent();
            var orderDetails = await viewOrderEntityComponent.GetOrderDetails(((NewQuickRentalMainPageViewModel)this.BindingContext).CurrentOrder?.OrderNo ?? 0);
            if (orderDetails != null)
            {
                await Navigation.PushAsync(new PaymentView(orderDetails));
            }
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new OrderNotesView(new Tuple<string, string>(theViewModel.CurrentOrder.OrderIntNotes, theViewModel.CurrentOrder.OrderNotes)));

        }

        private async void Button_Clicked_3(object sender, EventArgs e)
        {
            var orderRefresh = await (BindingContext as NewQuickRentalMainPageViewModel).UpdateDateValues();
            AfterUpdate_OrderProcessing(orderRefresh);

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var a = (e as TappedEventArgs).Parameter;
            this.Navigation.PushAsync(new EditDetailOfSelectedItemView(a as OrderDtl));
        }

        private void Button_Clicked_4(object sender, EventArgs e)
        {
            var vm = (BindingContext as NewQuickRentalMainPageViewModel);
            this.Navigation.PushAsync(new TotalBreakoutView(vm.CurrentOrder));
        }

        private async void LabelDropDownCustomControl_ItemSelected_1(object sender, CustomControls.ItemSelectedEventArgs e)
        {
            if (theViewModel.IsPageLoading) return;
            var orderRefresh = await (BindingContext as NewQuickRentalMainPageViewModel).UpdateDateValues();
            AfterUpdate_OrderProcessing(orderRefresh);
        }

        private async void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (theViewModel.IsPageLoading) return;
            var orderRefresh = await (BindingContext as NewQuickRentalMainPageViewModel).UpdateDateValues();
            AfterUpdate_OrderProcessing(orderRefresh);
        }

        private async void TimePicker_Unfocused(object sender, FocusEventArgs e)
        {
            if (theViewModel.IsPageLoading) return;
            var orderRefresh = await (BindingContext as NewQuickRentalMainPageViewModel).UpdateDateValues();
            AfterUpdate_OrderProcessing(orderRefresh);
        }
    }
}