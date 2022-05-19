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
	public partial class PaymentPageView : ContentPage
	{
		readonly PaymentPageViewModel viewModel;
		public PaymentPageView ()
		{
			InitializeComponent ();
			BindingContext = viewModel = new PaymentPageViewModel();
			
		}

        private void picker_paymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
			if(picker_paymentType.SelectedIndex != -1)
            {
				viewModel.SetCardView(picker_paymentType.SelectedItem.ToString());
            }
        }

        private void picker_DepositType_SelectedIndexChanged(object sender, EventArgs e)
        {
			if (picker_DepositType.SelectedIndex != -1)
			{
				_ = viewModel.SetPaymenyTypes(picker_DepositType.SelectedIndex);
			}
		}
    }
}