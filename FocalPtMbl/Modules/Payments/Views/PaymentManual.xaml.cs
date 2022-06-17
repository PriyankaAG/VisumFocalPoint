using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FocalPoint.Modules.Payments.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Payments.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentManual : ContentPage
    {
        PaymentPageViewModel viewModel;

        public PaymentManual()
        {
            InitializeComponent();
            hybridWebView.RegisterAction(data => GetResultFromJavaScript(data));
            //hybridWebView.Source = "https://visumbanyan.fpsdns.com:8080/cardconnect/Xamarin.html";
            hybridWebView.Navigated += CardConnectWebView_Navigated;
        }

        private  void GetResultFromJavaScript(string data)
        {
            viewModel.CreditCardDetails.ManualToken = string.IsNullOrEmpty(data) ? null : data;
            EditorTest.Focus();
            //await Navigation.PopAsync();
            //var response = await viewModel.ProcessPayment();
            //if (response == null)
            //{
            //    _ = DisplayAlert("Error", "Payment Response is null.", "Ok");
            //}
            //else if (response?.Notifications != null && response.Notifications.Any())
            //{
            //    _ = DisplayAlert("FocalPoint", response.Notifications.First(), "Ok");
            //}
            //else if (response?.Payment != null)
            //{
            //    var due = decimal.TryParse(viewModel.ChangeDue.Trim('$'), out decimal dueAmt) ? dueAmt : 0;
            //    var msg = due > 0 ? "Payment Complete, Change Due: " + Convert.ToDecimal(viewModel.ChangeDue).ToString("C") + "" : "Payment Complete";
            //    await DisplayAlert("FocalPoint", msg, "Ok", " ");
            //    _ = Navigation.PopAsync();
            //    _ = Navigation.PopAsync();
            //    _ = Navigation.PopAsync();
            //    _ = Navigation.PopAsync();
            //    //todo: check navigation method
            //}
            //else
            //{
            //    _ = DisplayAlert("FocalPoint", "Something went wrong.", "Ok");
            //}
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
            hybridWebView.EvaluateJavaScriptAsync("document.getElementById('address').setAttribute('value', '" + viewModel.CreditCardDetails.AvsStreetAddress + "')");
            hybridWebView.EvaluateJavaScriptAsync("document.getElementById('zip').setAttribute('value', '" + viewModel.CreditCardDetails.AvsZipCode + "')");
            hybridWebView.EvaluateJavaScriptAsync("document.getElementById('tokenframe').setAttribute('src', '" + viewModel.Settings.POSPublicKey + "')");
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