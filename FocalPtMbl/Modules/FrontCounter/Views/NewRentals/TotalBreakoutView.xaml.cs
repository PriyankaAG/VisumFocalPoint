using FocalPoint.Modules.FrontCounter.ViewModels.NewRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views.NewRentals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TotalBreakoutView : ContentPage
    {
        public TotalBreakoutView(Order ord)
        {
            InitializeComponent();
            Title = "Totals Breakout";
            this.BindingContext = new TotalBreakoutViewModel(ord);
        }
    }
}