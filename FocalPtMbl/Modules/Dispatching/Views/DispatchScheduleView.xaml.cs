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
    public partial class DispatchScheduleView : ContentPage
    {
        public DispatchScheduleView()
        {
            InitializeComponent();
        }

        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}