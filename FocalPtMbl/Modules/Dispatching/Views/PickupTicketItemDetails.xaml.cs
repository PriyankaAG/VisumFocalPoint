using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Dispatching.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickupTicketItemDetails : ContentPage
    {
        public PickupTicketItemDetails(PickupTicketItem pickupTicketItem)
        {
            InitializeComponent();
            this.Title = "Count Adjustment";
        }


        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}