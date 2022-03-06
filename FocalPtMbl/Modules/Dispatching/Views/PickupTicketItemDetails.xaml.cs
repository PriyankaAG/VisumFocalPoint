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
            this.
            InitializeComponent();
            this.Title = "Pickup Detail: " + pickupTicketItem.EquipID.ToString();
        }


        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}