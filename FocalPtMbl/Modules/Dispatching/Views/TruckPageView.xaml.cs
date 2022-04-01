using FocalPoint.Modules.Dispatching.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Dispatching.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TruckPageView : ContentPage
    {
        TruckPageViewModel _vm = null;
        DateTime _old = DateTime.Now.Date;

        public TruckPageView(TruckPageViewModel vm)
        {
            _vm = vm;
            _vm.DispatchVm.SearchDate = DateTime.Now.Date;

            this.BindingContext = _vm;

            InitializeComponent();


            if (_vm.Truck == null)
            {
                this.Title = "All";
                listView.ItemsSource = vm.DispatchVm.AllDispatches;
            }
            else
            {
                this.Title = _vm.Truck.TruckID;
                listView.ItemsSource = _vm.Dispatches;
            }

            this.IconImageSource = "dot.png";
        }
        protected override void OnAppearing()
        {
            startDate.Focused += StartDate_Focused;
            startDate.Unfocused += StartDate_Unfocused;

            startDate.DateChanged -= DateEdit_DateChanged;
            startDate.DateChanged += DateEdit_DateChanged;

            startDate.Date = DateTime.Now.Date;
        }
        protected override void OnDisappearing()
        {
            startDate.DateChanged -= DateEdit_DateChanged;
        }

        private void StartDate_Focused(object sender, FocusEventArgs e)
        {
            _old = _vm.DispatchVm.SearchDate;
        }

        async private void StartDate_Unfocused(object sender, FocusEventArgs e)
        {
            //if (_old != _vm.DispatchVm.SearchDate)
            //     _vm.DispatchVm.Search();
        }

        async void RowSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //var item = e.SelectedItem as DispatchRowViewModel;

            //if (item == null)
            //    return;

            //await this.Navigation.PushAsync(new DispatchFormPage(item));
            //listView.SelectedItem = null;
        }

        private void DateEdit_DateChanged(object sender, EventArgs e)
        {
            var propcheck = startDate.Date;

            if (_old != _vm.DispatchVm.SearchDate)
            {
                _vm.DispatchVm.Search();
                _old = _vm.DispatchVm.SearchDate;
            }
        }

        async void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as DispatchRowViewModel;
            if (item == null)
                return;

            await this.Navigation.PushAsync(new DispatchesPageView(item));
            listView.SelectedItem = null;
        }
    }
}