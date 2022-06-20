using FocalPoint.Modules.Payments.ViewModels;
using System;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Visum.Services.Mobile.Entities.PaymentRequest;

namespace FocalPoint.Modules.Payments.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentView : ContentPage
    {
        public PaymentView()
        {
            InitializeComponent();
            BindingContext = new PaymentPageViewModel();
            //((PaymentPageViewModel)BindingContext).GetOrderDetails(501842);
            //order.GetOrderDetails(501842).ContinueWith(task =>
            //{
            //    ((PaymentPageViewModel)BindingContext).Order = task.Result;
            //});
        }
        public PaymentView(Order currentOrder)
        {
            InitializeComponent();
            BindingContext = new PaymentPageViewModel(currentOrder);
        }

        private async void Payment_Tapped(object sender, EventArgs e)
        {
            ((PaymentPageViewModel)BindingContext).PaymentMethod = RequestTypes.Standard.ToString();
            await NavigateToPaymentMethods(sender, RequestTypes.Standard);
        }
        private async void Deposit_Tapped(object sender, EventArgs e)
        {
            ((PaymentPageViewModel)BindingContext).PaymentMethod = RequestTypes.StandardDepost.ToString();
            await NavigateToPaymentMethods(sender, RequestTypes.StandardDepost);
        }
        private async void SecurityDeposit_Tapped(object sender, EventArgs e)
        {
            ((PaymentPageViewModel)BindingContext).PaymentMethod = RequestTypes.SecurityDeposit.ToString();
            await NavigateToPaymentMethods(sender, RequestTypes.SecurityDeposit);
        }
        private async void Security_PreAuth_Tapped(object sender, EventArgs e)
        {
            ((PaymentPageViewModel)BindingContext).PaymentMethod = RequestTypes.PreAuthDeposit.ToString();
            await NavigateToPaymentMethods(sender, RequestTypes.PreAuthDeposit);
        }
        private async Task NavigateToPaymentMethods(object sender, RequestTypes request)
        {
            ((PaymentPageViewModel)BindingContext).Indicator = true;
            ((Frame)sender).IsEnabled = false;
            await ((PaymentPageViewModel)BindingContext).SetPaymenyTypes(request);
            PaymentMethodsPage paymentMethodsPage = new PaymentMethodsPage
            {
                BindingContext = (PaymentPageViewModel)BindingContext,
            };
            await Navigation.PushAsync(paymentMethodsPage);
            ((Frame)sender).IsEnabled = true;
            ((PaymentPageViewModel)BindingContext).Indicator = false;
        }

    }
}