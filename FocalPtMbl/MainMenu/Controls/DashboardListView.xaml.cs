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

        public static readonly BindableProperty IsRentalProperty = BindableProperty.Create(nameof(IsRental), typeof(bool), typeof(DashboardListView), default(bool), BindingMode.TwoWay);
        public bool IsRental
        {
            get
            {
                return (bool)GetValue(IsRentalProperty);
            }

            set
            {
                SetValue(IsRentalProperty, value);
            }
        }

        public static readonly BindableProperty IsWorkOrderProperty = BindableProperty.Create(nameof(IsWorkOrder), typeof(bool), typeof(DashboardListView), default(bool), BindingMode.TwoWay);
        public bool IsWorkOrder
        {
            get
            {
                return (bool)GetValue(IsWorkOrderProperty);
            }

            set
            {
                SetValue(IsWorkOrderProperty, value);
            }
        }
    }
}