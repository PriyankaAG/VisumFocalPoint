using FocalPoint.Modules.FrontCounter.ViewModels.Rentals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views.Rentals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OpenRentalDetailsView
    {
        public OpenRentalDetailsView()
        {
            InitializeComponent();
        }
        public OpenRentalDetailsView(Rental rental)
        {
            InitializeComponent();
            if (rental != null)
            {
                ((OpenRentalDetailsViewModel)this.BindingContext).CurrentRental = rental;
                ((OpenRentalDetailsViewModel)this.BindingContext).GetRate();
            }
            this.Title = "ID: " + rental.RentalEquipID + " " + rental.RentalDscr;
        }

        private void DateEdit_DateChanged(object sender, EventArgs e)
        {

        }

        private void TimeEdit_TimeChanged(object sender, EventArgs e)
        {

        }

        private void DateEdit_DateChanged_1(object sender, EventArgs e)
        {

        }

        private void TimeEdit_TimeChanged_1(object sender, EventArgs e)
        {

        }

        private void SimpleButton_Clicked(object sender, EventArgs e)
        {
            ((OpenRentalDetailsViewModel)this.BindingContext).GetRentalAvailability();
        }
    }
}