﻿using FocalPoint.Data.API;
using FocalPoint.MainMenu.ViewModels;
using FocalPoint.MainMenu.Views;
using FocalPoint.Modules.FrontCounter.ViewModels.NewRental;
using FocalPoint.Modules.Payments.Types;
using FocalPoint.Modules.Payments.Views;
using FocalPoint.Utils;
using FocalPtMbl.MainMenu.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Visum.Services.Mobile.Entities.OrderUpdate;

namespace FocalPoint.Modules.FrontCounter.Views.NewRentals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewQuickRentalMainPage : ContentPage
    {
        NewQuickRentalMainPageViewModel theViewModel;
        public StoreSettings StoreSettingsProp { get; set; }
        public bool IsPaymentVoidMade { get; set; }
        public NewQuickRentalMainPage()
        {
            InitializeComponent();

            (Application.Current.MainPage as FlyoutPage).IsGestureEnabled = false;
            theViewModel = new NewQuickRentalMainPageViewModel();
            theViewModel.IsPageLoading = true;
            BindingContext = theViewModel;

            SubscribeEvents();

            GetOrderInfo();

        }

        ~NewQuickRentalMainPage()
        {

            UnSubscribeMessagingCenter();

        }
        public void UnSubscribeMessagingCenter()
        {
            MessagingCenter.Unsubscribe<NewQuickRentalSelectCustomerPage, Customer>(this, "CustomerSelected");
            MessagingCenter.Unsubscribe<NewQuickRentalAddCustomerPage, Customer>(this, "CustomerSelectedADD");
            MessagingCenter.Unsubscribe<OrderNotesView, Tuple<string, string>>(this, "NotesAdded");
            MessagingCenter.Unsubscribe<EditDetailOfSelectedItemView, Tuple<Order, OrderDtl>>(this, "NotesAdded");
            MessagingCenter.Unsubscribe<PaymentKindPage, bool>(this, "PaymentComplete");
            MessagingCenter.Unsubscribe<PaymentHistoryDetail, bool>(this, "PaymentVoid");
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
                if (theViewModel.CurrentOrderUpdate == null ||
                theViewModel.CurrentOrderUpdate.Order == null)
                {
                    await DisplayAlert("Alert!", "Issue while creating a new order." + Environment.NewLine + "Please try after some time.", "OK");
                    NavigateToDashboard();

                    return;
                }

                StoreSettingsProp = await ((NewQuickRentalMainPageViewModel)BindingContext).GetStoreSettings();

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

            theViewModel.SelectedItem = "Select Item Type";
            selectedItem = "Select Item Type";

            (Application.Current.MainPage as MainMenuFlyout).IsQuickRentalScreenDisplaying = true;

            if (IsPaymentVoidMade)
            {
                IsPaymentVoidMade = false;
                RefreshCurrentOrder(true);
            }
        }

        public void SubscribeEvents()
        {
            UnSubscribeMessagingCenter();

            MessagingCenter.Subscribe<NewQuickRentalSelectCustomerPage, Customer>(this, "CustomerSelected", async (sender, customer) =>
            {
                //// SUSHIL: Check this back
                if ((BindingContext as NewQuickRentalMainPageViewModel).SelectedCustomer?.CustomerNo != customer.CustomerNo)
                {
                    (BindingContext as NewQuickRentalMainPageViewModel).SelectedCustomer = customer;
                    (BindingContext as NewQuickRentalMainPageViewModel).RefreshAllProperties();
                    UpdateTheOrder(customer);
                }
            });
            MessagingCenter.Subscribe<NewQuickRentalAddCustomerPage, Customer>(this, "CustomerSelectedADD", async (sender, customer) =>
            {
                (BindingContext as NewQuickRentalMainPageViewModel).SelectedCustomer = customer;
                (BindingContext as NewQuickRentalMainPageViewModel).RefreshAllProperties();
                UpdateTheOrder(customer);
            });
            MessagingCenter.Subscribe<OrderNotesView, Tuple<string, string>>(this, "NotesAdded", async (sender, theNotes) =>
            {
                if ((BindingContext as NewQuickRentalMainPageViewModel).CurrentOrder?.OrderIntNotes != theNotes.Item1
                || (BindingContext as NewQuickRentalMainPageViewModel).CurrentOrder?.OrderNotes != theNotes.Item2)
                {
                    UpdateTheOrder((BindingContext as NewQuickRentalMainPageViewModel).SelectedCustomer, theNotes);
                }
            });
            MessagingCenter.Subscribe<EditDetailOfSelectedItemView, Tuple<Order, OrderDtl>>(this, "OrderDetailUpdated", async (a, tup) =>
            {
                var retOrder = tup.Item1;
                var retOrderDtl = tup.Item2;
                (BindingContext as NewQuickRentalMainPageViewModel).ReloadOrderDetailItems(retOrder, retOrderDtl);
            });
            MessagingCenter.Subscribe<PaymentKindPage, bool>(this, "PaymentComplete", async (sender, args) =>
            {
                await RefreshCurrentOrder(args);
            });
            MessagingCenter.Subscribe<PaymentHistoryDetail, bool>(this, "PaymentVoid", async (sender, args) =>
            {
                IsPaymentVoidMade = true;
                await RefreshCurrentOrder(args);
            });
        }
        public async Task RefreshCurrentOrder(bool args)
        {
            try
            {
                if (args)
                {
                    theViewModel.IsPageLoading = true;

                    var orderNo = theViewModel.CurrentOrder.OrderNo.ToString();
                    var orderRefresh = await (BindingContext as NewQuickRentalMainPageViewModel).RefetchOrder(orderNo);
                    if (orderRefresh != null)
                    {
                        theViewModel.CurrentOrder = orderRefresh;

                        if (theViewModel.CurrentOrderUpdate == null) theViewModel.CurrentOrderUpdate = new OrderUpdate();

                        theViewModel.CurrentOrderUpdate.Order = theViewModel.CurrentOrder;
                    }

                    theViewModel.IsPageLoading = false;

                }
            }
            catch (Exception ex)
            {
                theViewModel.IsPageLoading = false;
            }

        }
        public async void UpdateTheOrder(Customer customer, Tuple<string, string> theNotes = null)
        {
            var orderRefresh = await (BindingContext as NewQuickRentalMainPageViewModel).UpdateCurrentOrder(customer, theNotes);
            AfterUpdate_OrderProcessing(orderRefresh);
        }

        public async Task<bool> AfterUpdate_OrderProcessing(OrderUpdate orderRefresh)
        {
            try
            {
                if (orderRefresh == null) return false;

                if (!string.IsNullOrEmpty(orderRefresh.NotAcceptableErrorMessage))
                {
                    await DisplayAlert("Invalid", orderRefresh.NotAcceptableErrorMessage, "Ok");

                    var res = await theViewModel.OrderLock(false);

                    NavigateToDashboard();

                    return false;
                }

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
                            bool custOk = await DisplayAlert("Customer Options", question.Answer, "Yes", "No");
                            orderRefresh.Answers.Find(qa => qa.Code == question.Code).Answer = custOk.ToString();

                            var ordUpdate = (BindingContext as NewQuickRentalMainPageViewModel).CurrentOrderUpdate;
                            if (ordUpdate == null)
                                ordUpdate = orderRefresh;
                            else
                            {
                                var ord = orderRefresh.Answers.Find(qa => qa.Code == question.Code);
                                ordUpdate.Answers.Add(ord);
                            }

                            ordUpdate.Order = theViewModel.CurrentOrder;
                            theViewModel.CurrentOrderUpdate = ordUpdate;
                            orderRefresh = await (BindingContext as NewQuickRentalMainPageViewModel).UpdateCurrentOrder(updateOrder: ordUpdate);
                            if (orderRefresh == null)
                            {
                                return true;
                            }
                        }

                    }
                }
                if (orderRefresh.Notifications.Count > 0)
                {
                    foreach (var notification in orderRefresh.Notifications)
                    {
                        await DisplayAlert("Customer Notification", notification, "OK");
                    }
                    return false;
                }
                if (orderRefresh.CustomerMessage != null && orderRefresh.CustomerMessage.Length > 0)
                {
                    await DisplayAlert("Customer Message", orderRefresh.CustomerMessage, "OK");
                    return false;
                }
                if (!string.IsNullOrEmpty(orderRefresh.NotAcceptableErrorMessage))
                {
                    await DisplayAlert("Invalid", orderRefresh.NotAcceptableErrorMessage, "Ok");

                    var res = await theViewModel.OrderLock(false);

                    NavigateToDashboard();

                    return false;
                }
                if (orderRefresh.Answers == null || orderRefresh.Answers.Count == 0)
                {
                    if (orderRefresh.Order != null)
                        return true;
                    else
                        return false;
                }
                else
                {
                    return await AfterUpdate_OrderProcessing(orderRefresh);
                }
            }
            catch (Exception)
            {
                return false;
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
            if (selectedItem == "Rentals")
            {
                AddDetailRentalView addDetailRentalView = new AddDetailRentalView();
                AddDetailRentalViewModel addDetailRentalViewModel = new AddDetailRentalViewModel(selectedItem, 1);
                addDetailRentalViewModel.CurrentOrder = ((NewQuickRentalMainPageViewModel)BindingContext).CurrentOrder;
                addDetailRentalViewModel.OrderSettings = ((NewQuickRentalMainPageViewModel)BindingContext).TheOrderSettings;
                addDetailRentalView.BindingContext = addDetailRentalViewModel;
                await Navigation.PushAsync(addDetailRentalView);
            }
            else if (selectedItem == "Merchandise")
            {
                AddDetailMerchView addDetailMerchView = new AddDetailMerchView();
                AddDetailMerchViewModel addDetailMerchViewModel = new AddDetailMerchViewModel();
                addDetailMerchViewModel.CurrentOrder = ((NewQuickRentalMainPageViewModel)BindingContext).CurrentOrder;
                addDetailMerchViewModel.OrderSettings = ((NewQuickRentalMainPageViewModel)BindingContext).TheOrderSettings;
                addDetailMerchView.BindingContext = addDetailMerchViewModel;
                await Navigation.PushAsync(addDetailMerchView);
            }
            else if (selectedItem == "Rental Saleable")
            {
                AddDetailRentalSalesView addDetailRentalSalesView = new AddDetailRentalSalesView();
                AddDetailRentalSalesViewModel addDetailRentalSalesViewModel = new AddDetailRentalSalesViewModel(selectedItem, 1);
                addDetailRentalSalesViewModel.CurrentOrder = ((NewQuickRentalMainPageViewModel)BindingContext).CurrentOrder;
                addDetailRentalSalesViewModel.OrderSettings = ((NewQuickRentalMainPageViewModel)BindingContext).TheOrderSettings;
                addDetailRentalSalesView.BindingContext = addDetailRentalSalesViewModel;
                await Navigation.PushAsync(addDetailRentalSalesView);
            }
            else if (selectedItem == "Rate Tables")
            {
                AddDetailRentalView addDetailRentalView = new AddDetailRentalView();
                AddDetailRentalViewModel addDetailRentalViewModel = new AddDetailRentalViewModel(selectedItem, 6);
                addDetailRentalViewModel.CurrentOrder = ((NewQuickRentalMainPageViewModel)BindingContext).CurrentOrder;
                addDetailRentalViewModel.OrderSettings = ((NewQuickRentalMainPageViewModel)BindingContext).TheOrderSettings;
                addDetailRentalView.BindingContext = addDetailRentalViewModel;
                await Navigation.PushAsync(addDetailRentalView);
            }
            else if (selectedItem == "Kits")
            {
                AddDetailKitsView addDetailKitsView = new AddDetailKitsView();
                AddDetailKitsViewModel addDetailKitsViewModel = new AddDetailKitsViewModel(selectedItem, 1);
                addDetailKitsViewModel.CurrentOrder = ((NewQuickRentalMainPageViewModel)BindingContext).CurrentOrder;
                addDetailKitsViewModel.OrderSettings = ((NewQuickRentalMainPageViewModel)BindingContext).TheOrderSettings;
                addDetailKitsView.BindingContext = addDetailKitsViewModel;
                await Navigation.PushAsync(addDetailKitsView);
            }

        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            var a = myPicker.SelectedItem;
            var b = myPicker.SelectedIndex;
            (BindingContext as NewQuickRentalMainPageViewModel).SelectedItem = "Rentals";
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
                if (theViewModel.CurrentOrder.OrderPaid > 0)
                {
                    await DisplayAlert("Alert!", "Payments are made against this order. Can not void.", "OK");
                }
                else
                {
                    bool doVoid = await DisplayAlert("Void", "Are you sure you want to void?", "OK", "Cancel");
                    if (doVoid)
                    {
                        string result = await ((NewQuickRentalMainPageViewModel)BindingContext).VoidOrder();
                        if (!result.HasData())
                        {
                            NavigateToDashboard();
                        }
                        else
                            await DisplayAlert("Void", "Void did not succeed try again", "OK");
                    }
                }

            }
            catch (Exception ex)
            {
                //TODO: log error
            }
        }
        public async void NavigateToDashboard()
        {
            UnSubscribeMessagingCenter();
            //TODO: Navigate to the Main Page
            (Application.Current.MainPage as FlyoutPage).IsGestureEnabled = true;
            (Application.Current.MainPage as MainMenuFlyout).IsQuickRentalScreenDisplaying = false;
            //FrontCounter
            ((Application.Current.MainPage as MainMenuFlyout).FlyoutPageDrawerObject.BindingContext as MainMenuFlyoutDrawerViewModel).ResetSelectedItem();

            var NavSer = DependencyService.Resolve<INavigationService>();
            await NavSer.PushPageFromMenu(typeof(FocalPtMbl.MainMenu.Views.MainPage), "Dashboard");
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
            if (!await CheckIsCashCustomer()) return;

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
            this.Navigation.PushAsync(new EditDetailOfSelectedItemView(a as OrderDtl, theViewModel.CurrentOrder));
        }

        private void Button_Clicked_4(object sender, EventArgs e)
        {
            var vm = (BindingContext as NewQuickRentalMainPageViewModel);
            this.Navigation.PushAsync(new TotalBreakoutView(vm.CurrentOrder));
        }

        private async void UpdateODateEDate()
        {
            theViewModel.IsPageLoading = true;
            var orderRefresh = await (BindingContext as NewQuickRentalMainPageViewModel).UpdateDateValues();
            await AfterUpdate_OrderProcessing(orderRefresh);
            theViewModel.IsPageLoading = false;
        }

        private async void LabelDropDownCustomControl_ItemSelected(object sender, CustomControls.ItemSelectedEventArgs e)
        {
            if (theViewModel.IsPageLoading) return;
            theViewModel.IsPageLoading = true;

            (BindingContext as NewQuickRentalMainPageViewModel).GetEndDateAndTimeValues();
            UpdateODateEDate();
        }

        private async void LabelDropDownCustomControl_ItemSelected_1(object sender, CustomControls.ItemSelectedEventArgs e)
        {
            if (theViewModel.IsPageLoading) return;

            UpdateODateEDate();
        }

        private async void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (theViewModel.IsPageLoading) return;

            UpdateODateEDate();
        }

        private async void TimePicker_Unfocused(object sender, FocusEventArgs e)
        {
            if (theViewModel.IsPageLoading) return;

            UpdateODateEDate();
        }
        private async Task<bool> CheckIsCashCustomer()
        {
            if (!theViewModel.IsSaveEnabled)
            {
                await DisplayAlert("Alert!", "Please select a valid customer.", "Ok");
                this.Navigation.PushAsync(new NewQuickRentalSelectCustomerPage());
                return false;
            }
            return true;
        }
        private async void SaveTapped(object sender, EventArgs e)
        {
            var isOk = await CheckIsCashCustomer();
            if (!isOk) return;

            var result = await SaveItNow();
            if (result)
            {
                NavigateToDashboard();
            }
        }

        public async Task<bool> SaveItNow(OrderSaveTypes saveType = OrderUpdate.OrderSaveTypes.ExitOnly)
        {
            var vm = (BindingContext as NewQuickRentalMainPageViewModel);
            if (vm.CurrentOrderUpdate == null)
            {
                vm.CurrentOrderUpdate = new OrderUpdate();
            }
            vm.CurrentOrderUpdate.Order = vm.CurrentOrder;
            vm.CurrentOrderUpdate.Save = saveType;

            bool isSaveSuccessfull = false;
            var orderRefresh = await vm.UpdateOrder(vm.CurrentOrderUpdate);
            if (orderRefresh == null)
            {
                await DisplayAlert("Success", "Record Saved.", "Ok");
                isSaveSuccessfull = true;
            }
            else
            {
                var isSuccess = await AfterUpdate_OrderProcessing(orderRefresh);
                if (isSuccess)
                {
                    await DisplayAlert("Success", "Record Saved.", "Ok");
                    isSaveSuccessfull = true;
                }
                else
                {
                    await DisplayAlert("Save Failed", "Could Not Save.", "Ok");
                    isSaveSuccessfull = false;
                }
            }

            return isSaveSuccessfull;
        }

        public async Task<bool> EmailItNow()
        {
            string customersEmail = theViewModel.CurrentOrderUpdate.Order.Customer.CustomerEmail;
            bool isEmailSuccess = false;
            if (!customersEmail.HasData())
            {
                bool confirmation = await DisplayAlert("No Email Found", "No Email on file for customer. Would you like to send to a new address? ", "Yes", "No");
                if (confirmation)
                {
                    customersEmail = await DisplayPromptAsync("Customer's Email", "What's the customers email address", keyboard: Keyboard.Email);
                    if (IsValidEmail(customersEmail))
                    {
                        theViewModel.CurrentOrderUpdate.Order.Customer.CustomerEmail = customersEmail;

                        var emailUpdate = await theViewModel.UpdateOrder(theViewModel.CurrentOrderUpdate);

                        if (emailUpdate == null)
                        {
                            isEmailSuccess = true;
                        }
                        else
                        {
                            isEmailSuccess = await AfterUpdate_OrderProcessing(emailUpdate);
                        }
                    }
                    else
                    {
                        await DisplayAlert("Email Incorrect", "The entered Email is invalid.", "Ok");
                        isEmailSuccess = false;
                    }
                }
            }
            else
            {
                isEmailSuccess = true;
            }

            return isEmailSuccess;
        }

        private async void SaveAndEmailTapped(object sender, EventArgs e)
        {
            var isOk = await CheckIsCashCustomer();
            if (!isOk) return;

            var isSaveSuccessfull = await SaveItNow();
            if (!isSaveSuccessfull)
                return;

            if (await EmailItNow())
            {
                if (!string.IsNullOrEmpty(theViewModel.CurrentOrderUpdate.Order.Customer.CustomerEmail))
                {
                    ////SEND EMAIL
                    IGeneralComponent generalComponent = new GeneralComponent();
                    EmailDocumentInputDTO emailDocumentInputDTO = new EmailDocumentInputDTO();
                    emailDocumentInputDTO.DocKind = (int)DocKinds.Order;
                    emailDocumentInputDTO.RecordID = theViewModel.CurrentOrder.OrderNo;
                    emailDocumentInputDTO.ToAddr = theViewModel.CurrentOrderUpdate.Order.Customer.CustomerEmail;
                    bool response = await generalComponent.SendEmailDocument(emailDocumentInputDTO);
                    if (response)
                    {
                        await DisplayAlert("Success", "Saved and Document sent successfully", "OK");
                        NavigateToDashboard();
                    }
                    else
                    {
                        await DisplayAlert("FocalPoint Mobile", "Failed to send an email", "OK");
                    }
                }
            }

        }

        private async void SaveAsQuoteTapped(object sender, EventArgs e)
        {
            var isOk = await CheckIsCashCustomer();
            if (!isOk) return;

            var result = await SaveItNow(OrderUpdate.OrderSaveTypes.ExitAsQuote);

            if (result)
            {
                NavigateToDashboard();
            }
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

    }
}