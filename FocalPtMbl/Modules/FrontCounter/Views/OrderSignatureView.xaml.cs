using FocalPoint.Modules.FrontCounter.ViewModels;
using SignaturePad.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderSignatureView : ContentPage
    {        
        public OrderSignatureView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            OrderSignatureViewModel orderSignatureViewModel = (OrderSignatureViewModel)this.BindingContext;
            Stream bitmap = await signatureView.GetImageStreamAsync(SignatureImageFormat.Png);
            orderSignatureViewModel.SaveSignature(bitmap);            
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            OrderSignatureViewModel orderSignatureViewModel = (OrderSignatureViewModel)this.BindingContext;
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