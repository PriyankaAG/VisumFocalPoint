using DevExpress.XamarinForms.CollectionView;
using DevExpress.XamarinForms.Editors;
using FocalPoint.Modules.Dispatching.ViewModels;
using System;
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
                ((PickupTicketsSelectViewModel)BindingContext).SelectedTicket = (PickupTicket)args.Item;
                await OpenDetailPage(GetTicketInfo(args.Item));
            }
        }
        private PickupTicket GetTicketInfo(object item)
        {
            if (item is PickupTicket pTicket)
                return pTicket;
            return new PickupTicket();
        }
        async Task OpenDetailPage(PickupTicket ticket)
        {
            PickupTicket detailedTicket = await ((PickupTicketsSelectViewModel)BindingContext).GetDetailedTicketInfo(ticket);
            if(detailedTicket == null)
            {
                await DisplayAlert("FocalPoint", "No details on this Pickup Ticket.", "OK");
                return;
            }
            await Navigation.PushAsync(new PickupTicketView(detailedTicket));
        }
        private void TextEdit_Completed(object sender, EventArgs e)
        {
            ((PickupTicketsSelectViewModel)BindingContext).GetSearchedTicketInfo((sender as TextEdit).Text);
        }
        private void TextEdit_Cleared(object sender, EventArgs e)
        {
            ((PickupTicketsSelectViewModel)BindingContext).GetSearchedTicketInfo("");
        }

        private void TextEdit_ClearIconClicked(object sender, System.ComponentModel.HandledEventArgs e)
        {
            ((PickupTicketsSelectViewModel)BindingContext).GetSearchedTicketInfo("");
        }
    }
}