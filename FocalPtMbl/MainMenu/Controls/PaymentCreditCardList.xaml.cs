using FocalPoint.Modules.Payments.Types;
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
            if(BindingContext is CreditCard cardDetails)
            {
                PaymentInfo paymentInfo = (PaymentInfo)((TappedEventArgs)e).Parameter;
                cardDetails.CardDetailSelectCommand.Execute(paymentInfo);
            }
        }
    }
}