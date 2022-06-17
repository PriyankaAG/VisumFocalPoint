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
        NewQuickRentalAddCustomerViewModel theViewModel;
        public NewQuickRentalAddCustomerPage()
        {
            InitializeComponent();
            Title = "Add New Customer";
            theViewModel = new NewQuickRentalAddCustomerViewModel();
            BindingContext = theViewModel;

            Device.BeginInvokeOnMainThread(async () =>
            {
                await theViewModel.FetchMasters();
            });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

            if (!theViewModel.ValidateData()) return;

            var res1 = theCityDropDown.SelectedIndex;
            var res = theCityDropDown.SelectedItem;

            theViewModel.CustomerToAdd.CustomerAddr1 = "Kalyaj";
            theViewModel.CustomerToAdd.CustomerAddr2 = "Pimpri";

            var tt = "Pune";

            theViewModel.CustomerCityString = tt;
            theViewModel.CustomerToAdd.CustomerStatus = "Light Hold";

            theViewModel.NotifyPropChanged("CustomerCityString");

            theViewModel.CustomerCountryString = "Canada";
            theViewModel.NotifyPropChanged("CustomerCityString");
        }
    }
}