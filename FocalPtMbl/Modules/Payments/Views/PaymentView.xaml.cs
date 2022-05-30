using FocalPoint.Modules.Payments.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Payments.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentView : ContentPage
    {
        PaymentPageViewModel viewModel;
        public PaymentView()
        {
            InitializeComponent();
            //BindingContext = new PaymentViewModel();
            BindingContext = viewModel = new PaymentPageViewModel();
        }

        private async void Payment_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PaymentMethodsPage(0));

        }

        private async void Deposit_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PaymentMethodsPage(1));
        }

        private async void SecurityDeposit_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PaymentMethodsPage(2));
        }

        private async void Security_PreAuth_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PaymentMethodsPage(3));
        }
    }
}