using FocalPoint.Modules.ViewModels;
using SignaturePad.Forms;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignatureView : ContentPage
    {        
        public SignatureView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, "AllowLandscape");
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send(this, "PreventLandscape");
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            SignatureViewModel orderSignatureViewModel = (SignatureViewModel)this.BindingContext;
            Stream bitmap = await signatureView.GetImageStreamAsync(SignatureImageFormat.Png);
            orderSignatureViewModel.SaveSignature(bitmap);            
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            SignatureViewModel orderSignatureViewModel = (SignatureViewModel)this.BindingContext;
            if (orderSignatureViewModel.IsWaiver)
            {
                DisplayAlert("FocalPoint Mobile", "Damage Waiver Rejected", "OK");
            }
            else
            {
                DisplayAlert("FocalPoint Mobile", "Signature Canceled", "OK");
            }
            Navigation.PopAsync();
        }
    }
}