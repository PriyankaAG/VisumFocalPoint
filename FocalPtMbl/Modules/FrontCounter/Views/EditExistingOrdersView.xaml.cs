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
            this.inNavigation = false;
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

        private void TextEdit_Cleared(object sender, EventArgs e)
        {
            ((EditExistingOrdersViewModel)this.BindingContext).Indicator = true;
            ClearNow(sender);
        }
        private async Task ClearNow(object sender)
        {
            _ = Task.Run(() =>
            {
                if (sender == searchorderText)
                {
                    ((EditExistingOrdersViewModel)this.BindingContext).GetSearchedOrdersInfo("", 1, true);
                }
                else if (sender == searchreservationText)
                {
                    ((EditExistingOrdersViewModel)this.BindingContext).GetSearchedOrdersInfo("", 2, true);
                }
                else if (sender == searchquotesText)
                {
                    ((EditExistingOrdersViewModel)this.BindingContext).GetSearchedOrdersInfo("", 3, true);
                }
                    ((EditExistingOrdersViewModel)this.BindingContext).Indicator = false;
            });
        }
        private void TextEdit_Completed(object sender, EventArgs e)
        {
            ((EditExistingOrdersViewModel)this.BindingContext).Indicator = true;
            SearchNow(sender);
        }
        private async Task SearchNow(object sender)
        {
            _ = Task.Run(() =>
            {
                if (sender == searchorderText)
                {
                    ((EditExistingOrdersViewModel)this.BindingContext).GetSearchedOrdersInfo((sender as TextEdit).Text, 1, true);
                }
                else if (sender == searchreservationText)
                {
                    ((EditExistingOrdersViewModel)this.BindingContext).GetSearchedOrdersInfo((sender as TextEdit).Text, 2, true);
                }
                else if (sender == searchquotesText)
                {
                    ((EditExistingOrdersViewModel)this.BindingContext).GetSearchedOrdersInfo((sender as TextEdit).Text, 3, true);
                }
                    ((EditExistingOrdersViewModel)this.BindingContext).Indicator = false;
            });
        }
        private void ContentPage_Appearing(object sender, EventArgs e)
        {

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

        private void Search_TextChanged(object sender, EventArgs e)
        {
            var enteredText = (sender as TextEdit).Text;
            if (sender == searchorderText)
            {
                ((EditExistingOrdersViewModel)this.BindingContext).SearchForOrder(enteredText, 1, true);
            }
            else if (sender == searchreservationText)
            {
                ((EditExistingOrdersViewModel)this.BindingContext).SearchForOrder(enteredText, 2, true);
            }
            else if (sender == searchquotesText)
            {
                ((EditExistingOrdersViewModel)this.BindingContext).SearchForOrder(enteredText, 3, true);
            }
        }
    }

}