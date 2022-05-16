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

        public static readonly BindableProperty HeaderProperty = BindableProperty.Create(nameof(Image), typeof(string), typeof(DashboardListView),
            default(string), Xamarin.Forms.BindingMode.OneWay);
        public string Header
        {
            get
            {
                return (string)GetValue(HeaderProperty);
            }

            set
            {
                SetValue(HeaderProperty, value);
            }
        }

        public static readonly BindableProperty ListDataProperty = BindableProperty.Create(nameof(Image), typeof(string), typeof(DashboardListView),
            default(OrderDashboardOverviewDeatail), Xamarin.Forms.BindingMode.OneWay);
        public ObservableCollection<OrderDashboardOverviewDeatail> ListData
        {
            get
            {
                return (ObservableCollection<OrderDashboardOverviewDeatail>)GetValue(ListDataProperty);
            }

            set
            {
                SetValue(ListDataProperty, value);
            }
        }
    }
}