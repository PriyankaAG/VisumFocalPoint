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
            FrontCounterViewModel frontCounterDashboardViewModel = new FrontCounterViewModel();
            //frontCounterDashboardViewModel.GetDetails();
            BindingContext = frontCounterDashboardViewModel;
        }        
    }
}