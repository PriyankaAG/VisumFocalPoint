using System;
using System.Threading.Tasks;
using FocalPoint.Modules.Dispatching.ViewModels;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Dispatching.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickupTicketItemDetails : ContentPage
    {
        readonly PickupTicketItemDetailsViewModel viewModel;

        public PickupTicketItemDetails(PickupTicketItem pickupTicketItem)
        {
            viewModel = new PickupTicketItemDetailsViewModel(pickupTicketItem);
            BindingContext = viewModel;
            Title = "Count Adjustment";
            InitializeComponent();


            Counted.Focused -= Counted_Focused;
            Counted.Focused += Counted_Focused;

        }

        private void Counted_Focused(object sender, FocusEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => SetForEntry(e.IsFocused));
        }

        void SetForEntry(bool focused)
        {
            ((PickupTicketItemDetailsViewModel)this.BindingContext).Refresh();

            //LastCountGrid.IsVisible = !focused;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {

            var vm = (PickupTicketItemDetailsViewModel)this.BindingContext;
            vm.SelectedDetail.UTCCountDte = DateTime.UtcNow;
            if (!vm.IsAccountedEqualToPickedUp())
            {
                await DisplayAlert("FocalPoint", "Picked up must be equal to Accounted For", "Ok");
                return;
            }
            var result = await CheckPopupValues(vm.SelectedDetail);
            if (!result)
            {
                vm.SelectedDetail = viewModel.OriginalPickupItem;
                await Navigation.PopAsync();
                return;
            }
            try
            {
                viewModel.Indicator = true;
                var countRes = await viewModel.PickupTicketItemCount();
                if (!countRes)
                {
                    await DisplayAlert("FocalPoint", "Item Counted by Another, last Counts Reloaded", "Ok");
                    vm.SelectedDetail = viewModel.OriginalPickupItem;
                    await Navigation.PopAsync();
                    viewModel.UpdateTicket();
                    return;
                }
                await Navigation.PopAsync();
                viewModel.UpdateTicket();
            }
            catch (Exception ex)
            {
                await DisplayAlert("FocalPoint-Error", ex.Message, "Ok");
            }
            finally
            {
                viewModel.Indicator = false;
            }
        }
        async Task<bool> CheckPopupValues(PickupTicketItem itemVm)
        {
            if (itemVm.OrderDtlMeterType != "N")
            {
                string result = await DisplayPromptAsync("Meter", "",
                    initialValue: ((PickupTicketItemDetailsViewModel)this.BindingContext).SelectedDetail.PuDtlMeterIn.ToString(), keyboard: Keyboard.Numeric);
                if (result != null && double.TryParse(result, out double meter))
                    ((PickupTicketItemDetailsViewModel)this.BindingContext).SelectedDetail.PuDtlMeterIn = meter;
                else
                    return false;
            }

            if (itemVm.OrderDtlFuelType != "N")
            {
                string result = await DisplayPromptAsync("Tank", "",
                    initialValue: ((PickupTicketItemDetailsViewModel)this.BindingContext).SelectedDetail.PuDtlTank.ToString(), keyboard: Keyboard.Numeric);
                if (result != null && double.TryParse(result, out double tank))
                    ((PickupTicketItemDetailsViewModel)this.BindingContext).SelectedDetail.PuDtlTank = tank;
                else
                    return false;
            }
            return true;
        }
    }
}