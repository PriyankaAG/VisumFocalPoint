using DevExpress.XamarinForms.CollectionView;
using DevExpress.XamarinForms.Editors;
using FocalPoint.Modules.FrontCounter.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;
using Visum.Services.Mobile.Entities;
using FocalPoint.Components.EntityComponents;
using Xamarin.Forms;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditExistingOrdersView
    {
        private bool inNavigation;
        private string selectedComboItem="";

        public EditExistingOrdersView()
        {
            InitializeComponent();
            DevExpress.XamarinForms.Navigation.Initializer.Init();
            //this.viewModel = new EditExistingOrdersViewModel();
            //BindingContext = new EditExistingOrdersViewModel();
            ((EditExistingOrdersViewModel)this.BindingContext).GetSearchedOrdersInfo("", 1, true);
            ((EditExistingOrdersViewModel)this.BindingContext).GetSearchedOrdersInfo("", 2, true);
            ((EditExistingOrdersViewModel)this.BindingContext).GetSearchedOrdersInfo("", 3, true);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //CustomerAdd = new CustomerLupView();
            this.inNavigation = false;
            //((QuickOrderDetailsViewModel)this.BindingContext).GetCustomersInfo();
            //((CustomerSimpleViewModel)this.BindingContext).GetCustomerInfo();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            collectionView.SelectedItem = null;
        }
        public async void ItemSelected(object sender, CollectionViewGestureEventArgs args)
        {
            ((EditExistingOrdersViewModel)this.BindingContext).Indicator = true;
            ((EditExistingOrdersViewModel)this.BindingContext).OrdersEnabled = false;
            _ = OpenDetails(args);

            //if (args.Item != null)
            //    await OpenDetailPage(GetOrderInfo(args.Item));
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

        //async Task<Task> OpenDetailPage(Order order)
        //{
        //    if (order == null)
        //        return Task.CompletedTask;

        //    if (this.inNavigation)
        //        return Task.CompletedTask;

        //    this.inNavigation = true;
        //    ViewOrderEntityComponent viewOrderEntityComponent = new ViewOrderEntityComponent();
        //    Order SelectedOrder= await viewOrderEntityComponent.GetOrderDetails(order.OrderNo);
        //    //Goto New Order Page
        //    return Navigation.PushAsync(new ViewOrderDetailsView(SelectedOrder));
        //}
        private void TextEdit_Completed(object sender, EventArgs e)
        {
            ((EditExistingOrdersViewModel)this.BindingContext).GetSearchedOrdersInfo((sender as TextEdit).Text,1, true);
        }
        private void TextEdit_Cleared(object sender, EventArgs e)
        {
            ((EditExistingOrdersViewModel)this.BindingContext).GetSearchedOrdersInfo("",1, true);
        }
        private void ContentPage_Appearing(object sender, EventArgs e)
        {

        }

        private void TextEdit_Completed_1(object sender, EventArgs e)
        {
            ((EditExistingOrdersViewModel)this.BindingContext).GetSearchedOrdersInfo((sender as TextEdit).Text, 2, true);
        }
        private void TextEdit_Cleared_1(object sender, System.ComponentModel.HandledEventArgs e)
        {
            ((EditExistingOrdersViewModel)this.BindingContext).GetSearchedOrdersInfo("", 2, true);
        }




        private void SimpleButton_Clicked(object sender, EventArgs e)
        {

        }

        private void SimpleButton_Clicked_1(object sender, EventArgs e)
        {

        }

        private void SimpleButton_Clicked_2(object sender, EventArgs e)
        {

        }

        private void SimpleButton_Clicked_3(object sender, EventArgs e)
        {

        }

        private void TextEdit_Completed_2(object sender, EventArgs e)
        {
            ((EditExistingOrdersViewModel)this.BindingContext).GetSearchedOrdersInfo((sender as TextEdit).Text, 3, true);
        }
        private void TextEdit_Cleared_2(object sender, System.ComponentModel.HandledEventArgs e)
        {
            ((EditExistingOrdersViewModel)this.BindingContext).GetSearchedOrdersInfo("", 3, true);
        }

    }
}