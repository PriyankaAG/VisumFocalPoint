using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FocalPoint.Modules.Payments.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Payments.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentProcessView : ContentView
    {
        PaymentPageViewModel viewModel;
        public PaymentProcessView()
        {
            InitializeComponent();
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            viewModel = (PaymentPageViewModel)BindingContext;
        }

        //private void ImageButton_Clicked(object sender, EventArgs e)
        //{
        //    var imageSource = ((ImageButton)sender).Source.ToString();
        //    if (imageSource.Contains("arrow_down.png"))
        //    {
        //        PaymentGrid.IsVisible = false;
        //        ((ImageButton)sender).Source = "arrow_up.png";
        //    }
        //    else
        //    {
        //        PaymentGrid.IsVisible = true;
        //        ((ImageButton)sender).Source = "arrow_down.png";
        //    }
        //}
    }
}