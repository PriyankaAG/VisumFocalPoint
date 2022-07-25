using DevExpress.XamarinForms.CollectionView;
using DevExpress.XamarinForms.Editors;
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
    public partial class VendorsView : ContentPage
    {
        private bool inNavigation;

        public VendorsView()
        {
            //DevExpress.XamarinForms.CollectionView.Initializer.Init();
            InitializeComponent();
            //BindingContext = new VendorsViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.inNavigation = false;
        }
        public async void ItemSelected(object sender, CollectionViewGestureEventArgs args)
        {
            if (args.Item != null)
            {
                await OpenDetailPage(GetCustInfo(args.Item));
                collectionView.SelectedItem = null;
            }
        }
        private Vendor GetCustInfo(object item)
        {
            if (item is Vendor vendorInfo)
                return vendorInfo;
            return new Vendor();
        }
        Task OpenDetailPage(Vendor vend)
        {
            if (vend == null)
                return Task.CompletedTask;

            if (this.inNavigation)
                return Task.CompletedTask;

            this.inNavigation = true;
            return Navigation.PushAsync(new VendorDetailView(vend));
        }
        private void TextEdit_Completed(object sender, EventArgs e)
        {
            ((VendorsViewModel)this.BindingContext).GetSearchedVendorsInfo((sender as TextEdit).Text);
        }
        private void TextEdit_Cleared(object sender, EventArgs e)
        {
            ((VendorsViewModel)this.BindingContext).GetSearchedVendorsInfo("");
        }
    }
}