using FocalPoint.Modules.FrontCounter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            FrontCounterDashboardViewModel frontCounterDashboardViewModel = new FrontCounterDashboardViewModel();
            frontCounterDashboardViewModel.GetDashboardDetail().GetAwaiter().GetResult();
            BindingContext = frontCounterDashboardViewModel;
        }
    }
}