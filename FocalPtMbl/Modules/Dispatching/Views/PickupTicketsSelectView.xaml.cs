using DevExpress.XamarinForms.CollectionView;
using DevExpress.XamarinForms.Editors;
using FocalPoint.Modules.Dispatching.ViewModels;
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
    public partial class PickupTicketsSelectView : ContentPage
    {
        public PickupTicketsSelectView()
        {
            InitializeComponent();
            BindingContext = new PickupTicketsSelectViewModel();
            this.Title = "Pickup Tickets ";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((PickupTicketsSelectViewModel)this.BindingContext).GetPickupTicketInfo();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // EventPass("Back Code");
        }
        public async void ItemSelected(object sender, CollectionViewGestureEventArgs args)
        {
            if (args.Item != null)
            {
                ((PickupTicketsSelectViewModel)this.BindingContext).SelectedTicket = (PickupTicket)args.Item;
                await OpenDetailPage(GetTicketInfo(args.Item));
                if(((PickupTicketsSelectViewModel)this.BindingContext).SelectedTicket != null)
                ((PickupTicketsSelectViewModel)this.BindingContext).UnlockTicket(((PickupTicketsSelectViewModel)this.BindingContext).SelectedTicket);
            }
        }
        private PickupTicket GetTicketInfo(object item)
        {
            if (item is PickupTicket pTicket)
                return pTicket;
            return new PickupTicket();
        }
        Task OpenDetailPage(PickupTicket ticket)
        {
            if (ticket == null)
                return Task.CompletedTask;

            PickupTicket detailedTicket = ((PickupTicketsSelectViewModel)this.BindingContext).GetDetailedTicketInfo(ticket);
            //MessagingCenter.Send<SelectCustomerView, string>(this, "Hi", "John");
            //MessagingCenter.Send<SelectCustomerView, Customer>(this, "Hi", cust);
            //EventPass(((SelectCustomerViewModel)this.BindingContext).SelectedCustomer);
            //return Navigation.PushAsync(new PickupTicketView(detailedTicket));
            return Navigation.PopAsync();
        }
        private void TextEdit_Completed(object sender, EventArgs e)
        {
            ((PickupTicketsSelectViewModel)this.BindingContext).GetSearchedTicketInfo((sender as TextEdit).Text);
        }
        private void TextEdit_Cleared(object sender, EventArgs e)
        {
            ((PickupTicketsSelectViewModel)this.BindingContext).GetSearchedTicketInfo("");
        }

        private void TextEdit_ClearIconClicked(object sender, System.ComponentModel.HandledEventArgs e)
        {
            ((PickupTicketsSelectViewModel)this.BindingContext).GetSearchedTicketInfo("");
        }
    }
}