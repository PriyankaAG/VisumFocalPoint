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
        public TruckPageView()
        {
            InitializeComponent();
        }

        private void DateEdit_DateChanged(object sender, EventArgs e)
        {

        }

        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}