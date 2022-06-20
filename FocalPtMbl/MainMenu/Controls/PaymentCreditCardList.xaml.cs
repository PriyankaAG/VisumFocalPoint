using FocalPoint.Modules.Payments.Types;
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
            //if(BindingContext is PaymentPageViewModel paymentPageViewModel)
            foreach (View item in ((StackLayout)(sender as Grid).Parent).Children)
            {
                item.BackgroundColor = Color.FromHex("#f5f6fa");
            }
            
            ((Grid)sender).BackgroundColor = Color.Red;
            if(BindingContext is CreditCard cardDetails)
            {
                PaymentInfo paymentInfo = (PaymentInfo)((TappedEventArgs)e).Parameter;
                cardDetails.CardDetailSelectCommand.Execute(paymentInfo);
            }
        }
    }
}