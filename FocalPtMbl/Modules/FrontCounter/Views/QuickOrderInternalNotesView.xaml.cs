using DevExpress.XamarinForms.CollectionView;
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
    public partial class QuickOrderInternalNotesView : ContentPage
    {
        public QuickOrderInternalNotesView()
        {
            InitializeComponent();
        }
        private async void SimpleButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

    }
}