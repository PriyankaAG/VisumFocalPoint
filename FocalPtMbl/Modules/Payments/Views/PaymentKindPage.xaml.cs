using System;
using FocalPoint.Modules.Payments.ViewModels;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Payments.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaymentKindPage : ContentPage
	{
        public PaymentPageViewModel viewModel { get; set; }
        public PaymentKindPage()
		{
			InitializeComponent ();
		}

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}