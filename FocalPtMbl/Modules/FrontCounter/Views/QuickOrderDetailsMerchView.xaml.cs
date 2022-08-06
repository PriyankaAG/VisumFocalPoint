using DevExpress.XamarinForms.CollectionView;
using DevExpress.XamarinForms.Editors;
using Visum.Services.Mobile.Entities;
using FocalPoint.Modules.FrontCounter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuickOrderDetailsMerchView : ContentPage
    {
        private bool inNavigation;
        private bool hasNewItemsToAdd;
        public bool HasNewItemsToAdd
        {
            get { return hasNewItemsToAdd; }
            set
            {
                if (hasNewItemsToAdd != value)
                {
                    hasNewItemsToAdd = value;
                }
            }
        }
        private List<OrderDtl> orderItems;
        private int selectedComboItem;

        public List<OrderDtl> OrderItems
        {
            get { return orderItems; }
            set
            {
                if (orderItems != value)
                {
                    orderItems = value;
                }
            }
        }
        private Order currentOrder = new Order();
        public Order CurrentOrder
        {
            get { return currentOrder; }
            set
            {
                if (currentOrder != value)
                {
                    currentOrder = value;
                }
            }
        }
        public QuickOrderDetailsMerchView(Order curOrder)
        {
            InitializeComponent();
            orderItems = new List<OrderDtl>();
            HasNewItemsToAdd = false;
            CurrentOrder = curOrder;
            this.Title = "Select Merch";

        }
        private AvailabilityMerch selItem;
        private double width;
        private double height;

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;
                if (width > height)
                {
                //    innerGrid.RowDefinitions.Clear();
                //    innerGrid.ColumnDefinitions.Clear();
                //    innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                //    innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                //    innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                //    innerGrid.Children.Remove(controlsGrid);
                //    innerGrid.Children.Add(controlsGrid, 1, 0);
                //}
                //else
                //{
                //    innerGrid.RowDefinitions.Clear();
                //    innerGrid.ColumnDefinitions.Clear();
                //    innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                //    innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                //    innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                //    innerGrid.Children.Remove(controlsGrid);
                //    innerGrid.Children.Add(controlsGrid, 0, 1);
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //CustomerAdd = new CustomerLupView();
            this.inNavigation = false;
            HasNewItemsToAdd = false;
            //Reset All fields

            OrderUpdate orderUpdate;
            //Code from question, True/False will change
            //orderUpdate.Answers.Add("","")

            //((QuickOrderDetailsViewModel)this.BindingContext).GetCustomersInfo();
            //((CustomerSimpleViewModel)this.BindingContext).GetCustomerInfo();
        }
        //public void ItemSelected(object sender, CollectionViewGestureEventArgs args)
        //{
        //    if (args.Item != null)
        //        selItem= GetMerchInfo(args.Item);
        //}
        private AvailabilityMerch GetMerchInfo(object item)
        {
            if (item is AvailabilityMerch merchInfo)
                return merchInfo;
            return new AvailabilityMerch();
        }
        Task OpenDetailPage(Customer cust)
        {
            if (cust == null)
                return Task.CompletedTask;

            if (this.inNavigation)
                return Task.CompletedTask;

            this.inNavigation = true;
            return Navigation.PopModalAsync();
        }
        private void TextEdit_Completed(object sender, EventArgs e)
        {
            ((QuickOrderDetailsMerchViewModel)this.BindingContext).GetSearchedMerchInfo((sender as TextEdit).Text);
        }
        private void TextEdit_Cleared(object sender, EventArgs e)
        {
            ((QuickOrderDetailsMerchViewModel)this.BindingContext).GetSearchedMerchInfo("");
        }
        private async void SimpleButton_Clicked(object sender, EventArgs e)
        {
            // get order info
            await Navigation.PopAsync();
        }

        private void ComboBoxEdit_SelectionChanged(object sender, EventArgs e)
        {
            var combo = sender as ComboBoxEdit;
            selectedComboItem = combo.SelectedIndex + 1;
        }

        private async void AddToOrder_Clicked(object sender, EventArgs e)
        {
            List<string> selectedSerials = new List<string>();
            string result= "0";
            //List<string> serialNumbers= ((QuickOrderDetailsMerchViewModel)this.BindingContext).GetSerials(selItem);
            //Select the serial Numbers

            //if serials exist popup a selection dialog select multi? 
            if (selItem != null)
            {
                //Check Serialized on selcted item
                if (selItem.AvailSerialized)
                {
                    //get the serial numbers
                    await Navigation.PushAsync(new SelectSerialOnlyView(selItem.AvailCmp, selItem.AvailItem));
                }
                else
                {
                    result = await DisplayPromptAsync("Pick Quantity", "Enter in the Quantity", keyboard: Keyboard.Numeric);
                }

                if (result != null && Convert.ToDecimal(result) > 0)
                {
                    decimal numberOfItems = Convert.ToDecimal(result);
                    OrderUpdate UpdatedOrder = ((QuickOrderDetailsMerchViewModel)this.BindingContext).AddItem(selItem, numberOfItems, CurrentOrder);
                    if (UpdatedOrder != null && UpdatedOrder.Order != null)
                    {
                        MessagingCenter.Send<QuickOrderDetailsMerchView, OrderUpdate>(this, "Hi", UpdatedOrder);
                        await Navigation.PopAsync();
                    }
                    else if (UpdatedOrder.Order == null)
                    {
                        //ask questions ' Show Message and return no numbers for not assigning else return number to assign equal to qty
                        //List<string> serialNumbers = ((QuickOrderDetailsMerchViewModel)this.BindingContext).GetSerials(selItem);
                        await Navigation.PushAsync(new SelectSerialOnlyView(selItem.AvailCmp, selItem.AvailItem));
                        //string action = await DisplayActionSheet("Select Serialized Items", "Cancel", null, serialNumbers);
                        //await DisplayAlert("Serialized Item", "Item not added", "ok");
                    }
                    else
                    {
                        await DisplayAlert("Item not added", "Item not added", "ok");
                    }
                }
            }
            else
               await DisplayAlert("Select Item", "Please Search and select an Item.", "ok");
        }
        private List<string> serials;
        private bool hasSerialsContinue;

        private void GetSerials(AvailabilityMerch selItem)
        {
            throw new NotImplementedException();
        }

        private void grid_SelectionChanged(object sender, DevExpress.XamarinForms.DataGrid.SelectionChangedEventArgs e)
        {
            if (e.Item != null)
                if (selItem == null)
                {
                    selItem = new AvailabilityMerch();
                }
                selItem = GetMerchInfo(e.Item);
        }

        private void grid_DoubleTap(object sender, DevExpress.XamarinForms.DataGrid.DataGridGestureEventArgs e)
        {
            AddToOrder_Clicked(sender, e);
        }
    }
}