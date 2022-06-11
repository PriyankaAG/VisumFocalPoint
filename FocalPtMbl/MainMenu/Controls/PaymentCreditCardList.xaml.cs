using FocalPoint.Modules.Payments.ViewModels;
using System;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.MainMenu.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentCreditCardList : ContentView
    {
        public PaymentCreditCardList()
        {
            InitializeComponent();
        }

        private void CreditCardDetail_Tapped(object sender, EventArgs e)
        {
            if(BindingContext is PaymentPageViewModel paymentPageViewModel)
            {
                PaymentInfo paymentInfo = (PaymentInfo)((TappedEventArgs)e).Parameter;
                paymentPageViewModel.CardDetailSelectCommand.Execute(paymentInfo);
            }
        }
    }
}