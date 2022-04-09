using DevExpress.XamarinForms.Navigation;
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
    public partial class ScheduleDispatchingPageView
    {
        DispatchesPageViewModel _vm = new DispatchesPageViewModel(null);

        public string NewTitle { get; set; }
        public ScheduleDispatchingPageView()
        {
            InitializeComponent();
            this.BindingContext = _vm;

            ((DispatchesPageViewModel)this.BindingContext).Indicator = true;

            LoadData();

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

        }

        public async void LoadData()
        {
            var trucks = await ((DispatchesPageViewModel)this.BindingContext).GetTrucks();

            _vm = new DispatchesPageViewModel(trucks);
            this.BindingContext = _vm;

            _vm.Search();

            //this.Children.Clear();

            Device.BeginInvokeOnMainThread(() =>
            {
                //var actInd = this.Items[0].Content.FindByName<ActivityIndicator>("activityIndicator");
                //actInd.IsRunning = false;
                //actInd.IsVisible = false;


                var ind = new TruckPageViewModel(_vm, null);
                var objView = new TruckPageView(ind);
                objView.MinimumWidthRequest = 100;
                var pi = new TabPageItem();
                pi.Content = objView;
                pi.HeaderVisibleElements = HeaderElements.Text;

                this.Items.Add(pi);

                this.UpdateChildrenLayout();
            });

            foreach (var t in _vm.TruckViewModels)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var objView = new TruckPageView(t);
                    objView.MinimumWidthRequest = 100;
                    var pi = new TabPageItem();
                    pi.Content = objView;
                    pi.HeaderVisibleElements = HeaderElements.Text;
                    this.Items.Add(pi);


                });
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                //this.SelectedItemIndex = 1;
                //Task.Delay(1000).ContinueWith(task =>
                //{
                //    this.SelectedItemIndex = 0;
                //});

                this.Items.RemoveAt(0);
                this.ForceLayout();
                OnPropertyChanged("Items");
            });

            OnPropertyChanged("Items");
            ((DispatchesPageViewModel)this.BindingContext).Indicator = false;
        }
    }
}