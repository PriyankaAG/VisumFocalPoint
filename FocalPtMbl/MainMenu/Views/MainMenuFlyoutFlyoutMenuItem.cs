using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using FocalPtMbl.MainMenu.Services;
using FocalPtMbl.MainMenu.ViewModels.Services;
using FocalPtMbl.MainMenu.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FocalPoint.MainMenu.Views
{
    public class MainMenuFlyoutFlyoutMenuItem
    {
        NavigationService NavService;
        public MainMenuFlyoutFlyoutMenuItem(NavigationService _navService)
        {
            TargetType = typeof(MainMenuFlyoutFlyoutMenuItem);
            NavService = _navService;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }

        public List<MainMenuFlyoutSubItem> SubMenuItems { get; set; }

        public bool IsBadgeVisible { get; set; }
        private int badgeCount;
        public int BadgeCount
        {
            get
            {
                return badgeCount;
            }
            set
            {
                badgeCount = value;
                if (value > 9)
                    BadgeCountDisplay = "9+";
                else
                    BadgeCountDisplay = value.ToString();

                IsBadgeVisible = value > 0;
            }
        }
        public string BadgeCountDisplay { get; set; }
        public string RowColor
        {
            get
            {
                if ((Id % 2) == 0)
                    return "#ffffff";
                else
                    return "#f4f4f4";
            }
        }

        public Type TargetType { get; set; }

    }

    public class MainMenuFlyoutSubItem
    {
        NavigationService NavService;
        public MainMenuFlyoutSubItem(NavigationService _navService)
        {
            NavService = _navService;
        }
        public string SubItemText { get; set; }
        public Type SubText_TargetType { get; set; }
        public bool IsVisible { get; set; } = false;
        public ICommand TapSubItemCommand => new Command(() => ExecuteClickCommand());
        protected async void ExecuteClickCommand()
        {
            var NavSer = DependencyService.Resolve<INavigationService>();

            Page page;
            if (SubText_TargetType == typeof(MainPage))
            {
                page = Application.Current.MainPage;
                //page.Title = item.Title;
                //Detail = ((MainMenuFlyout)Application.Current.MainPage).navPage;
            }
            else
            {
                await NavSer.PushPageFromMenu(SubText_TargetType);
            }
        }
    }
}
