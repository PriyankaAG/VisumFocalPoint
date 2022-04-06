using System;
using System.Threading.Tasks;
using FocalPoint.Modules.Dispatching.ViewModels;
using FocalPoint.Modules.FrontCounter.Views;
using FocalPoint.Modules.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Dispatching.PickupTicketTabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickupTicketHeaderPage : ContentPage
    {
        public PickupTicketHeaderPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Unsubscribe<SignatureViewModel, string>(this, "Signature");
            MessagingCenter.Subscribe<SignatureViewModel, string>(this, "Signature", async (sender, capturedImage) =>
            {
                PickupTicketViewModel viewOrderDetailsViewModel = (PickupTicketViewModel)BindingContext;
                viewOrderDetailsViewModel.SignatureImage = capturedImage;
                bool success = await viewOrderDetailsViewModel.SaveSignature();
                if (success)
                {
                    await DisplayAlert("FocalPoint Mobile", "Signature added successfully", "OK");
                }
                else
                {
                    await DisplayAlert("FocalPoint Mobile", "Failed to add Signature", "OK");
                }
                await Navigation.PopAsync();
                await Navigation.PopAsync();
            });
        }
        private async void CompleteCountButton_Clicked(object sender, EventArgs e)
        {
            if (((PickupTicketViewModel)this.BindingContext).ToBeCounted > 0)
            {
                await DisplayAlert("FocalPoint Mobile", "Please Count All Detail Lines.", "OK");
                return;
            }
            var underCount = false;
            foreach (var itemVm in ((PickupTicketViewModel)BindingContext).Details)
            {
                if (((PickupTicketViewModel)BindingContext).Totals > itemVm.PuDtlQty)
                {
                    await DisplayAlert("FocalPoint", "Items Over Counted, Please check details for Errors.", "OK");
                    return;
                }

                if (((PickupTicketViewModel)BindingContext).Totals > 0 && ((PickupTicketViewModel)BindingContext).Totals < itemVm.PuDtlQty)
                    underCount = true;
            }
            if (underCount)
            {
                string proceed = await DisplayPromptAsync("FocalPoint", "Some Items are under Counted, Are you sure you want to Complete?", "Yes", "No");

                if (proceed == "No")
                    return;
            }
            //App.Platform.Show("Sending Counts...");

            var success = await ((PickupTicketViewModel)BindingContext).PickupTicketCounted();
            if (success)
            {
                await ShowSignatureScreen();
            }

        }
        private async Task ShowSignatureScreen()
        {
            SignatureViewModel signatureViewModel = new SignatureViewModel(false, "Sign below to accept Pickup");
            var orderSignatureView = new SignatureView
            {
                BindingContext = signatureViewModel
            };
            await Navigation.PushAsync(orderSignatureView);
        }
    }
}