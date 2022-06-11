using FocalPoint.Modules.FrontCounter.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FrontCounterDashboard : ContentPage
    {
        public FrontCounterDashboard()
        {
            InitializeComponent();
            FrontCounterViewModel frontCounterDashboardViewModel = new FrontCounterViewModel();
            //frontCounterDashboardViewModel.GetDashboardDetail();
            BindingContext = frontCounterDashboardViewModel;
        }
    }
}