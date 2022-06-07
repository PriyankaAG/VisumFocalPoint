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
	public partial class PaymentProcessView : ContentView
	{
		PaymentPageViewModel viewModel;
		public PaymentProcessView ()
		{
			InitializeComponent();
		}
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			viewModel = (PaymentPageViewModel)BindingContext;
		}
	}
}