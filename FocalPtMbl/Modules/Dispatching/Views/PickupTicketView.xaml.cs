﻿using DevExpress.XamarinForms.CollectionView;
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
        public PickupTicketView(PickupTicket pickupTicket)
        {
            //check to see if visable see details
            this.viewModel = new PickupTicketViewModel(pickupTicket);
            BindingContext = this.viewModel;
            Title = "Pickup Ticket # " + pickupTicket.PuTNo.ToString();
            InitializeComponent();
        }

        public void ItemSelected(object sender, CollectionViewGestureEventArgs args)
        {
            if (sender is Image) return;
            if (args.Item != null)
            {
                ((PickupTicketViewModel)BindingContext).SelectedDetail = (PickupTicketItem)args.Item;
            }
        }
        private PickupTicketItem GetDetailInfo(object item)
        {
            if (item is PickupTicketItem pTicket)
                return pTicket;
            return new PickupTicketItem();
        }
        Task OpenDetailPage(PickupTicketItem ticket)
        {
            if (ticket == null)
                return Task.CompletedTask;

            //PickupTicket detailedTicket = ((PickupTicketsSelectViewModel)this.BindingContext).GetDetailedTicketInfo(ticket);
            //MessagingCenter.Send<SelectCustomerView, string>(this, "Hi", "John");
            //MessagingCenter.Send<SelectCustomerView, Customer>(this, "Hi", cust);
            //EventPass(((SelectCustomerViewModel)this.BindingContext).SelectedCustomer);

            return Task.CompletedTask;
            //return Navigation.PushAsync(new PickupTicketItemView(detailedTicket));
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

            var success = ((PickupTicketViewModel)BindingContext).PickupTicketCounted();
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

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {

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
            base.OnAppearing();
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

            MessagingCenter.Unsubscribe<PickupTicketItemDetailsViewModel, PickupTicketItem>(this, "ItemDetails");
            MessagingCenter.Subscribe<PickupTicketItemDetailsViewModel, PickupTicketItem>(this, "ItemDetails", (sender, details) =>
                {
                    viewModel.SelectedDetail = details;
                    viewModel.SelectedItemChecked(true);
                });
        }

        async protected void CheckBoxTapped(object sender, EventArgs args)
        {
            PickupTicketViewModel viewModel = BindingContext as PickupTicketViewModel;
            bool isChecked;
            /*if (viewModel.SelectedDetail.ImageName != "UnCheckedBox.png")
isChecked = true;*/

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
                bool update = await ((PickupTicketViewModel)BindingContext).PickupTicketItemCount();
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

            //Get Itemized Sheet for selected Item
            ((PickupTicketViewModel)this.BindingContext).SelectedItemEdit();

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