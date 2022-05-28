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
		readonly PaymentPageViewModel viewModel;
		public PaymentKindPage (PaymentType type)
		{
			InitializeComponent ();
			BindingContext = viewModel = new PaymentPageViewModel();
			viewModel.SelectedPaymentType = type;
		}

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}