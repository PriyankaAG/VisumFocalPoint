using FocalPoint.Modules.FrontCounter.ViewModels.NewRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views.NewRentals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditDetailOfSelectedItemView : ContentPage
    {
        EditDetailOfSelectedItemViewModel _viewModel;
        OrderDtlUpdate CurrentOrderDtlUpdate;
        public string OrderDetailType { get; set; }
        public EditDetailOfSelectedItemView(OrderDtl ordDetails, Order currentOrder)
        {
            InitializeComponent();
            string typeString = "";
            OrderDetailType = ordDetails?.OrderDtlType;
            switch (ordDetails?.OrderDtlType)
            {
                case "R":
                    typeString = "Rental";
                    break;
                case "M":
                    typeString = "Merchandise";
                    break;
                case "E":
                    typeString = "Exchange";
                    break;
            }

            Title = $"Edit {typeString}";
            CurrentOrderDtlUpdate = new OrderDtlUpdate() { Detail = ordDetails };
            _viewModel = new EditDetailOfSelectedItemViewModel(CurrentOrderDtlUpdate, currentOrder, new Command(() => StartUpdate()));
            _viewModel.IsPageLoading = true;
            this.BindingContext = _viewModel;
            Task.Delay(1000).ContinueWith((a) =>
            {
                _viewModel.IsPageLoading = false;
            });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            if (!await isDataValid())
            {
                return;
            }

            Tuple<Order, OrderDtl> retVal = new Tuple<Order, OrderDtl>(_viewModel.CurrentOrder, _viewModel.OrderDetails);
            MessagingCenter.Send(this, "OrderDetailUpdated", retVal);
            this.Navigation.PopAsync();
        }
        public async Task<bool> isDataValid()
        {
            if (OrderDetailType != "M" && qtyEntry.EditorText.Contains('.'))
            {
                await DisplayAlert("Invalid", "Quantity can not contain decimal.", "Ok");
                return false;
            }
            if (qtyEntry.EditorText == "0" || qtyEntry.EditorText == "" || _viewModel.OrderDetails.OrderDtlQty == 0)
            {
                await DisplayAlert("Invalid", "Quantity can not be Zero.", "Ok");
                return false;
            }
            else if (overridePriceEntry.Text == "0" || overridePriceEntry.Text == "" || _viewModel.OrderDetails.OrderDtlAmt == 0)
            {
                await DisplayAlert("Invalid", "Price can not be Zero.", "Ok");
                return false;
            }

            return true;
        }
        private async void StartUpdate()
        {
            if (_viewModel.IsPageLoading) return;

            if (!await isDataValid())
            {
                return;
            }

            _viewModel.Indicator = true;

            OrderUpdate orderUpdateResponse = await _viewModel.UpdateOrderDetail();
            AfterUpdate_OrderProcessing(orderUpdateResponse);
        }
        public async void AfterUpdate_OrderProcessing(OrderUpdate orderRefresh)
        {
            try
            {
                if (orderRefresh == null) return;
                if (orderRefresh.Answers != null && orderRefresh.Answers.Count > 0)
                {
                    while (orderRefresh.Answers != null && orderRefresh.Answers.Count > 0)
                    {
                        //display answer if key does not have a value update that value
                        //checkUpdate.Answers.
                        if (orderRefresh.Answers.Count > 0)
                        {
                            // KIRK REM KeyValuePair<int, string> question = new KeyValuePair<int, string>();
                            QuestionAnswer question = null;
                            foreach (var answer in orderRefresh.Answers)
                            {
                                //0 or 1 is not found
                                if (!(answer.Answer == "True" || answer.Answer == "False"))
                                {
                                    question = answer;
                                    break;
                                }
                            }
                            bool custOk = await DisplayAlert("Customer Options", question.Answer, "Yes", "No");
                            orderRefresh.Answers.Find(qa => qa.Code == question.Code).Answer = custOk.ToString();

                            var ordUpdate = _viewModel.OrderDetailUpdate;
                            //if (ordUpdate == null)
                            //    ordUpdate = orderRefresh;
                            //else
                            {
                                var ord = orderRefresh.Answers.Find(qa => qa.Code == question.Code);
                                ordUpdate.Answers.Add(ord);
                            }

                            ordUpdate.Detail = CurrentOrderDtlUpdate?.Detail;
                            _viewModel.OrderDetailUpdate = ordUpdate;
                            orderRefresh = await _viewModel.UpdateOrderDetail();
                        }

                    }

                }
            }
            catch (Exception)
            {
                _viewModel.Indicator = false;
            }
        }

        private void taxableheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            StartUpdate();
        }

        private void CustomEntry_Unfocused(object sender, FocusEventArgs e)
        {
            StartUpdate();
        }
    }
}