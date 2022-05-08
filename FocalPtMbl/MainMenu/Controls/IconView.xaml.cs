using System;
using DevExpress.XamarinForms.Core.Themes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPtMbl.MainMenu.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IconView : Image
    {
        public static readonly BindableProperty ThemeNameProperty = BindableProperty.Create("ThemeName", typeof(string),
            typeof(IconView), propertyChanged: ThemeNamePropertyChanged, defaultValue: Theme.Light);

        static void ThemeNamePropertyChanged(BindableObject bindable, object oldValue, object newValue) =>
            ((IconView)bindable).OnThemeNameChanged((string)newValue);

        public static readonly BindableProperty ForegroundColorProperty = BindableProperty.Create("ForegroundColor",
            typeof(Color), typeof(IconView), defaultValue: Color.Default,
            propertyChanged: ForegroundColorPropertyChanged);

        static void ForegroundColorPropertyChanged(BindableObject bindable, object oldValue, object newValue) { }
        public IconView()
        {
            InitializeComponent();
            if (App.Current is FocalPtMbl.App  FPMobileApp)
            {
                FPMobileApp.ThemeChagedEvent += FPMobileApp_ThemeChagedEvent;
            }
        }
        private void FPMobileApp_ThemeChagedEvent(object sender, EventArgs e)
        {
            if (Source is FileImageSource)
                OnPropertyChanged(nameof(Image.Source));
        }
        public string ThemeName
        {
            get => (string)GetValue(ThemeNameProperty);
            set => SetValue(ThemeNameProperty, value);
        }
        public Color ForegroundColor
        {
            get => (Color)GetValue(ForegroundColorProperty);
            set => SetValue(ForegroundColorProperty, value);
        }
        void OnThemeNameChanged(string newValue)
        {
        }
    }
}