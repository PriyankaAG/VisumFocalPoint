using System;
using FocalPoint.Modules.Dispatching.ViewModels;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FocalPoint.Modules.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FocalPoint.Modules.Dispatching.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickupTicketPage : TabbedPage
    {
        readonly PickupTicketViewModel viewModel;
        bool _continue = true;

        public PickupTicketPage(PickupTicket pickupTicket)
        {
            //check to see if visable see details
            this.viewModel = new PickupTicketViewModel(pickupTicket);
            BindingContext = viewModel;

            foreach (var tabPage in Children)
                tabPage.BindingContext = viewModel;

            Title = "Pickup Ticket # " + pickupTicket.PuTNo.ToString();
            InitializeComponent();
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                _continue = true;
                var viewModel = ((PickupTicketViewModel)BindingContext);
                bool locked = await viewModel.AttemptLock(true.ToString());
                if (locked == false)
                {
                    await DisplayAlert("FocalPoint", "Pickup Ticket Locked by Store", "OK");
                    await Navigation.PopAsync();
                    return;
                }
                CheckForLockPeriodically();

                var ticket = await viewModel.GetTicketInfo(viewModel.Ticket.PuTNo.ToString());
                if (ticket == null) return;

                viewModel.Ticket = ticket;
                var orders = await viewModel.PickupTicketOrder(viewModel.Ticket.PuTNo);
                foreach (var order in orders)
                {
                    viewModel.Orders.Add(order);
                }
                foreach (var tabPage in this.Children)
                    tabPage.BindingContext = viewModel;

                if (viewModel.Details.Count == 0)
                {
                    if (viewModel.Ticket.PuMobile)
                    {
                        await DisplayAlert("FocalPoint", "Mobile Defined Pickup Ticket, Please Add Details to be Counted from Order.", "OK");
                        //TODO: Go to Details Tab
                    }
                    else
                    {
                        await DisplayAlert("FocalPoint", "No details on this Pickup Ticket.", "OK");
                        await Navigation.PopAsync();
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                var msg = e.Message;
                await DisplayAlert("FocalPoint", "Failed to retrieve Pickup Ticket Information.", "OK");
            }
        }

        async protected override void OnDisappearing()
        {
            _continue = false;
            base.OnDisappearing();
            try
            {
                await viewModel.AttemptLock(false.ToString());
            }
            catch { }
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
    }
}