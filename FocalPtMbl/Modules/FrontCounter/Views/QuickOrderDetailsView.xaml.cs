using DevExpress.XamarinForms.CollectionView;
using DevExpress.XamarinForms.Editors;
using Visum.Services.Mobile.Entities;
using FocalPoint.Modules.FrontCounter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuickOrderDetailsView : ContentPage
    {
        private bool inNavigation;
        private bool hasNewItemsToAdd;
        public bool HasNewItemsToAdd
        {
            get { return hasNewItemsToAdd; }
            set
            {
                if (hasNewItemsToAdd != value)
                {
                    hasNewItemsToAdd = value;
                }
            }
        }

        public Order CurrentOrder { get; private set; }

        private List<OrderDtl> orderItems;
        private int selectedComboItem;

        public List<OrderDtl> OrderItems
        {
            get { return orderItems; }
            set
            {
                if (orderItems != value)
                {
                    orderItems = value;
                }
            }
        }
        public QuickOrderDetailsView(Order curOrder)
        {
            InitializeComponent();
            orderItems = new List<OrderDtl>();
            HasNewItemsToAdd = false;
            CurrentOrder = curOrder;
            this.Title = "Select Rental";
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //CustomerAdd = new CustomerLupView();
            this.inNavigation = false;
            HasNewItemsToAdd = false;
            //Reset All fields

            OrderUpdate orderUpdate;
            //Code from question, True/False will change
            //orderUpdate.Answers.Add("","")

            //((QuickOrderDetailsViewModel)this.BindingContext).GetCustomersInfo();
            //((CustomerSimpleViewModel)this.BindingContext).GetCustomerInfo();
        }

        public async void ItemSelected(object sender, CollectionViewGestureEventArgs args)
        {
            //if (args.Item != null)
              //  await OpenDetailPage(GetCustInfo(args.Item));
        }
        private AvailabilityRent GetRentInfo(object item)
        {
            if (item is AvailabilityRent rentInfo)
                return rentInfo;
            return new AvailabilityRent();
        }
        Task OpenDetailPage(Customer cust)
        {
            if (cust == null)
                return Task.CompletedTask;

            if (this.inNavigation)
                return Task.CompletedTask;

            this.inNavigation = true;
            return Navigation.PopModalAsync();
        }
        private void TextEdit_Completed(object sender, EventArgs e)
        {
            ((QuickOrderDetailsViewModel)this.BindingContext).GetSearchedCustomersInfo((sender as TextEdit).Text);
        }
        private void TextEdit_Cleared(object sender, EventArgs e)
        {
            ((QuickOrderDetailsViewModel)this.BindingContext).GetSearchedCustomersInfo("");
        }
        private async void SimpleButton_Clicked(object sender, EventArgs e)
        {
            // get order info
            await Navigation.PopAsync();
        }

        private void ComboBoxEdit_SelectionChanged(object sender, EventArgs e)
        {
            var combo = sender as ComboBoxEdit;
            selectedComboItem = combo.SelectedIndex+1;
        }
        private AvailabilityRent selItem;

        private async void AddToOrder_Clicked(object sender, EventArgs e)
        {
            List<string> selectedSerials = new List<string>();
            string result = "0";
            //List<string> serialNumbers= ((QuickOrderDetailsMerchViewModel)this.BindingContext).GetSerials(selItem);
            //Select the serial Numbers

            //if serials exist popup a selection dialog select multi? 
            if (selItem != null)
            {
                //Check Serialized on selcted item
                if (selItem.AvailSerialized)
                {
                    await FinishQuestions(1);
                    //get the serial numbers
                    //await Navigation.PushAsync(new SelectSerialOnlyView(selItem));
                }
                else
                {
                    result = await DisplayPromptAsync("Pick Quantity", "Enter in the Quantity", keyboard: Keyboard.Numeric);
                    if (result != "cancel")
                     await FinishQuestions(int.Parse(result));
                }
            }
            else
                await DisplayAlert("Select Item", "Please Search and select an Item.", "ok");
        }

        private async Task FinishQuestions(int count)
        {
            OrderUpdate UpdatedOrder = null;
            QuestionFaultExceptiom questionFault = null;
            Dictionary<int, string> currentAnswers = new Dictionary<int, string>();
            do
            {
                UpdatedOrder = ((QuickOrderDetailsViewModel)this.BindingContext).AddItem(selItem, count, CurrentOrder, UpdatedOrder, out questionFault);
                if (questionFault != null)
                {
                    switch (questionFault.Code) {

                        case 1000:
                            {
                                var action = await DisplayAlert("Question", questionFault.Message, "ok", "cancel");
                                if (!action)
                                    UpdatedOrder = null;
                                else
                                {
                                    currentAnswers[questionFault.Code] = action.ToString();
                                    UpdatedOrder.Answers= currentAnswers.Select(qa => new QuestionAnswer(qa.Key, qa.Value)).ToList();
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
                                else{ 

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
                else if (questionFault == null)
                {
                    MessagingCenter.Send<QuickOrderDetailsView, OrderUpdate>(this, "Hi", UpdatedOrder);
                    await Navigation.PopAsync();
                }
                else if (UpdatedOrder == null)
                {
                    await DisplayAlert("Item not added", "Item not added", "ok");
                }

            } while (UpdatedOrder != null && questionFault != null);
        }


        private void grid_DoubleTap(object sender, DevExpress.XamarinForms.DataGrid.DataGridGestureEventArgs e)
        {
            AddToOrder_Clicked(sender, e);
        }

        private void grid_SelectionChanged(object sender, DevExpress.XamarinForms.DataGrid.SelectionChangedEventArgs e)
        {
            if (e.Item != null)
                if (selItem == null)
                {
                    selItem = new AvailabilityRent();
                }
            selItem = GetRentInfo(e.Item);
        }
    }
}