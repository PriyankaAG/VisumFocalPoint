using DevExpress.XamarinForms.CollectionView;
using FocalPoint.Modules.Dispatching.ViewModels;
using FocalPoint.Modules.FrontCounter.ViewModels;
using FocalPoint.Modules.FrontCounter.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Dispatching.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickupTicketView : ContentPage
    {
        bool _continue = true;
        public PickupTicketView(PickupTicket pickupTicket)
        {
            //check to see if visable see details
            this.viewModel = new PickupTicketViewModel(pickupTicket);
            BindingContext = this.viewModel;
            Title = "Pickup Ticket # " + pickupTicket.PuTNo.ToString();
            InitializeComponent();
        }

        private void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                ((PickupTicketViewModel)BindingContext).SelectedDetail = (PickupTicketItem)e.SelectedItem;
            }
        }
        readonly PickupTicketViewModel viewModel;

        private async void CompleteCountButton_Clicked(object sender, EventArgs e)
        {
            if (((PickupTicketViewModel)this.BindingContext).ToBeCounted > 0)
            {
                await DisplayAlert("FocalPoint Mobile", "Please Count All Detail Lines.", "OK");
                return;
            }
            var underCount = false;
            foreach (var itemVm in ((PickupTicketViewModel)BindingContext).Details)
            {
                if (((PickupTicketViewModel)BindingContext).Totals > itemVm.PuDtlQty)
                {
                    await DisplayAlert("FocalPoint", "Items Over Counted, Please check details for Errors.", "OK");
                    return;
                }

                if (((PickupTicketViewModel)BindingContext).Totals > 0 && ((PickupTicketViewModel)BindingContext).Totals < itemVm.PuDtlQty)
                    underCount = true;
            }
            if (underCount)
            {
                string proceed = await DisplayPromptAsync("FocalPoint", "Some Items are under Counted, Are you sure you want to Complete?", "Yes", "No");

                if (proceed == "No")
                    return;
            }
            //App.Platform.Show("Sending Counts...");

            var success = await ((PickupTicketViewModel)BindingContext).PickupTicketCounted();
            if (success)
            {
                await ShowSignatureScreen();
            }

        }

        private async Task ShowSignatureScreen()
        {
            Order order = null;
            OrderSignatureViewModel signatureViewModel = new OrderSignatureViewModel(order, false, "Sign below to accept Pickup");
            var orderSignatureView = new OrderSignatureView
            {
                BindingContext = signatureViewModel
            };
            await Navigation.PushAsync(orderSignatureView);
        }

        private void TapGestureRecognizer_Tapped_4(object sender, EventArgs e)
        {
            bool isChecked = false;
            if (((PickupTicketViewModel)this.BindingContext).SelectedDetail.ImageName != "UnCheckedBox.png")
                isChecked = true;
            ((PickupTicketViewModel)this.BindingContext).SelectedItemChecked(isChecked);
        }

        async protected override void OnAppearing()
        {
            var viewModel = ((PickupTicketViewModel)BindingContext);
            base.OnAppearing();
            bool locked = await viewModel.AttemptLock(true.ToString());
            if (locked == false)
            {
                await DisplayAlert("FocalPoint", "Pickup Ticket Locked by Store", "OK");
                await Navigation.PopAsync();
                return;
            }
            CheckForLockPeriodically();

            if (viewModel.Details.Count == 0)
            {
                if (viewModel.PuMobile)
                {
                    await DisplayAlert("FocalPoint", "Mobile Defined Pickup Ticket, Please Add Details to be Counted from Order.", "OK");
                }
                else
                {
                    await DisplayAlert("FocalPoint", "No details on this Pickup Ticket.", "OK");
                    await Navigation.PopAsync();
                    return;
                }
            }
            //await RefreshTicket();
            //Device.StartTimer(TimeSpan.FromSeconds(30), () =>
            //{
            //    Task.Run(async () => await RefreshTicket());
            //    return true;
            //});
            MessagingCenter.Unsubscribe<OrderSignatureViewModel, string>(this, "Signature");
            MessagingCenter.Subscribe<OrderSignatureViewModel, string>(this, "Signature", async (sender, capturedImage) =>
            {
                PickupTicketViewModel viewOrderDetailsViewModel = (PickupTicketViewModel)BindingContext;
                viewOrderDetailsViewModel.SignatureImage = capturedImage;
                bool success = await viewOrderDetailsViewModel.SaveSignature();
                if (success)
                {
                    await DisplayAlert("FocalPoint Mobile", "Signature added successfully", "OK");
                }
                else
                {
                    await DisplayAlert("FocalPoint Mobile", "Failed to add Signature", "OK");
                }
                await Navigation.PopAsync();
                await Navigation.PopAsync();
            });

            MessagingCenter.Unsubscribe<PickupTicketItemDetailsViewModel, Tuple<PickupTicketItem, bool>>(this, "ItemDetails");
            MessagingCenter.Subscribe<PickupTicketItemDetailsViewModel, Tuple<PickupTicketItem, bool>>(this, "ItemDetails", async (sender, details) =>
            {
                viewModel.SelectedDetail = details.Item1;
                if (details.Item2)
                {
                    await CheckPopupValues();
                    viewModel.SelectedItemChecked(true, true);
                    await viewModel.UpdateItem();
                }
                else
                    viewModel.SelectedItemChecked(false, true);
            });
        }

        private void CheckForLockPeriodically()
        {
            Device.StartTimer(TimeSpan.FromSeconds(120), () =>
            {
                if (!_continue)
                    return false;
                try
                {
                    Task.Run(async () => await viewModel.AttemptLock(false.ToString()));
                }
                catch (Exception ex)
                {
                    //Do nothing
                }
                return _continue;
            });
        }

        async protected override void OnDisappearing()
        {
            _continue = false;
            base.OnDisappearing();
            bool locked = await viewModel.AttemptLock(false.ToString());
            if (locked == false)
            {
                await DisplayAlert("FocalPoint", "Pickup Ticket Locked by Store", "OK");
                return;
            }
        }

        async protected void CheckBoxTapped(object sender, EventArgs args)
        {
            var imageSender = (Image)sender;
            var parent = (Grid)imageSender.Parent;
            ((PickupTicketViewModel)BindingContext).SelectedDetail = (PickupTicketItem)parent.BindingContext;

            PickupTicketViewModel viewModel = BindingContext as PickupTicketViewModel;
            bool isChecked;
            viewModel.SelectedDetail.Checked = isChecked = !viewModel.SelectedDetail.Checked;

            if (isChecked)
            {
                var isSuccess = await CheckPopupValues();
                if (!isSuccess)
                    return;
            }
            viewModel.SelectedItemChecked(isChecked);
            try
            {
                viewModel.Indicator = true;
                bool update = await viewModel.PickupTicketItemCount(viewModel.SelectedDetail);
                await viewModel.UpdateItem();
                if (!update)
                {
                    await DisplayAlert("FocalPoint", isChecked ? "Item Counted by Another, Counts Reloaded." :
                        "Item Counted by Another, Last Counts Reloaded.", "OK");
                    //await RefreshTicket();
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
        private void SimpleButton_Clicked(object sender, EventArgs e)
        {

        }

        private async void DetailLine_Tapped(object sender, EventArgs e)
        {
            var parent = (Grid)sender;
            ((PickupTicketViewModel)BindingContext).SelectedDetail = (PickupTicketItem)parent.BindingContext;
            await Navigation.PushAsync(new PickupTicketItemDetails(((PickupTicketViewModel)this.BindingContext).SelectedDetail));
        }

        private async Task RefreshTicket()
        {
            var PickupTicketEntityComponent = new PickupTicketEntityComponent();
            var detailedTicket = await PickupTicketEntityComponent.GetPickupTicket(viewModel.SelectedDetail.PuTNo.ToString());

            viewModel.Init(detailedTicket);
        }

        private async void MobilePickupClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PickupDetailMobileSelect(((PickupTicketViewModel)this.BindingContext).Ticket));
        }
    }
}