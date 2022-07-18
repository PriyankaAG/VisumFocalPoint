using DevExpress.XamarinForms.CollectionView;
using DevExpress.XamarinForms.Editors;
using FocalPoint.Modules.FrontCounter.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditExistingOrdersView
    {
        private bool inNavigation;
        private string selectedComboItem = "";

        public EditExistingOrdersView()
        {
            InitializeComponent();
            //DevExpress.XamarinForms.Navigation.Initializer.Init();
            Device.BeginInvokeOnMainThread(() =>
            {
                _ = ((EditExistingOrdersViewModel)this.BindingContext).GetSearchedOrdersInfo("", 1, true);
                _ = ((EditExistingOrdersViewModel)this.BindingContext).GetSearchedOrdersInfo("", 2, true);
                _ = ((EditExistingOrdersViewModel)this.BindingContext).GetSearchedOrdersInfo("", 3, true);
            });
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.inNavigation = false;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            collectionView.SelectedItem = null;
            collectionView2.SelectedItem = null;
            collectionView3.SelectedItem = null;
        }
        public async void ItemSelected(object sender, CollectionViewGestureEventArgs args)
        {
            ((EditExistingOrdersViewModel)this.BindingContext).Indicator = true;
            ((EditExistingOrdersViewModel)this.BindingContext).OrdersEnabled = false;
            _ = OpenDetails(args);
        }

        private Order GetOrderInfo(object item)
        {
            if (item is Order orderInfo)
                return orderInfo;
            return new Order();
        }

        private async Task OpenDetails(CollectionViewGestureEventArgs args)
        {
            _ = Task.Run(() =>
            {
                if (args.Item != null)
                {
                    var order = GetOrderInfo(args.Item);
                    this.inNavigation = true;
                    ViewOrderEntityComponent viewOrderEntityComponent = new ViewOrderEntityComponent();
                    _ = viewOrderEntityComponent.GetOrderDetails(order.OrderNo).ContinueWith(task =>
                    {
                        Order SelectedOrder = task.Result;
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Navigation.PushAsync(new ViewOrderDetailsView(SelectedOrder));
                            ((EditExistingOrdersViewModel)BindingContext).Indicator = false;
                            ((EditExistingOrdersViewModel)this.BindingContext).OrdersEnabled = true;
                        });
                    });
                }
            });
        }
    }

}