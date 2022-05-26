using FocalPoint.Modules.FrontCounter.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.MainMenu.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardListView : ContentView
    {
        public DashboardListView()
        {
            InitializeComponent();
        }
    }
}