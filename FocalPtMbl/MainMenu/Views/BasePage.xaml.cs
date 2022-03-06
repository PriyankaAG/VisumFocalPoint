using DevExpress.XamarinForms.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPtMbl.MainMenu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasePage : DrawerPage
    {
        bool originalSizesSaved = false;
        Thickness originalAboutViewPaddings;
        double iOSSpecificAboutTopPadding = 10;

        public NavigationPage NavPage => navPage;

        public BasePage()
        {
            InitializeComponent();

        }
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page.SafeAreaInsetsProperty.PropertyName)
                SetInsetsToAboutViewSizes();
            if (propertyName == nameof(DrawerPage.IsDrawerOpened))
                ChangeDrawerContentScrollsToTop();
        }

        void SetInsetsToAboutViewSizes()
        {
            Thickness insets = Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page.GetSafeAreaInsets(this);
            if (!originalSizesSaved)
            {
                originalAboutViewPaddings = AboutPage.Padding;
                originalSizesSaved = true;
            }
            AboutPage.Padding = new Thickness(
                originalAboutViewPaddings.Left,
                insets.Top > 0 ? iOSSpecificAboutTopPadding : originalAboutViewPaddings.Top,
                originalAboutViewPaddings.Right,
                originalAboutViewPaddings.Bottom);
        }
        void ChangeDrawerContentScrollsToTop()
        {
            AboutPage.OpenedByParent = this.IsDrawerOpened;
        }
    }
}