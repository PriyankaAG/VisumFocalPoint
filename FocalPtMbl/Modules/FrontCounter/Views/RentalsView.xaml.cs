using DevExpress.XamarinForms.CollectionView;
using DevExpress.XamarinForms.Editors;
using FocalPoint.Modules.FrontCounter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RentalsView : ContentPage
    {
        private bool inNavigation;

        public RentalsView()
        {
            InitializeComponent();
            BindingContext = new RentalsViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.inNavigation = false;
            Device.BeginInvokeOnMainThread(() =>
            {
                ((RentalsViewModel)this.BindingContext).GetCustomersInfo();
            });
            //((CustomerSimpleViewModel)this.BindingContext).GetCustomerInfo();
        }
        public async void ItemSelected(object sender, CollectionViewGestureEventArgs args)
        {
            if (args.Item != null)
                await OpenDetailPage(GetCustInfo(args.Item));
        }
        private Customer GetCustInfo(object item)
        {
            if (item is Customer custInfo)
                return custInfo;
            return new Customer();
        }
        Task OpenDetailPage(Customer cust)
        {
            if (cust == null)
                return Task.CompletedTask;

            if (this.inNavigation)
                return Task.CompletedTask;

            this.inNavigation = true;
            return Navigation.PopAsync();//PushAsync(new CustomerDetailView(cust));
        }
        private DateTime? startDte = new DateTime();
        public DateTime? StartDate
        {
            get { return startDte; }
            set
            {
                if (startDte != value)
                {
                    startDte = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }
        private DateTime? endDte = new DateTime();
        public DateTime? EndDate
        {
            get { return endDte; }
            set
            {
                if (endDte != value)
                {
                    endDte = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        private string textsearched = "";
        public string textSearched
        {
            get { return textsearched; }
            set
            {
                if (textsearched != value)
                {
                    textsearched = value;
                    OnPropertyChanged(nameof(textSearched));
                }
            }
        }


        private DateTime? startTime = new DateTime();
        public DateTime? StartTime
        {
            get { return startTime; }
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    OnPropertyChanged(nameof(StartTime));
                }
            }
        }
        private DateTime? endTime = new DateTime();
        public DateTime? EndTime
        {
            get { return endTime; }
            set
            {
                if (endTime != value)
                {
                    endTime = value;
                    OnPropertyChanged(nameof(EndTime));
                }
            }
        }




        private void DateEdit_DateChanged(object sender, EventArgs e)
        {
            //if list count is not 0 add date else popup cancel/cont change date back
            var startdateEdit = sender as DateEdit;
            StartDate = startdateEdit.Date;
            ((RentalsViewModel)this.BindingContext).StartDte = StartDate;
            ((RentalsViewModel)this.BindingContext).DateChanged(StartDate);
        }
        private void DateEdit_DateChanged_1(object sender, EventArgs e)
        {
            //if list count is not 0 add date else popup cancel/cont change date back
            var startdateEdit = sender as DateEdit;
            EndDate = startdateEdit.Date;
            ((RentalsViewModel)this.BindingContext).EndDte = EndDate;
            ((RentalsViewModel)this.BindingContext).DateChanged(StartDate);
        }
        private void TextEdit_Completed(object sender, EventArgs e)
        {
            var searchText = sender as TextEdit;
            textSearched = (string)searchText.Text;
        }
        private void TextEdit_Cleared(object sender, EventArgs e)
        {
            ((RentalsViewModel)this.BindingContext).SearchTextEdit = "";
        }

        private void TimeEdit_TimeChanged(object sender, EventArgs e)
        {
            var starttimeEdit = sender as TimeEdit;
            StartTime = starttimeEdit.Time;
            ((RentalsViewModel)this.BindingContext).StartTime = StartTime;
        }

        private void TimeEdit_TimeChanged_1(object sender, EventArgs e)
        {
            var endtimeEdit = sender as TimeEdit;
            EndTime = endtimeEdit.Time;
            ((RentalsViewModel)this.BindingContext).EndTime = EndTime;
        }

        private void ComboBoxEdit_SelectionChanged(object sender, EventArgs e)
        {
            var storeCombo = sender as ComboBoxEdit;
            ((RentalsViewModel)this.BindingContext).SelectedStore = (Company)storeCombo.SelectedItem;
        }

        private void ComboBoxEdit_SelectionChanged_1(object sender, EventArgs e)
        {
            var searchInCombo = sender as ComboBoxEdit;
            ((RentalsViewModel)this.BindingContext).SelectedSearchIn = (string)searchInCombo.SelectedItem;
        }

        private async void SimpleButton_Clicked(object sender, EventArgs e)
        {
            ((RentalsViewModel)this.BindingContext).SearchTextEdit = textSearched;
            if (textSearched != "")
            {
                var SearchedObsCollection = ((RentalsViewModel)this.BindingContext).SearchButtonClicked();
                //Push Results page
                if (SearchedObsCollection == null)
                    await DisplayAlert("Enter Values", "Please enter a value for all the fields", "OK");
                else if (SearchedObsCollection.Count > 0)
                    await Navigation.PushAsync(new RentalResultsView(SearchedObsCollection));
                else if (SearchedObsCollection.Count == 0)
                    await DisplayAlert("No Results Found", "No Results Found for: " + textSearched, "OK");
            }
            else
            {
                await DisplayAlert("Enter Value", "Please enter a value for Search", "OK");
            }
        }

        private void TextEdit_ClearIconClicked(object sender, System.ComponentModel.HandledEventArgs e)
        {
            ; textSearched = "";
            ((RentalsViewModel)this.BindingContext).SearchTextEdit = "";
        }
    }
}