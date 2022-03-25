using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XamarinForms.Editors;
using FocalPoint.Modules.Dispatching.ViewModels;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Dispatching.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickupDetailMobileSelect : ContentPage
    {
        readonly PickupTicketViewModel viewModel;
        public PickupDetailMobileSelect(PickupTicket puTicket)
        {
            viewModel = new PickupTicketViewModel(puTicket);
            this.BindingContext = viewModel;

            InitializeComponent();

            this.Title = "Pickup Ticket - " + viewModel.Ticket.PuTNo.ToString();
        }

        public TaskCompletionSource<bool> Success { get; set; }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                viewModel.Indicator = true;
                List<PickupTicketOrder> orders = null;
                orders = await viewModel.PickupTicketOrder(viewModel.Ticket.PuTNo);

                if (orders != null)
                {
                    viewModel.Orders.Clear();
                    foreach (var order in orders)
                        viewModel.Orders.Add(order);
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("FocalPoint", "Failed to retrieve Pickup Ticket Orders", "Ok");
            }
            finally
            {
                viewModel.Indicator = false;
            }
        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            var vm = BindingContext as PickupTicketViewModel;
            try
            {
                foreach (var order in vm.Orders)
                {
                    if (order.PuDtlQty < 0)
                    {
                        await DisplayAlert("FocalPoint", "Pickup quantity cannot be less than 0.", "Ok");
                        return;
                    }

                    if (order.PuDtlQty > order.OrderDtlQty)
                    {
                        await DisplayAlert("FocalPoint", "Pickup quantity cannot be more than 'Out' quantity.", "Ok");
                        return;
                    }
                }
                var selectedOrders = vm.Orders.Where(x => x.PuDtlQty > 0);
                if(selectedOrders == null || !selectedOrders.Any())
                {
                    await DisplayAlert("FocalPoint", "No Orders selected.", "OK");
                    return;
                }
                var success = await vm.PickupTicketCreate(selectedOrders.ToList());
                if (!success)
                    await DisplayAlert("FocalPoint", "Failed to create the Pickup Tickets.", "Ok");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("FocalPoint", "Failed to send all counts.", "Ok");
            }
        }

        async private void Text_Unfocused(object sender, FocusEventArgs e)
        {
            var entry = (TextEdit)sender;
            var order = entry.BindingContext as PickupTicketOrder;

            if (order.PuDtlQty < 0)
            {
                await DisplayAlert("FocalPoint", "Quantity cannot be negative.", "Ok");
                entry.Focus();

            }
            else if (order.PuDtlQty > order.OrderDtlQty)
            {
                await DisplayAlert("FocalPoint", "Quantity cannot be more than Out Quantity.", "Ok");
                entry.Focus();
            }
        }
    }
}