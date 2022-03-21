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
    public partial class SignatureTermsView : ContentPage
    {
        public SignatureTermsView()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, "AllowLandscape");
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send(this, "PreventLandscape");
        }
    }
}