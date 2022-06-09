using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FocalPoint.Modules.Payments.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            viewModel = (PaymentPageViewModel)BindingContext;
            viewModel.Payment = viewModel.TotalReceived = viewModel.ChangeDue = 0.0.ToString("c");
        }
        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = e.CurrentSelection.FirstOrDefault();
            var paymentValue = Convert.ToDecimal(selectedItem.ToString().Trim('$'));
            ((PaymentPageViewModel)BindingContext).SetPayment(paymentValue);
        }
    }
}