﻿using FocalPoint.Modules.FrontCounter.ViewModels.NewRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views.NewRentals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddDetailMerchView : ContentPage
    {
        public AddDetailMerchView()
        {
            InitializeComponent();
        }

        private AvailabilityMerch selItem;

        private Order currentOrder = new Order();
        public Order CurrentOrder
        {
            get { return currentOrder; }
            set
            {
                if (currentOrder != value)
                {
                    currentOrder = value;
                }
            }
        }

        private void AddToOrder_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                OrderUpdate UpdatedOrder = null;
                QuestionFaultExceptiom questionFault = null;
                Dictionary<int, string> currentAnswers = new Dictionary<int, string>();
                List<string> selectedSerials = new List<string>();
                string result = "0";

                if (selItem != null)
                {
                    //Check Serialized on selected item
                    if (!selItem.AvailSerialized)
                    {
                        result = await DisplayPromptAsync("Pick Quantity", "Enter in the Quantity", keyboard: Keyboard.Numeric);
                    }
                    else
                    {
                        result = "1";
                    }

                    if (result != null && Convert.ToDecimal(result) > 0)
                    {
                        do
                        {
                            decimal numberOfItems = Convert.ToDecimal(result);
                            Tuple<OrderUpdate, QuestionFaultExceptiom> addRentalAPIResult = await ((AddDetailMerchViewModel)this.BindingContext).AddItem(selItem, numberOfItems, selectedSerials, questionFault);

                            if (addRentalAPIResult != null)
                            {
                                UpdatedOrder = addRentalAPIResult.Item1;
                                questionFault = addRentalAPIResult.Item2;
                            }
                            if (questionFault != null)
                            {
                                if (questionFault.Message == "Do you want to Select Serial Numbers Now?")
                                {
                                    var action = await DisplayAlert("Question", questionFault.Message, "ok", "cancel");
                                    if (!action)
                                        UpdatedOrder = null;
                                    else
                                    {
                                        currentAnswers[questionFault.Code] = action.ToString();
                                        UpdatedOrder.Answers = currentAnswers.Select(qa => new QuestionAnswer(qa.Key, qa.Value)).ToList();
                                    }
                                }
                                else
                                {
                                    var selectSerialOnlyPage = new SelectSerialOnlyView(selItem);
                                    await this.Navigation.PushModalAsync(selectSerialOnlyPage);
                                    selectedSerials = await selectSerialOnlyPage.Result.Task;
                                    currentAnswers[questionFault.Code] = true.ToString();
                                    UpdatedOrder.Answers = currentAnswers.Select(qa => new QuestionAnswer(qa.Key, qa.Value)).ToList();
                                }

                            }
                            else if (questionFault == null)
                            {
                                MessagingCenter.Send<AddDetailMerchView, OrderUpdate>(this, "UpdateOrder", UpdatedOrder);
                                await Navigation.PopAsync();
                            }
                            else if (UpdatedOrder == null)
                            {
                                await DisplayAlert("Item not added", "Item not added", "ok");
                            }

                        } while (UpdatedOrder != null && questionFault != null);
                    }
                    else
                        await DisplayAlert("Select Item", "Please Search and select an Item.", "ok");
                }
            });
        }

        private void Search_Tapped(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await ((AddDetailMerchViewModel)this.BindingContext).GetSearchedCustomersInfo(SearchTextEditor.Text);
                }
                catch (Exception ex)
                {

                }
            });
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Any())
            {
                var selecteItem = e.CurrentSelection.First() as AvailabilityMerch;
                selItem = selecteItem;
            }
        }
    }
}