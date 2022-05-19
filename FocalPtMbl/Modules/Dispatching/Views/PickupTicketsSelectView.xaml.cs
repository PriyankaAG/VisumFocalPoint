using DevExpress.XamarinForms.CollectionView;
using DevExpress.XamarinForms.Editors;
using FocalPoint.Modules.Dispatching.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
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
            ((PickupTicketsSelectViewModel)BindingContext).GetSearchedTicketInfo(searchText.Text.ToLower());
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        public async void ItemSelected(object sender, CollectionViewGestureEventArgs args)
        {
            if (args.Item != null)
            {
                ((PickupTicketsSelectViewModel)BindingContext).SelectedTicket = (PickupTicket)args.Item;
                await OpenDetailPage(((PickupTicketsSelectViewModel)BindingContext).GetTicketInfo(args.Item));
            }
        }
        async Task OpenDetailPage(PickupTicket ticket)
        {
            PickupTicket detailedTicket = await ((PickupTicketsSelectViewModel)BindingContext).GetDetailedTicketInfo(ticket);
            if (detailedTicket == null)
            {
                await DisplayAlert("FocalPoint", "No details on this Pickup Ticket.", "OK");
                return;
            }
            await Navigation.PushAsync(new PickupTicketPage(detailedTicket));
        }

        private void Search_TextChanged(object sender, EventArgs e)
        {
            var enteredText = (sender as TextEdit).Text;
            ((PickupTicketsSelectViewModel)BindingContext).GetSearchedTicketInfo(enteredText.ToLower());
        }
        private void Search_TextCleared(object sender, EventArgs e)
        {
            ((PickupTicketsSelectViewModel)BindingContext).GetSearchedTicketInfo("");
        }
    }
}