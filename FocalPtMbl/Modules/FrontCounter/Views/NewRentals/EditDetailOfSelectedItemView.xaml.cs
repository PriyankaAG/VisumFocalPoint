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
    public partial class EditDetailOfSelectedItemView : ContentPage
    {
        public EditDetailOfSelectedItemView(OrderDtl ordDetails)
        {
            InitializeComponent();
            Title = "Edit Merc";
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {

        }
    }
}