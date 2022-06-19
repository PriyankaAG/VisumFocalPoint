using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FocalPoint.Modules.FrontCounter.ViewModels.NewRental;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views.NewRentals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewQuickRentalAddItemPage : ContentPage
    {
        public NewQuickRentalAddItemPage()
        {
            InitializeComponent();
            BindingContext = new NewQuickRentalAddItemViewModel();
        }
    }
}