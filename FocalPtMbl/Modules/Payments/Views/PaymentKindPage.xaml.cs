using System;
using System.Linq;
using System.Threading.Tasks;
using FocalPoint.Modules.Payments.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Payments.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentKindPage : ContentPage
    {
        PaymentPageViewModel viewModel;
        public PaymentKindPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel = (PaymentPageViewModel)BindingContext;
            if (!string.IsNullOrEmpty(viewModel.CreditCardDetails.ManualToken))
            {
                _ = ProcessPayment();
                return;
            }
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void ProcessPayment_Clicked(object sender, EventArgs e)
        {
            var validationMsg = viewModel.ValidatePaymentKinds();
            if (!string.IsNullOrEmpty(validationMsg))
            {
                _ = DisplayAlert("FocalPoint", validationMsg, "Ok");
                return;
            }
            try
            {
                if (viewModel.SelectedPaymentType.PaymentKind == "CC" && viewModel.CreditCardDetails.ProcessOnline)
                {
                    PaymentManual manualPayment = new PaymentManual
                    {
                        BindingContext = viewModel
                    };
                    await Navigation.PushAsync(manualPayment);
                    return;
                }
                await ProcessPayment();
            }
            catch (Exception ex)
            {
                _ = DisplayAlert("FocalPoint", ex.Message, "Ok");
            }
        }

        private async Task ProcessPayment()
        {
            try
            {
                var response = await viewModel.ProcessPayment();
                if (response == null)
                {
                    viewModel.CreditCardDetails.ManualToken = null;
                    _ = DisplayAlert("Error", "Payment Response is null.", "Ok");
                }
                else if (response?.Notifications != null && response.Notifications.Any())
                {
                    viewModel.CreditCardDetails.ManualToken = null;
                    _ = DisplayAlert("FocalPoint", response.Notifications.First(), "Ok");
                }
                else if (response?.Payment != null)
                {
                    var due = decimal.TryParse(viewModel.ChangeDue.Trim('$'), out decimal dueAmt) ? dueAmt : 0;
                    var msg = due > 0 ? "Payment Complete, Change Due: " + Convert.ToDecimal(viewModel.ChangeDue).ToString("C") + "" : "Payment Complete";
                    await DisplayAlert("FocalPoint", msg, "Ok", " ");
                    if (response.GetSignature)
                    {
                        
                    }
                    viewModel.ResetCards();
                    _ = Navigation.PopAsync();
                    _ = Navigation.PopAsync();
                    _ = Navigation.PopAsync();
                    //todo: check navigation method
                }
                else
                {
                    viewModel.CreditCardDetails.ManualToken = null;
                    _ = DisplayAlert("FocalPoint", "Something went wrong.", "Ok");
                }
            }
            catch(Exception ex)
            {
                _ = DisplayAlert("FocalPoint", ex.Message, "Ok");
            }
        }
    }
}