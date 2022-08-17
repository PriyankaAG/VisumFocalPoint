using DevExpress.XamarinForms.DataGrid;
using FocalPoint.Modules.Inventory.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Inventory.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VendorDetailView 
    {
        readonly VendorDetailViewModel viewModel;
        public VendorDetailView(Vendor vend)
        {
            //DevExpress.XamarinForms.Navigation.Initializer.Init();
            this.viewModel = new VendorDetailViewModel(vend);
            InitializeComponent();
            BindingContext = this.viewModel;
        }
        public async void On_ItemSelected(object sender, DataGridGestureEventArgs args)
        {

        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            // do the update to Balance
        }
    }
}