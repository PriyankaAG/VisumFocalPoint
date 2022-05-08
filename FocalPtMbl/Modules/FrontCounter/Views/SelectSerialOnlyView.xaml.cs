using FocalPoint.Modules.FrontCounter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectSerialOnlyView : ContentPage
    {
        public SelectSerialOnlyView(AvailabilityMerch selectedItem)
        {
            this.Title = "Select Serials";
            InitializeComponent();
            ((SelectSerialsOnlyViewModel)this.BindingContext).SelectedItem = selectedItem;
            ((SelectSerialsOnlyViewModel)this.BindingContext).GetSerials(selectedItem);
        }

        private void SimpleButton_Clicked(object sender, EventArgs e)
        {

        }

        private void SimpleButton_Clicked_1(object sender, EventArgs e)
        {

        }
    }
}