using DevExpress.XamarinForms.CollectionView.Themes;
using DevExpress.XamarinForms.Core.Themes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FocalPtMbl.MainMenu.ViewModels
{
    public class ThemeBaseViewModel : NotificationObject
    {
        bool isLightTheme = true;

        public string Title { get; set; }
        public bool IsLightTheme
        {
            get
            {
                isLightTheme = ThemeManager.ThemeName == "Light";
                    return isLightTheme;
            }
            set { SetProperty(ref isLightTheme, value, onChanged: () => ((App)Application.Current).ApplyTheme(isLightTheme, true)); }
        }

        private bool _indicator;
        public bool Indicator
        {
            get => _indicator;
            set
            {
                _indicator = value;
                OnPropertyChanged(nameof(Indicator));
            }
        }

        private bool _ordersEnabled;
        public bool OrdersEnabled
        {
            get => _ordersEnabled;
            set
            {
                _ordersEnabled = value;
                OnPropertyChanged(nameof(OrdersEnabled));
            }
        }

        public ICommand ThemeCommand { get; }
        public ThemeBaseViewModel()
        {
            ThemeCommand = new DelegateCommand(() => IsLightTheme = !isLightTheme);
        }

        public ThemeBaseViewModel(string title)
        {
            Title = title;
            ThemeCommand = new DelegateCommand(() => IsLightTheme = !isLightTheme);
        }
    }
}
