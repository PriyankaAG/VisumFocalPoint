using FocalPoint.Modules.Payments.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        PaymentPageViewModel viewModel;
        //public PaymentView()
        //{
        //    InitializeComponent();
        //    //BindingContext = new PaymentViewModel();
        //    BindingContext = viewModel = new PaymentPageViewModel();
        //}

        public PaymentView(Order currentOrder)
        {
            InitializeComponent();
            BindingContext = viewModel = new PaymentPageViewModel(currentOrder);
        }

        private async void Payment_Tapped(object sender, EventArgs e)
        {
            await viewModel.SetPaymenyTypes(RequestTypes.Standard);
            PaymentMethodsPage paymentMethodsPage = new PaymentMethodsPage
            {
                BindingContext = viewModel,
            };
            await Navigation.PushAsync(paymentMethodsPage);

        }

        private async void Deposit_Tapped(object sender, EventArgs e)
        {
            await viewModel.SetPaymenyTypes(RequestTypes.StandardDepost);
            PaymentMethodsPage paymentMethodsPage = new PaymentMethodsPage
            {
                BindingContext = viewModel,
            };
            await Navigation.PushAsync(paymentMethodsPage);
        }

        private async void SecurityDeposit_Tapped(object sender, EventArgs e)
        {
            await viewModel.SetPaymenyTypes(RequestTypes.SecurityDeposit);
            PaymentMethodsPage paymentMethodsPage = new PaymentMethodsPage
            {
                BindingContext = viewModel,
            };
            await Navigation.PushAsync(paymentMethodsPage);
        }

        private async void Security_PreAuth_Tapped(object sender, EventArgs e)
        {
            await viewModel.SetPaymenyTypes(RequestTypes.PreAuthDeposit);
            PaymentMethodsPage paymentMethodsPage = new PaymentMethodsPage
            {
                BindingContext = viewModel,
            };
            await Navigation.PushAsync(paymentMethodsPage);
        }
    }
}