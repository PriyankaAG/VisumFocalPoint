using DevExpress.XamarinForms.CollectionView;
using DevExpress.XamarinForms.Editors;
using Visum.Services.Mobile.Entities;
using FocalPoint.Modules.FrontCounter.ViewModels;
using FocalPoint.Modules.FrontCounter.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectCustomerView : ContentPage
    {
        private bool inNavigation;
        public CustomerLupView CustomerPage;
        //public delegate void EventHandler(Customer cust);
        //public event EventHandler EventPass;
        public SelectCustomerView(Order curOrder)
        {
            InitializeComponent();
            BindingContext = new SelectCustomerViewModel(curOrder);
            CustomerPage = new CustomerLupView();
            this.Title = "Customer Select";
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.inNavigation = false;
            ((SelectCustomerViewModel)this.BindingContext).GetCustomersInfo();
            //((CustomerSimpleViewModel)this.BindingContext).GetCustomerInfo();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // EventPass("Back Code");
        }
        public async void ItemSelected(object sender, CollectionViewGestureEventArgs args)
        {
            if (args.Item != null)
            {
                ((SelectCustomerViewModel)this.BindingContext).SelectedCustomer = (Customer)args.Item;
                OrderUpdate orderRefresh = (((SelectCustomerViewModel)this.BindingContext).UpdateCust(((SelectCustomerViewModel)this.BindingContext).SelectedCustomer));

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
                            ((SelectCustomerViewModel)this.BindingContext).orderUpdateRefresh = orderRefresh;
                            orderRefresh = ((SelectCustomerViewModel)this.BindingContext).UpdateCust(((SelectCustomerViewModel)this.BindingContext).SelectedCustomer);
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
                await OpenDetailPage((orderRefresh));
            }
        }
        Task OpenDetailPage(OrderUpdate orderRefresh)
        {
            if (orderRefresh == null)
                return Task.CompletedTask;

            if (this.inNavigation)
                return Task.CompletedTask;

            this.inNavigation = true;

            MessagingCenter.Send<SelectCustomerView, string>(this, "Hi", "John");
            MessagingCenter.Send<SelectCustomerView, OrderUpdate>(this, "Hi", orderRefresh);

            return Navigation.PopAsync();
        }
        private void TextEdit_Completed(object sender, EventArgs e)
        {
            ((SelectCustomerViewModel)this.BindingContext).GetSearchedCustomersInfo((sender as TextEdit).Text);
        }
        private void TextEdit_Cleared(object sender, EventArgs e)
        {
            ((SelectCustomerViewModel)this.BindingContext).GetSearchedCustomersInfo("");
        }

        private async void SimpleButton_Clicked(object sender, EventArgs e)
        {
            // dataForm.Commit();
            // if (dataForm.Validate())
            await Navigation.PopAsync();// DisplayAlert("Success", "Your delivery information has been successfully saved", "OK");
        }
        private async void AddNew_Clicked(object sender, EventArgs e)
        {
            // dataForm.Commit();
            // if (dataForm.Validate())
            await Navigation.PushAsync(new CustomerLupView());// DisplayAlert("Success", "Your delivery information has been successfully saved", "OK");

        }

        private void collectionView_SelectionChanged(object sender, CollectionViewSelectionChangedEventArgs e)
        {
            ((SelectCustomerViewModel)this.BindingContext).SelectedCustomer = (Customer)e.SelectedItems[0];
        }
    }
}