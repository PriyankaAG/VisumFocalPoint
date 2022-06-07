using DevExpress.XamarinForms.CollectionView;
using DevExpress.XamarinForms.Editors;
using Visum.Services.Mobile.Entities;
using FocalPoint.Modules.FrontCounter.ViewModels;
using FocalPoint.Modules.FrontCounter.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FocalPoint.Modules.Payments.Views;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuickOrderCreationOutlineView : ContentPage
    {
        private bool inNavigation;
        private string selectedItem = "";
        private Order CurrentOrderCreated;
        //private 
        public CustomerLupView ClvPage;
        public QuickOrderInternalNotesView QOInternalNV;
        public QuickOrderInternalNotesView QOPrintNV;
        public QuickOrderTotalBreakoutView QOTotalBV;
        public QuickOrderDetailsView QODetailsV;
        public SelectCustomerView SelectCustV;
        public QuickOrderHeaderView QOHeadderV;
        public QuickOrderDetailsMerchView QODetailsMerchV;
        public Customer selectedCustomer;
        public QuickOrderPaymentsView QOPaymentSelectV;
        public QuickOrderCreationOutlineView()
        {
            
            selectedCustomer = new Customer();
           // SelectedCustomerNameBox = "Select Customer";
            DevExpress.XamarinForms.CollectionView.Initializer.Init();
            InitializeComponent();
            BindingContext = new QuickOrderCreationOutlineViewModel();
            //((QuickOrderCreationOutlineViewModel)this.BindingContext).SelectedTypes = new System.Collections.ObjectModel.ObservableCollection<string>{ "Rentals", "Merchandise", "Rate Table", "Kits" } ;
            ClvPage = new CustomerLupView();
           //ClvPage.EventPassNewCust += SelectNewCustView_EventPassNewCust;
            //SelectCustV = new SelectCustomerView();
            //SelectCustV.EventPass += SelectCustomerView_EventPass;
            QOHeadderV = new QuickOrderHeaderView();
            QOInternalNV = new QuickOrderInternalNotesView();
            QOPrintNV = new QuickOrderInternalNotesView();
            QOTotalBV = new QuickOrderTotalBreakoutView();
            
            //QODetailsV = new QuickOrderDetailsView();
            //QODetailsMerchV = new QuickOrderDetailsMerchView();

            MessagingCenter.Subscribe<QuickOrderCreationOutlineView, string>(this, "Hi", async (sender, arg) =>
            {
                selectedCustomer = new Customer();
                await DisplayAlert("Message received", "arg=" + arg, "OK");
            });
            MessagingCenter.Subscribe<QuickOrderCreationOutlineView, Customer>(this, "Hi", async (sender, arg) =>
            {
                selectedCustomer = arg;
                string displayCustType = "";
                if (arg.CustomerType == "c")
                    displayCustType = "Credit";
                if (arg.CustomerType == "C")
                    displayCustType = "Credit";
                ((QuickOrderCreationOutlineViewModel)this.BindingContext).SelectedCustomerNameBox = selectedCustomer.CustomerName + " " + Regex.Replace(selectedCustomer.CustomerPhone, @"(\d{3})(\d{3})(\d{4})", "($1)$2-$3") + Environment.NewLine + " " + selectedCustomer.CustomerCity + ", " + selectedCustomer.CustomerState + " " + selectedCustomer.CustomerZip + " " + "Type: " + displayCustType;
                await DisplayAlert("Message received", "arg=" + arg.CustomerName, "OK");
            });
            GetOrderInfo();
            if(((QuickOrderCreationOutlineViewModel)this.BindingContext).CurrentOrder != null)
            {
                //((QuickOrderCreationOutlineViewModel)this.BindingContext).SelectedHeaderBox = ((QuickOrderCreationOutlineViewModel)this.BindingContext).CurrentOrder.OrderDDte.Date.ToString() + ((QuickOrderCreationOutlineViewModel)this.BindingContext).CurrentOrder.OrderODte.Date.ToString();
            }
        }

        private async void SelectCustomerView_EventPass(Customer cust)
        {
            //Get customerType Display from value
            string displayCustType="";
            if (cust.CustomerType =="c")
             displayCustType = "Cash";
            if (cust.CustomerType == "c")
                displayCustType = "Cash";
            OrderUpdate checkUpdate = ((QuickOrderCreationOutlineViewModel)this.BindingContext).UpdateCust(cust);
            if (checkUpdate.Answers != null && checkUpdate.Answers.Count > 0)
            {

            }
            if (checkUpdate.Notifications.Count > 0)
            {
                foreach (var notification in checkUpdate.Notifications)
                {
                    await DisplayAlert("Customer Notification", notification, "OK");
                }
            }
            if (checkUpdate.CustomerMessage.Length > 0)
            {
                await DisplayAlert("Customer Message", checkUpdate.CustomerMessage, "OK");
            }
            ((QuickOrderCreationOutlineViewModel)this.BindingContext).SelectedCustomerNameBox = cust.CustomerName + " " + Regex.Replace(cust.CustomerPhone, @"(\d{3})(\d{3})(\d{4})", "($1)$2-$3") + Environment.NewLine + cust.CustomerCity + ", " + cust.CustomerState + " " + cust.CustomerZip + " " + "Type: " + displayCustType + " "; // String.Format("{0:(###) ###-####}", cust.CustomerPhone); 
                                                                                                                                                                                                                                                                                                                                            //  SelectedCustomerNameBox = cust.CustomerName;
                                                                                                                                                                                                                                                                                                                                            //throw new NotImplementedException();

        }
        private async void SelectNewCustView_EventPassNewCust(Customer cust)
        {
            //Get customerType Display from value
            string displayCustType = "";
            if (cust.CustomerType == "c")
                displayCustType = "Cash";
            if (cust.CustomerType == "c")
                displayCustType = "Cash";

           OrderUpdate checkUpdate =  ((QuickOrderCreationOutlineViewModel)this.BindingContext).UpdateCust(cust);
            if(checkUpdate.Answers != null && checkUpdate.Answers.Count >0)
            {
                while (checkUpdate.Answers != null || checkUpdate.Answers.Count>0)
                {
                    //display answer if key does not have a value update that value
                    //checkUpdate.Answers.
                    checkUpdate = ((QuickOrderCreationOutlineViewModel)this.BindingContext).UpdateCust(cust);
                }
            }
            if(checkUpdate.Notifications.Count >0)
            {
                foreach(var notification in checkUpdate.Notifications)
                {
                    await DisplayAlert("Customer Notification", notification, "OK");
                }
            }
            if(checkUpdate.CustomerMessage.Length>0)
            {
                await DisplayAlert("Customer Message", checkUpdate.CustomerMessage, "OK");
            }
            ((QuickOrderCreationOutlineViewModel)this.BindingContext).SelectedCustomerNameBox = cust.CustomerName + " " + Regex.Replace(cust.CustomerPhone, @"(\d{3})(\d{3})(\d{4})", "($1)$2-$3") + Environment.NewLine  + cust.CustomerCity + ", " + cust.CustomerState + " " + cust.CustomerZip + " " + "Type: " + displayCustType + " "; // String.Format("{0:(###) ###-####}", cust.CustomerPhone); 
                                                                                                                                                                                                                                                                                                                                            //  SelectedCustomerNameBox = cust.CustomerName;
                                                                                                                                                                                                                                                                                                                                            //throw new NotImplementedException();
        }
        public async void ItemSelected(object sender, CollectionViewGestureEventArgs args)
        {
            if (args.Item != null)
                await OpenDetailPage(GetDetailInfo(args.Item));
        }

        Task OpenDetailPage(OrderDtl dtl)
        {
            if (dtl == null)
                return Task.CompletedTask;

            if (this.inNavigation)
                return Task.CompletedTask;

            this.inNavigation = true;
            //EventPassNewCust(((CustomerLupViewModel)this.BindingContext).NewCustomer);
            //MessagingCenter.Send<CustomerLupView, Customer>(this, "Hi", dtl);
            //Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 1]);

            //MessagingCenter.Send<CustomerLupView, Customer>(this, "Hi", dtl);

            //return Navigation.PopModalAsync();
            if(dtl.OrderDtlType == "m")
            {
                return Navigation.PushAsync(new SelectSerialsView(dtl));
            }
            return Navigation.PushAsync(new SelectSerialsView(dtl));
        }

        private OrderDtl GetDetailInfo(object selectedDtl)
        {
            if (selectedDtl is OrderDtl dtl)
                return dtl;
            return new OrderDtl();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.inNavigation = false;

            //check if update is needed
            //((QuickOrderCreationOutlineViewModel)this.BindingContext).CurrentOrder.OrderAmount.
            ((QuickOrderCreationOutlineViewModel)this.BindingContext).CurrentTotal = "Total:" + ((QuickOrderCreationOutlineViewModel)this.BindingContext).CurrentOrder.OrderAmount.ToString("c") + "     Balance Due: " + ((QuickOrderCreationOutlineViewModel)this.BindingContext).CurrentOrder.Totals.TotalDueAmt.ToString("c");

            //((QuickOrderCreationOutlineViewModel)this.BindingContext).
            // ((QuickOrderCreationOutlineViewModel)this.BindingContext).GetCustomersInfo();
            //((CustomerSimpleViewModel)this.BindingContext).GetCustomerInfo();

        }
        protected override void OnDisappearing()
        {
            //SelectCustV.EventPass -= SelectCustomerView_EventPass;
            //ClvPage.EventPassNewCust -= SelectNewCustView_EventPassNewCust;
            base.OnDisappearing();
        }
        //throw events when needed
        //Check Updated Details
        //if (QODetailsV.HasNewItemsToAdd)
        //{
        //    foreach(var item in QODetailsV.OrderItems)
        //       ((QuickOrderCreationOutlineViewModel) BindingContext).CurrentOrder.OrderDtls.Append(item);
        // }
        private async void SimpleButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        private async void AddNew_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(ClvPage);
        }
        private async void SelectCust_Clicked(object sender, EventArgs e)
        {
            
            await Navigation.PushAsync(new SelectCustomerView(((QuickOrderCreationOutlineViewModel)this.BindingContext).CurrentOrder));
        }
        private async void SelectHeader_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(QOHeadderV);
        }
        private async void InternalNotes_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(QOInternalNV);
        }
        private async void PrintNotes_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(QOPrintNV);
        }
        private async void TotalBreakout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(QOTotalBV);
        }
        private async void Payment_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new QuickOrderPaymentsView(((QuickOrderCreationOutlineViewModel)this.BindingContext).CurrentOrder));
            IViewOrderEntityComponent order = new ViewOrderEntityComponent();
            var orderDetails = await order.GetOrderDetails(501842);
            await Navigation.PushAsync(new PaymentView(orderDetails));
            //await Navigation.PushAsync(new PaymentView(((QuickOrderCreationOutlineViewModel)this.BindingContext).CurrentOrder));
        }
        private List<string> notifications = new List<string>();
        private async void GetOrderInfo()
        {
            notifications = ((QuickOrderCreationOutlineViewModel)BindingContext).CreateNewOrder();
            if (notifications != null)
            {
                if (notifications.Count > 0)
                    foreach (var notification in notifications)
                        await DisplayAlert("Notification", notification, "ok");
            }
            else
            {
                await DisplayAlert("Problem Creating an order", "There was a problem creating the order. Please try again with a better connection", "OK");
            }
        }
        private async void AddDetails_Clicked(object sender, EventArgs e)
        {
            //if (QOHeadderV.StartDate != null && QOHeadderV.EndDate != null)
            //{
                if (selectedItem == "Rentals")
                    await Navigation.PushAsync(new QuickOrderDetailsView(((QuickOrderCreationOutlineViewModel)BindingContext).CurrentOrder));
                if (selectedItem == "Merchandise")
                    await Navigation.PushAsync(new QuickOrderDetailsMerchView(((QuickOrderCreationOutlineViewModel)BindingContext).CurrentOrder));
                if (selectedItem == "Rate Table")
                    await Navigation.PushAsync(new QuickOrderDetailsView(((QuickOrderCreationOutlineViewModel)BindingContext).CurrentOrder));
                if (selectedItem == "Rental Salable")
                    await Navigation.PushAsync(new QuickOrderDetailsView(((QuickOrderCreationOutlineViewModel)BindingContext).CurrentOrder));
                if(selectedItem == "")
                {
                    await DisplayAlert("Select Type", "Please select a search type", "OK");
                }
           // }
        }

        private void ComboBoxEdit_SelectionChanged(object sender, EventArgs e)
        {
            var combo = sender as ComboBoxEdit;
            selectedItem = (string)combo.SelectedItem;
        }

        private async void SimpleButton_Clicked_1(object sender, EventArgs e)
        {
            bool doVoid = await DisplayAlert("Void", "Are you sure you want to void?", "OK", "Cancel");
            if (doVoid)
            {
               if( ((QuickOrderCreationOutlineViewModel)BindingContext).VoidOrder())
                 await Navigation.PopAsync();
               else
                   await DisplayAlert("Void", "Void did not succeed try again", "OK");
            }
        }
    }
}