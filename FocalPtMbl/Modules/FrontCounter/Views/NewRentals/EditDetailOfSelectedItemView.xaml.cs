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
            OrderDtlUpdate ordUpdateDtl = new OrderDtlUpdate() { Detail = ordDetails };
            _viewModel = new EditDetailOfSelectedItemViewModel(ordUpdateDtl, currentOrder, new Command(() => StartUpdate()));
            this.BindingContext = _viewModel;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            StartUpdate();
        }
        private async void StartUpdate()
        {
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
                            bool custOk = await DisplayAlert("Customer Options", question.Answer, "OK", "Cancel");
                            orderRefresh.Answers.Find(qa => qa.Code == question.Code).Answer = custOk.ToString();
                            // KIRK REM orderRefresh.Answers[question.Key] = custOk.ToString();
                            _viewModel.OrderDetailUpdate = orderRefresh;
                            orderRefresh = await _viewModel.UpdateOrderDetail();
                        }

                    }

                }
            }
            catch (Exception)
            {
            }
        }
    }
}