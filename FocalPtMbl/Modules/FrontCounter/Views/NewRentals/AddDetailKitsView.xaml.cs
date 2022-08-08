using FocalPoint.Modules.FrontCounter.ViewModels.NewRental;
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
    public partial class AddDetailKitsView : ContentPage
    {
        public AddDetailKitsView()
        {
            InitializeComponent();
        }

        private AvailabilityKit selItem;
        private Int16 SearchIn = 1;

        private void AddToOrder_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    string result = "0";
                    if (selItem != null)
                    {
                        result = await DisplayPromptAsync("Pick Quantity", "Enter in the Quantity", keyboard: Keyboard.Numeric);
                        if (result != "cancel")
                        {
                            if (selItem.AvailType == "M")
                                await MerchFinishQuestions(int.Parse(result));
                            else
                                await RentalFinishQuestions(int.Parse(result));
                        }
                    }
                    else
                        await DisplayAlert("Select Item", "Please Search and select an Item.", "ok");
                }
                catch (Exception ex)
                {
                    //TODO: log error
                }
            });
        }

        private async Task RentalFinishQuestions(int count)
        {
            OrderUpdate UpdatedOrder = null;
            QuestionFaultExceptiom questionFault = null;
            Dictionary<int, string> currentAnswers = new Dictionary<int, string>();
            string errorMessage = string.Empty;
            AddDetailKitsViewModel AddDetailKitsViewModel = (AddDetailKitsViewModel)this.BindingContext;
            do
            {
                Tuple<OrderUpdate, QuestionFaultExceptiom, string> addRentalAPIResult = await AddDetailKitsViewModel.RentalAddItem(selItem, count, AddDetailKitsViewModel.CurrentOrder, UpdatedOrder, questionFault);
                if (addRentalAPIResult != null)
                {
                    UpdatedOrder = addRentalAPIResult.Item1;
                    questionFault = addRentalAPIResult.Item2;
                    errorMessage = addRentalAPIResult.Item3;
                }
                if (questionFault != null)
                {
                    switch (questionFault.Code)
                    {
                        case 1000:
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
                            break;
                        case 1001:
                            {
                                // if(UpdatedOrder)
                                var action = await DisplayAlert("Question", questionFault.Message, "ok", "cancel");
                                if (!action)
                                    UpdatedOrder = null;
                                else
                                {
                                    currentAnswers[questionFault.Code] = action.ToString();
                                    UpdatedOrder.Answers = currentAnswers.Select(qa => new QuestionAnswer(qa.Key, qa.Value)).ToList();
                                }
                            }
                            break;
                        case 1002:
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
                            break;
                        case 1003:
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
                            break;
                        case 1004:
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
                            break;
                        case 1005:
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
                            break;
                        case 1006:
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
                            break;
                        case 1007:
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
                            break;
                        case 1008:
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
                            break;
                        case 1009:
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
                            break;
                        case 1010:
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
                            break;
                        case 1011:
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
                            break;
                        case 1012:
                            {
                                //Get Meter Value in Double 3 decimal places
                                string action = await DisplayPromptAsync("Enter Meter", questionFault.Message, initialValue: "1", keyboard: Keyboard.Numeric);
                                if (action == null && action == "")
                                    UpdatedOrder = null;
                                else
                                {
                                    currentAnswers[questionFault.Code] = action.ToString();
                                    UpdatedOrder.Answers = currentAnswers.Select(qa => new QuestionAnswer(qa.Key, qa.Value)).ToList();
                                }
                            }
                            break;
                        case 1013:
                            {
                                //See Rentals.frmRateTablePackage, display rate and use selected package as short
                                var action = await DisplayAlert("Question", questionFault.Message, "ok", "cancel");
                                if (!action)
                                    UpdatedOrder = null;
                                else
                                {
                                    currentAnswers[questionFault.Code] = action.ToString();
                                    UpdatedOrder.Answers = currentAnswers.Select(qa => new QuestionAnswer(qa.Key, qa.Value)).ToList();
                                }
                            }
                            break;
                        case 1014:
                            {
                                //Get Job Number String
                                var action = await DisplayAlert("Question", questionFault.Message, "ok", "cancel");
                                if (!action)
                                    UpdatedOrder = null;
                                else
                                {
                                    currentAnswers[questionFault.Code] = action.ToString();
                                    UpdatedOrder.Answers = currentAnswers.Select(qa => new QuestionAnswer(qa.Key, qa.Value)).ToList();
                                }
                            }
                            break;
                        case 1015:
                            {
                                //Get PO Number String
                                var action = await DisplayAlert("Question", questionFault.Message, "ok", "cancel");
                                if (!action)
                                    UpdatedOrder = null;
                                else
                                {
                                    currentAnswers[questionFault.Code] = action.ToString();
                                    UpdatedOrder.Answers = currentAnswers.Select(qa => new QuestionAnswer(qa.Key, qa.Value)).ToList();
                                }
                            }
                            break;
                        case 1016:
                            {
                                //  call GET SalesPeople and return selected value
                                var action = await DisplayAlert("Question", questionFault.Message, "ok", "cancel");
                                if (!action)
                                    UpdatedOrder = null;
                                else
                                {
                                    currentAnswers[questionFault.Code] = action.ToString();
                                    UpdatedOrder.Answers = currentAnswers.Select(qa => new QuestionAnswer(qa.Key, qa.Value)).ToList();
                                }
                            }
                            break;
                        case 1017:
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
                            break;
                        case 1018:
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
                            break;
                        case 1019:
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
                            break;

                        default:
                            break;
                    };
                }
                else if (!string.IsNullOrEmpty(errorMessage))
                {
                    await DisplayAlert("Error", errorMessage, "OK");
                }
                else if (questionFault == null)
                {
                    MessagingCenter.Send<AddDetailKitsView, OrderUpdate>(this, "UpdateOrder", UpdatedOrder);
                    await Navigation.PopAsync();
                }
                else if (UpdatedOrder == null)
                {
                    await DisplayAlert("Item not added", "Item not added", "OK");
                }
            } while (UpdatedOrder != null && questionFault != null);
        }
        private async Task MerchFinishQuestions(int count)
        {
            OrderUpdate UpdatedOrder = null;
            QuestionFaultExceptiom questionFault = null;
            Dictionary<int, string> currentAnswers = new Dictionary<int, string>();
            List<string> selectedSerials = new List<string>();
            string errorMessage = string.Empty;
            do
            {
                decimal numberOfItems = count;

                Tuple<OrderUpdate, QuestionFaultExceptiom, string> addRentalAPIResult = await ((AddDetailKitsViewModel)this.BindingContext).MerchAddItem(selItem, numberOfItems, selectedSerials, questionFault);

                if (addRentalAPIResult != null)
                {
                    UpdatedOrder = addRentalAPIResult.Item1;
                    questionFault = addRentalAPIResult.Item2;
                    errorMessage = addRentalAPIResult.Item3;
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
                        var selectSerialOnlyPage = new SelectSerialOnlyView(selItem.AvailCmp, selItem.AvailItem);
                        await this.Navigation.PushModalAsync(selectSerialOnlyPage);
                        selectedSerials = await selectSerialOnlyPage.Result.Task;
                        currentAnswers[questionFault.Code] = true.ToString();
                        UpdatedOrder.Answers = currentAnswers.Select(qa => new QuestionAnswer(qa.Key, qa.Value)).ToList();
                    }

                }
                else if (!string.IsNullOrEmpty(errorMessage))
                {
                    await DisplayAlert("Error", errorMessage, "OK");
                }
                else if (questionFault == null)
                {
                    MessagingCenter.Send<AddDetailKitsView, OrderUpdate>(this, "UpdateOrder", UpdatedOrder);
                    await Navigation.PopAsync();
                }
                else if (UpdatedOrder == null)
                {
                    await DisplayAlert("Item not added", "Item not added", "ok");
                }

            } while (UpdatedOrder != null && questionFault != null);
        }

        private async void Search_Tapped(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    if (SearchTextEditor.Text == null)
                    {
                        await DisplayAlert("Validation", "Please enter Search For.", "OK");
                        return;
                    }
                    await ((AddDetailKitsViewModel)this.BindingContext).GetSearchedInfo(SearchTextEditor.Text);
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
                var selecteItem = e.CurrentSelection.First() as AvailabilityKit;
                selItem = selecteItem;
            }
        }
    }
}