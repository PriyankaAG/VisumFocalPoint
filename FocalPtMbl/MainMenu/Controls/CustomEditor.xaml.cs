using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.MainMenu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomEditor : ContentView
    {
        public CustomEditor()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ImageProperty = BindableProperty.Create(nameof(Image), typeof(string), typeof(CustomEditor), 
            default(string), Xamarin.Forms.BindingMode.OneWay);
        public string Image
        {
            get
            {
                return (string)GetValue(ImageProperty);
            }

            set
            {
                SetValue(ImageProperty, value);
            }
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(CustomTextValue), typeof(string), typeof(CustomEditor), default(string), BindingMode.TwoWay);
        public string CustomTextValue
        {
            get
            {
                return (string)GetValue(TextProperty);
            }

            set
            {
                SetValue(TextProperty, value);
            }
        }

        public static readonly BindableProperty PlaceHolderProperty = BindableProperty.Create(nameof(PlaceHolder), typeof(string), typeof(CustomEditor), default(string), BindingMode.TwoWay);
        public string PlaceHolder
        {
            get
            {
                return (string)GetValue(PlaceHolderProperty);
            }

            set
            {
                SetValue(PlaceHolderProperty, value);
            }
        }

        public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(nameof(IsPassword), typeof(string), typeof(CustomEditor), default(string), BindingMode.TwoWay);
        public string IsPassword
        {
            get
            {
                return (string)GetValue(IsPasswordProperty);
            }

            set
            {
                SetValue(IsPasswordProperty, value);
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            CustomTextValue = e.NewTextValue;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            /*if (propertyName == ImageProperty.PropertyName)
            {
                title.Text = Image;
            }
            else if (propertyName == TextProperty.PropertyName)
            {
                entry.Text = Text;
            }*/
        }
    }
}
