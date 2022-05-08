using FocalPoint.Modules.FrontCounter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuickOrderTotalBreakoutView : ContentPage
    {
        public QuickOrderTotalBreakoutView()
        {
            InitializeComponent();
            BindingContext = new QuickOrderTotalBreakoutViewModel();
        }
    }
}