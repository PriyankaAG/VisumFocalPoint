using DevExpress.XamarinForms.CollectionView;
using DevExpress.XamarinForms.Editors;
using FocalPoint.Modules.FrontCounter.ViewModels.Rentals;
using FocalPtMbl.MainMenu.ViewModels.Services;
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
    public partial class OpenRentalsView : ContentPage
    {
        public OpenRentalsView()
        {
            InitializeComponent();
            //this.Title = "View";
        }
        
        public async void ItemSelected(object sender, CollectionViewGestureEventArgs args)
        {
            if (args.Item != null)
            {
                await OpenDetailPage(GetOrderInfo(args.Item));
                collectionView.SelectedItem = null;
            }
        }
        private Rental GetOrderInfo(object item)
        {
            if (item is Rental orderInfo)
                return orderInfo;
            return new Rental();
        }
        Task OpenDetailPage(Rental order)
        {
            if (order == null)
                return Task.CompletedTask;

            //if (this.inNavigation)
            //    return Task.CompletedTask;

            //this.inNavigation = true;
            //Goto New Order Page
            //return Navigation.PushAsync(new OpenRentalDetailsView(order));
            var NavSer = DependencyService.Resolve<INavigationService>();
            NavSer.PushChildPage(new OpenRentalDetailsView(order));
            return Task.CompletedTask;
        }

        private void TextEdit_ClearIconClicked(object sender, System.ComponentModel.HandledEventArgs e)
        {
            ((OpenRentalsViewModel)this.BindingContext).GetRentals("");
        }

        private void TextEdit_Completed(object sender, EventArgs e)
        {
            ((OpenRentalsViewModel)this.BindingContext).GetRentals((sender as TextEdit).Text);
        }

        private void collectionView_SelectionChanged(object sender, DevExpress.XamarinForms.CollectionView.CollectionViewSelectionChangedEventArgs e)
        {

        }

        private void SimpleButton_Clicked(object sender, EventArgs e)
        {

        }

        private void collectionView_Tap(object sender, DevExpress.XamarinForms.CollectionView.CollectionViewGestureEventArgs e)
        {

        }
    }
}