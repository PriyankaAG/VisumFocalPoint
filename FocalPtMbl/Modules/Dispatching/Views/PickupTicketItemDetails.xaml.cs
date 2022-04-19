using System;
using System.Threading.Tasks;
using DevExpress.XamarinForms.Editors;
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

        private async void Submit_Clicked(object sender, EventArgs e)
        {

            var vm = (PickupTicketItemDetailsViewModel)this.BindingContext;
            vm.SelectedDetail.UTCCountDte = DateTime.UtcNow;
            var countedResult = vm.IsCountedGreaterThanToBeCounted();
            switch (countedResult)
            {
                case true:
                    await DisplayAlert("FocalPoint", "Line is over counted, please adjust your counts", "Ok");
                    return;
                case false:
                    var isConfirm = await DisplayAlert("FocalPoint", "Line is under counted, do  you want to mark the remaining still out?", "Yes", "No");
                    if (isConfirm)
                    {
                        vm.StillOut = vm.ToBePickedUp - vm.Totals;
                    }
                    break;
                case null:
                    break;
            }
            try
            {
                viewModel.Indicator = true;
                var isSuccess = await viewModel.PickupTicketItemCount();
                if (isSuccess)
                {
                    await Navigation.PopAsync();
                    viewModel.UpdateTicket(true);
                }
                else
                {
                    await DisplayAlert("FocalPoint", "There was an error updating the Pickup Ticket Item.", "Ok");
                    return;
                }
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

        private void Text_Unfocused(object sender, FocusEventArgs e)
        {
            var entry = (TextEdit)sender;
            if (string.IsNullOrEmpty(entry.Text))
            {
                entry.Text = "0";
            }
        }
    }
}