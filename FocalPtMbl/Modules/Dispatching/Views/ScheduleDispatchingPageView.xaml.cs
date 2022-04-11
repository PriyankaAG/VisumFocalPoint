using FocalPoint.Modules.Dispatching.ViewModels;
using FocalPtMbl;
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
    public partial class ScheduleDispatchingPageView : TabbedPage
    {
        DispatchesPageViewModel _vm = new DispatchesPageViewModel(null);

        public string NewTitle { get; set; }
        public ScheduleDispatchingPageView()
        {
            InitializeComponent();
            this.BindingContext = _vm;

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ((DispatchesPageViewModel)this.BindingContext).Indicator = true;

            LoadData();

        }

        public async void LoadData()
        {
            var trucks = await ((DispatchesPageViewModel)this.BindingContext).GetTrucks();

            _vm = new DispatchesPageViewModel(trucks);
            this.BindingContext = _vm;

            _vm.Search();

            this.Children.Clear();

            Device.BeginInvokeOnMainThread(() =>
            {
                var ind = new TruckPageViewModel(_vm, null);
                this.Children.Add(new TruckPageView(ind));
                OnPropertyChanged("Children");
            });

            foreach (var t in _vm.TruckViewModels)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var objView = new TruckPageView(t);
                    objView.MinimumWidthRequest = 100;
                    this.Children.Add(objView);
                });
            }
            OnPropertyChanged("Children");
            ((DispatchesPageViewModel)this.BindingContext).Indicator = false;
        }
    }
}