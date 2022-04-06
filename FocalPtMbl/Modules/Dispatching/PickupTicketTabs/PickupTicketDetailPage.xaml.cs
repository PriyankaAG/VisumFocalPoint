using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FocalPoint.Modules.Dispatching.ViewModels;
using FocalPoint.Modules.Dispatching.Views;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Dispatching.PickupTicketTabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickupTicketDetailPage : ContentPage
    {
        PickupTicketViewModel viewModel;
        bool _continue = true;
        bool _refresh = true;

        public PickupTicketDetailPage()
        {
            InitializeComponent();
        }
        async protected override void OnAppearing()
        {
            viewModel = this.BindingContext as PickupTicketViewModel;
            base.OnAppearing();
            if (_refresh)
            {
                await RefreshTicket(true);
                if(!viewModel.IsSelectPickupItemVisible)
                {
                    Device.StartTimer(TimeSpan.FromSeconds(30), () =>
                    {
                        Task.Run(async () => await RefreshTicket(false));
                        return _continue;
                    });
                }
            }
            else
                _refresh = true;

            MessagingCenter.Unsubscribe<PickupTicketItemDetailsViewModel, Tuple<PickupTicketItem, bool>>(this, "ItemDetails");
            MessagingCenter.Subscribe<PickupTicketItemDetailsViewModel, Tuple<PickupTicketItem, bool>>(this, "ItemDetails", async (sender, details) =>
            {
                await ItemDetailsCallback(details);
            });

            MessagingCenter.Unsubscribe<PickupDetailMobileSelect, bool>(this, "MobileTicket");
            MessagingCenter.Subscribe<PickupDetailMobileSelect, bool>(this, "MobileTicket", async (sender, isSuccess) =>
            {
                if(isSuccess)
                {
                    await RefreshTicket(true);
                }
            });
        }

        private async Task ItemDetailsCallback(Tuple<PickupTicketItem, bool> details)
        {
            viewModel.SelectedItem = details.Item1;
            if (details.Item2)
            {
                await CheckPopupValues();
                var updatedItem = await viewModel.UpdateItem(details.Item1);
                viewModel.SelectedItemChecked(updatedItem, true, true);
                //await viewModel.UpdateItem();
            }
            else
                viewModel.SelectedItemChecked(viewModel.SelectedItem, false, true);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _continue = false;
        }

        bool _refreshing = false;

        async Task RefreshTicket(bool progress = false)
        {
            if (_refreshing)
                return;

            try
            {
                _refreshing = true;

                //if (progress)
                //    App.Platform.Show("Refreshing...");

                var ticket = await viewModel.GetTicketInfo(viewModel.Ticket.PuTNo.ToString());

                if (_continue)
                {
                    viewModel.Ticket = ticket;
                }
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }
            finally
            {
                _refreshing = false;
                //if (progress)
                //    App.Platform.Dismiss();

            }
        }

        private void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                ((PickupTicketViewModel)BindingContext).SelectedItem = (PickupTicketItem)e.SelectedItem;
            }
        }
        private async void DetailLine_Tapped(object sender, EventArgs e)
        {
            var parent = (Grid)sender;
            viewModel.SelectedItem = (PickupTicketItem)parent.BindingContext;
            viewModel.SelectedItem = await viewModel.UpdateItem(viewModel.SelectedItem);
            await Navigation.PushAsync(new PickupTicketItemDetails(viewModel.SelectedItem));
        }
        async protected void CheckBoxTapped(object sender, EventArgs args)
        {
            var imageSender = (Image)sender;
            var parent = (Grid)imageSender.Parent;
            viewModel.SelectedItem = (PickupTicketItem)parent.BindingContext;

            bool isChecked;
            viewModel.SelectedItem.Checked = isChecked = !viewModel.SelectedItem.Checked;

            if (isChecked)
            {
                var isSuccess = await CheckPopupValues();
                if (!isSuccess)
                    return;
            }
            viewModel.SelectedItemChecked(viewModel.SelectedItem,isChecked);
            try
            {
                viewModel.Indicator = true;
                bool update = await viewModel.PickupTicketItemCount(viewModel.SelectedItem);
                viewModel.SelectedItem = await viewModel.UpdateItem(viewModel.SelectedItem);
                if (!update)
                {
                    await DisplayAlert("FocalPoint", isChecked ? "Item Counted by Another, Counts Reloaded." :
                        "Item Counted by Another, Last Counts Reloaded.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("FocalPoint-Error", "Failed to update Item.", "OK");
            }
            finally
            {
                viewModel.Indicator = false;
            }
        }

        async Task<bool> CheckPopupValues()
        {
            List<string> popUpCount = viewModel.GetPopUpCount();
            foreach (var popupString in popUpCount)
            {
                double initValue = viewModel.GetPopupType(popupString);
                string result = await DisplayPromptAsync("Change Pickup", popupString, initialValue: initValue.ToString(), keyboard: Keyboard.Numeric);
                if (result != null)
                    viewModel.setPopupValue(popupString, result);
                else
                    return false;
            }
            return true;
        }

        private async void MobilePickupClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PickupDetailMobileSelect(viewModel.Ticket));
        }
    }
}