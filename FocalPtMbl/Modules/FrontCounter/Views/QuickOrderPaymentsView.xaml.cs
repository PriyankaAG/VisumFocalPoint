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
    public partial class QuickOrderPaymentsView : ContentPage
    {
        public QuickOrderPaymentsView(Order curOrder)
        {
            this.Title = "Payments";
            InitializeComponent();
        }

        private async void SimpleButton_Clicked(object sender, EventArgs e)
        {
            //Push Payment page
            await Navigation.PushAsync(new QuickOrderPaymentsSelectView());
        }

        private void collectionView_Tap(object sender, DevExpress.XamarinForms.CollectionView.CollectionViewGestureEventArgs e)
        {

        }
    }
}