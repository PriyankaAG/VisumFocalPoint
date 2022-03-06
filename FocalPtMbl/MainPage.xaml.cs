using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using DevExpress.XamarinForms;
using DevExpress.XamarinForms.Editors.Internal;

namespace FocalPtMbl
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            DevExpress.XamarinForms.Charts.Initializer.Init();
            DevExpress.XamarinForms.Navigation.Initializer.Init();
            DevExpress.XamarinForms.DataGrid.Initializer.Init();
            DevExpress.XamarinForms.Editors.Initializer.Init();
            //DevExpress.XamarinForms.Popup.Initializer.Init();
            DevExpress.XamarinForms.CollectionView.Initializer.Init();
            InitializeComponent();
        }
    }
}
