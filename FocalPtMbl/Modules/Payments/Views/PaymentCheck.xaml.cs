using FocalPoint.Modules.Payments.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Payments.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaymentCheck : ContentView
	{
		PaymentPageViewModel viewModel;
		public PaymentCheck ()
		{
			InitializeComponent ();
		}
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			viewModel = (PaymentPageViewModel)BindingContext;
			viewModel.ResetCheck();
		}
	}
}