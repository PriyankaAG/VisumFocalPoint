using FocalPoint.Modules.FrontCounter.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FrontCounter : ContentPage
    {
        public FrontCounter()
        {
            InitializeComponent();
            FrontCounterViewModel frontCounterDashboardViewModel = new FrontCounterViewModel();           
            BindingContext = frontCounterDashboardViewModel;
            Device.BeginInvokeOnMainThread(async () =>
            {
                await frontCounterDashboardViewModel.GetDashboardDetail();
            });
        }

        private void Counter_Clicked(object sender, System.EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (BindingContext is FrontCounterViewModel frontCounterViewModel)
                {
                    frontCounterViewModel.ChangeButtonStyle(true);
                    grdUtilization.IsVisible = false;
                    RentalCounterDetailView.IsVisible = true;
                    WorkOrderDetailView.IsVisible = true;
                    GrandTotalDetailView.IsVisible = true;
                }
            });
        }

        private void Utilization_Clicked(object sender, System.EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (BindingContext is FrontCounterViewModel frontCounterViewModel)
                {
                    frontCounterViewModel.ChangeButtonStyle(false);
                    grdUtilization.IsVisible = true;
                    RentalCounterDetailView.IsVisible = false;
                    WorkOrderDetailView.IsVisible = false;
                    GrandTotalDetailView.IsVisible = false;
                }
            });
        }
    }
}