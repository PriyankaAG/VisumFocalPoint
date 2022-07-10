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
        public EditDetailOfSelectedItemView(OrderDtl ordDetails, Order currentOrder)
        {
            InitializeComponent();
            string typeString = "";
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
            MessagingCenter.Send(this, "OrderDetailUpdated", CurrentOrderDtlUpdate.Detail);
            this.Navigation.PopAsync();
        }
        private async void StartUpdate()
        {
            if (_viewModel.IsPageLoading) return;

            //_viewModel.OrderDetailUpdate.Detail.OrderDtlAmt

            OrderDtlUpdate orderUpdateResponse = await _viewModel.UpdateOrderDetail();
            AfterUpdate_OrderProcessing(orderUpdateResponse);
        }
        public async void AfterUpdate_OrderProcessing(OrderDtlUpdate orderRefresh)
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
                            if (ordUpdate == null)
                                ordUpdate = orderRefresh;
                            else
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