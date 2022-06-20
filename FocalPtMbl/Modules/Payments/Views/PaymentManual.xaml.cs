using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FocalPoint.Modules.Payments.ViewModels;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Payments.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentManual : ContentPage
    {
        PaymentPageViewModel viewModel;
        PaymentRequest.RequestTypes requestType;

        public PaymentManual(PaymentRequest.RequestTypes requestType)
        {
            InitializeComponent();
            this.requestType = requestType;
            hybridWebView.RegisterAction(data => GetResultFromJavaScript(data));
            //hybridWebView.Source = "https://visumbanyan.fpsdns.com:8080/cardconnect/Xamarin.html";
            hybridWebView.Navigated += CardConnectWebView_Navigated;
        }

        private void GetResultFromJavaScript(string data)
        {
            viewModel.CreditCardDetails.ManualToken = string.IsNullOrEmpty(data) ? null : data;
            EditorTest.Focus();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private void CardConnectWebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            var firstName = viewModel.CreditCardDetails.CardHolderName?.Value.Split(' ')[0];
            var lastName = viewModel.CreditCardDetails.CardHolderName?.Value.Split(' ').Count() > 1 ? viewModel.CreditCardDetails.CardHolderName?.Value.Split(' ')[1] : "";
            hybridWebView.EvaluateJavaScriptAsync("document.getElementById('firstname').setAttribute('value', '" + firstName + "')");
            hybridWebView.EvaluateJavaScriptAsync("document.getElementById('lastname').setAttribute('value', '" + lastName + "')");
            hybridWebView.EvaluateJavaScriptAsync("document.getElementById('address').setAttribute('value', '" + viewModel.CreditCardDetails.AvsStreetAddress.Value + "')");
            hybridWebView.EvaluateJavaScriptAsync("document.getElementById('zip').setAttribute('value', '" + viewModel.CreditCardDetails.AvsZipCode.Value + "')");
            hybridWebView.EvaluateJavaScriptAsync("document.getElementById('tokenframe').setAttribute('src', '" + viewModel.Settings.POSPublicKey + "')");
            if (requestType == PaymentRequest.RequestTypes.PreAuthDeposit)
            {
                var htmlStr = "PreAuth: " + viewModel.Payment.Value;
                hybridWebView.EvaluateJavaScriptAsync("document.getElementById('lblTrans').innerHTML = '" + htmlStr + "'");
            }
            else
            {
                //hybridWebView.EvaluateJavaScriptAsync("document.getElementById('lblTrans').innerHTML = '" + htmlStr + "'");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel = (PaymentPageViewModel)BindingContext;
            hybridWebView.Source = "https://visumbanyan.fpsdns.com" + viewModel.Settings.POSManualUrl;
        }

        private void EditorTest_Focused(object sender, FocusEventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}