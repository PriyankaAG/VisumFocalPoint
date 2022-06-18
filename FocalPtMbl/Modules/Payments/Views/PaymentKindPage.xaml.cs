using System;
using System.Linq;
using System.Threading.Tasks;
using FocalPoint.Modules.FrontCounter.Views;
using FocalPoint.Modules.Payments.ViewModels;
using FocalPoint.Modules.ViewModels;
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
            MessagingCenter.Unsubscribe<SignatureTermsViewModel, bool>(this, "TermsDeclined");
            MessagingCenter.Unsubscribe<SignatureTermsViewModel, bool>(this, "TermsAccepted");
            MessagingCenter.Unsubscribe<SignatureViewModel, string>(this, "WaiverSignature");
            MessagingCenter.Unsubscribe<SignatureViewModel, string>(this, "Signature");

            MessagingCenter.Subscribe<SignatureTermsViewModel, bool>(this, "TermsDeclined", async (sender, args) =>
            {
                SignatureTermsView signatureTermsView = (SignatureTermsView)Navigation.NavigationStack.FirstOrDefault(p => p is SignatureTermsView);
                SignatureView orderSignatureView = (SignatureView)Navigation.NavigationStack.FirstOrDefault(p => p is SignatureView);
                if (signatureTermsView != null || orderSignatureView != null)
                    await Navigation.PopAsync();
                if (args)
                {
                    await DisplayAlert("FocalPoint Mobile", "Damage Waiver Rejected", "OK");
                }
                else
                {
                    await DisplayAlert("FocalPoint Mobile", "Terms Rejected", "OK");
                }

            });

            MessagingCenter.Subscribe<SignatureTermsViewModel, bool>(this, "TermsAccepted", async (sender, args) =>
            {
                SignatureView orderSignatureView = (SignatureView)Navigation.NavigationStack.FirstOrDefault(p => p is SignatureView);
                SignatureTermsView signatureTermsView = (SignatureTermsView)Navigation.NavigationStack.FirstOrDefault(p => p is SignatureTermsView);
                if (orderSignatureView != null || signatureTermsView != null)
                    await Navigation.PopAsync();
                PaymentPageViewModel paymentPageViewModel = (PaymentPageViewModel)BindingContext;
                paymentPageViewModel.OpenSignaturePage(Navigation, args);
            });

            MessagingCenter.Subscribe<SignatureViewModel, string>(this, "WaiverSignature", async (sender, capturedImage) =>
            {
                SignatureView orderSignatureView = (SignatureView)Navigation.NavigationStack.FirstOrDefault(p => p is SignatureView);
                SignatureTermsView signatureTermsView = (SignatureTermsView)Navigation.NavigationStack.FirstOrDefault(p => p is SignatureTermsView);
                if (orderSignatureView != null || signatureTermsView != null)
                    await Navigation.PopAsync();
                PaymentPageViewModel paymentPageViewModel = (PaymentPageViewModel)BindingContext;
                paymentPageViewModel.WaiverCapturedImage = capturedImage;
                paymentPageViewModel.IsNeedToRedirectTermsOrSignature(Navigation);
            });

            MessagingCenter.Subscribe<SignatureViewModel, string>(this, "Signature", async (sender, capturedImage) =>
            {
                SignatureTermsView signatureTermsView = (SignatureTermsView)Navigation.NavigationStack.FirstOrDefault(p => p is SignatureTermsView);
                SignatureView orderSignatureView = (SignatureView)Navigation.NavigationStack.FirstOrDefault(p => p is SignatureView);
                if (signatureTermsView != null || orderSignatureView != null)
                    await Navigation.PopAsync();
                PaymentPageViewModel paymentPageViewModel = (PaymentPageViewModel)BindingContext;
                paymentPageViewModel.SignatureImage = capturedImage;
                bool success = await paymentPageViewModel.SaveSignature();
                if (success)
                {
                    await DisplayAlert("FocalPoint Mobile", "Signature added successfully", "OK");
                }
                else
                {
                    await DisplayAlert("FocalPoint Mobile", "Failed to add Signature", "OK");
                }
            });

            //todo: need to find better way(Msg center)
            if (!string.IsNullOrEmpty(viewModel.CreditCardDetails?.ManualToken))
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
            viewModel.Indicator = true;
            try
            {
                var response = await viewModel.ProcessPayment();
                if (viewModel?.CreditCardDetails != null) viewModel.CreditCardDetails.ManualToken = null;
                if (response == null)
                {
                    _ = DisplayAlert("Error", "Something went wrong. Please try again.", "Ok");
                }
                else if (response?.Notifications != null && response.Notifications.Any())
                {
                    _ = DisplayAlert("FocalPoint", string.Join(Environment.NewLine, response.Notifications) , "Ok");
                }
                else if (response?.Payment != null)
                {
                    var due = decimal.TryParse(viewModel.ChangeDue.Trim('$'), out decimal dueAmt) ? dueAmt : 0;
                    var msg = due > 0 ? "Payment Complete, Change Due: " + Convert.ToDecimal(viewModel.ChangeDue).ToString("C") + "" : "Payment Complete";
                    await DisplayAlert("FocalPoint", msg, "Ok", " ");
                    if (response.GetSignature)
                    {
                        await Signature();
                    }
                    viewModel.ResetCards();
                    _ = Navigation.PopAsync();
                    _ = Navigation.PopAsync();
                    _ = Navigation.PopAsync();
                    //todo: check navigation method
                }
                else
                {
                    _ = DisplayAlert("FocalPoint", "Something went wrong.", "Ok");
                }
            }
            catch(Exception ex)
            {
                _ = DisplayAlert("FocalPoint", ex.Message, "Ok");
            }
            finally
            {
                viewModel.Indicator = false;
            }
        }

        private async Task Signature()
        {
            PaymentPageViewModel paymentPageViewModel = ((PaymentPageViewModel)this.BindingContext);
            await paymentPageViewModel.SignatureCommand(this.Navigation);
        }
    }
}