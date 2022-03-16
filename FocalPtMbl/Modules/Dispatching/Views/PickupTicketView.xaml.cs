using DevExpress.XamarinForms.CollectionView;
using FocalPoint.Modules.Dispatching.ViewModels;
using FocalPoint.Modules.FrontCounter.ViewModels;
using FocalPoint.Modules.FrontCounter.Views;
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

        public async void ItemSelected(object sender, CollectionViewGestureEventArgs args)
        {
            if (args.Item != null)
            {
                ((PickupTicketViewModel)BindingContext).SelectedDetail = (PickupTicketItem)args.Item;
                await Navigation.PushAsync(new PickupTicketItemDetails(((PickupTicketViewModel)this.BindingContext).SelectedDetail));
                //Get Number of questions
                //List<string> popUpCount = ((PickupTicketViewModel)this.BindingContext).GetPopUpCount();
                //check popups?
                /*foreach (var popupString in popUpCount)
                {
                    double initValue = 0;
                    //bool stayOrLeave = await DisplayAlert("New customer # duplicate", "The New customer has the same phone number as one already in the database", "Cancel", "Continue");
                    if (popupString == "Input Meter")
                        initValue = ((PickupTicketViewModel)this.BindingContext).SelectedDetail.PuDtlMeterIn;
                    else if (popupString == "Select Count")
                        initValue = (double)((PickupTicketViewModel)this.BindingContext).SelectedDetail.LastCntOutQty;
                    else if (popupString == "Add Fuel")
                        initValue = (double)((PickupTicketViewModel)this.BindingContext).SelectedDetail.PuDtlTank;
                    string result = await DisplayPromptAsync("Change Pickup", popupString, initialValue: initValue.ToString(), keyboard: Keyboard.Numeric);
                    if (popupString == "Input Meter" && result != null)
                        ((PickupTicketViewModel)this.BindingContext).SelectedDetail.PuDtlMeterIn = Convert.ToDouble(result);
                    else if (popupString == "Select Count" && result != null)
                        ((PickupTicketViewModel)this.BindingContext).SelectedDetail.LastCntOutQty = Convert.ToDecimal(result);
                    else if (popupString == "Add Fuel" && result != null)
                        ((PickupTicketViewModel)this.BindingContext).SelectedDetail.PuDtlTank = Convert.ToDouble(result);

                    ((PickupTicketViewModel)this.BindingContext).SelectedDetail.CurrentTotalCnt = ((PickupTicketViewModel)this.BindingContext).Totals;
                    ((PickupTicketViewModel)this.BindingContext).SelectedDetail.ImageName = ((PickupTicketViewModel)this.BindingContext).GetImageString();
                    //((PickupTicketViewModel)this.BindingContext).PickupTicketItemToSubmit();
                }*/

                //await OpenDetailPage(GetDetailInfo(args.Item));
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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (((PickupTicketViewModel)this.BindingContext).ToBeCounted > 0)
            {
                await DisplayAlert("FocalPoint Mobile", "Please Count All Detail Lines.", "OK");
                return;
            }
            var underCount = false;
            foreach (var itemVm in viewModel.Details)
            {
                if (viewModel.Totals > itemVm.PuDtlQty)
                {
                    await DisplayAlert("FocalPoint", "Items Over Counted, Please check details for Errors.", "OK");
                    return;
                }

                if (viewModel.Totals > 0 && viewModel.Totals < itemVm.PuDtlQty)
                    underCount = true;
            }
            if (underCount)
            {
                string proceed = await DisplayPromptAsync("FocalPoint", "Some Items are under Counted, Are you sure you want to Complete?", "Yes", "No");

                if (proceed == "No")
                    return;
            }
            //App.Platform.Show("Sending Counts...");

            var success = viewModel.PickupTicketCounted(viewModel.PuTNo);
            if (success)
            {
                OrderSignatureViewModel orderSignatureViewModel = new OrderSignatureViewModel(null, false, "Sign below to accept Pickup");
                var orderSignatureView = new OrderSignatureView();
                orderSignatureView.BindingContext = orderSignatureViewModel;
                //navigation.PushAsync(orderSignatureView);
            }

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

        async protected void CheckBoxTapped(object sender, EventArgs args)
        {
            var img = (Image)sender;
            PickupTicketViewModel viewModel = BindingContext as PickupTicketViewModel;
            bool isChecked = false;
            if (viewModel.SelectedDetail.ImageName != "UnCheckedBox.png")
                isChecked = true;

            if (isChecked)
            {
                viewModel.SelectedDetail.PuDtlCntQty = viewModel.SelectedDetail.PuDtlQty;
                List<string> popUpCount = viewModel.GetPopUpCount();
                foreach (var popupString in popUpCount)
                {
                    double initValue = viewModel.GetPopupType(popupString);
                    string result = await DisplayPromptAsync("Change Pickup", popupString, initialValue: initValue.ToString(), keyboard: Keyboard.Numeric);
                    if (result != null)
                        viewModel.setPopupValue(popupString, result);
                }
            }
            else
                viewModel.ClearQuantities();
            viewModel.SelectedDetail.UTCCountDte = DateTime.UtcNow;
            bool update = viewModel.PickupTicketItemCount();
            if (!update)
            {
                if (isChecked)
                    await DisplayAlert("FocalPoint", "Item Counted by Another, Counts Reloaded.", "OK");
                else
                    await DisplayAlert("FocalPoint", "Item Counted by Another, Last Counts Reloaded.", "OK");
            }
        }

        private void SimpleButton_Clicked(object sender, EventArgs e)
        {

        }

        private async void TapGestureRecognizer_Tapped_5(object sender, EventArgs e)
        {
            //Get Itemized Sheet for selected Item
            ((PickupTicketViewModel)this.BindingContext).SelectedItemEdit();

            await Navigation.PushAsync(new PickupTicketItemDetails(((PickupTicketViewModel)this.BindingContext).SelectedDetail));
        }
    }
}