using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FocalPoint.Modules.Payments.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Visum.Services.Mobile.Entities.PaymentRequest;

namespace FocalPoint.Modules.Payments.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentCash : ContentView
    {
        PaymentPageViewModel viewModel;
        public PaymentCash()
        {
            InitializeComponent();
        }
       
        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = e.CurrentSelection.FirstOrDefault();
            var paymentValue = Convert.ToDecimal(selectedItem.ToString().Trim('$'));
            ((PaymentPageViewModel)BindingContext).SetSelectedPayment(paymentValue);
        }
    }
}