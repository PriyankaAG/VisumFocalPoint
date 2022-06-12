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
    public partial class NewQuickRentalAddCustomerPage : ContentPage
    {
        public NewQuickRentalAddCustomerPage()
        {
            InitializeComponent();
            Title = "Add New Customer";
            NewQuickRentalAddCustomerViewModel theViewModel = new NewQuickRentalAddCustomerViewModel();
            BindingContext = theViewModel;
        }

        private void LabelDropDownCustomControl_ItemSelected(object sender, CustomControls.ItemSelectedEventArgs e)
        {

        }
    }
}