using FocalPoint.Modules.FrontCounter.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RentalResultsView : ContentPage
    {
        public RentalResultsView(ObservableCollection<RentalAvailability> searchedRentals)
        {
            BindingContext = new RentalResultsViewModel(searchedRentals);
            this.Title = "Rental Availability";
            InitializeComponent();
        }
    }
}