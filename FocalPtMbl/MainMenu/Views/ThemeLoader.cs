using DevExpress.XamarinForms.Core.Themes;
using FocalPtMbl.Themes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FocalPtMbl.MainMenu.Views
{
    public interface IEnvironment
    {
        string PhoneId { get; set; }
        Task<bool> IsLightOperatingSystemTheme();
    }
    internal class ThemeLoader : IThemeChangingHandler
    {
        static ThemeLoader instance = null;
        IThemeLoader platformLoader = null;
        public static ThemeLoader Instance
        {
            get
            {
                if (instance == null)
                    instance = new ThemeLoader();

                return instance;
            }
        }

        private ThemeLoader() 
        {
            platformLoader = DependencyService.Get<IThemeLoader>();
            ThemeManager.AddThemeChangedHandler(this);
        }

        public void LoadTheme()
        {
            bool isLightTheme = ThemeManager.ThemeName == Theme.Light;
            ResourceDictionary theme = null;
            if (isLightTheme)
            {
                theme = new LightTheme();
            }
            else
            {
                theme = new DarkTheme();
            }
            if (theme != null)
            {
                Application.Current.Resources.MergedDictionaries.Add(theme);
                platformLoader?.LoadTheme(theme, isLightTheme);
            }
        }

        void IThemeChangingHandler.OnThemeChanged()
        {
            LoadTheme();
        }
    }
    public interface IThemeLoader
    {
        void LoadTheme(ResourceDictionary theme, bool isLightTheme);
    }
}
