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
    public partial class DispatchesPageView : ContentPage
    {
        DispatchesPageViewModel _vm = new DispatchesPageViewModel(null);
        public DispatchesPageView()
        {
           InitializeComponent();
            this.BindingContext = _vm;

            var trucks = ((DispatchesPageViewModel)this.BindingContext).GetTrucks();
            _vm = new DispatchesPageViewModel(trucks);
            this.BindingContext = _vm;

            //_vm.Search();




            //this.Children.Add(new TruckPageView(new TruckPageViewModel(_vm, null)));

            //foreach (var t in _vm.TruckViewModels)
            //{
            //    this.Children.Add(new TruckPage(t));
            //}
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {

        }
    }
}