using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FocalPoint.MainMenu.ViewModels;
using FocalPtMbl.MainMenu.Services;
using FocalPtMbl.MainMenu.ViewModels;
using FocalPtMbl.MainMenu.ViewModels.Services;
using FocalPtMbl.MainMenu.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FocalPoint.MainMenu.Views
{
    public class MainMenuFlyoutFlyoutMenuItem : NotificationObject
    {
        public MainMenuFlyoutFlyoutMenuItem()
        {
            TargetType = typeof(MainMenuFlyoutFlyoutMenuItem);
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

                return "#ffffff";

            }
        }

        public Type TargetType { get; set; }

    }

    public class MainMenuFlyoutSubItem : NotificationObject
    {
        public MainMenuFlyoutSubItem()
        {
        }
        public string Title { get; set; }
        public string SubItemText { get; set; }
        public Type SubText_TargetType { get; set; }
        public bool IsVisible { get; set; } = false;
        private bool _isSelected = false;
        public bool IsSelected {
            get 
            {
                return _isSelected;
            } 
            set 
            {
                _isSelected = value;
                //if (_isSelected)
                //{
                //    ItemColor = "#57b8ff";
                //    ItemTextColor = "#ffffff";
                //}
                //else
                //{
                //    ItemColor = "#00000000";
                //    ItemTextColor = "#0058ff";
                //}

                OnPropertyChanged("IsSelected");
                OnPropertyChanged("ItemColor");
                OnPropertyChanged("ItemTextColor");
            }
        }

        public string ItemColor
        {
            get;
            set;
        }
        public string ItemTextColor
        {
            get;
            set;
        } = "#0058ff";
        public ICommand TapSubItemCommand => new Command(() =>
        {
            ExecuteClickCommand();
        });
        protected async void ExecuteClickCommand()
        {
            ((Application.Current.MainPage as MainMenuFlyout).FlyoutPageDrawerObject.BindingContext as MainMenuFlyoutDrawerViewModel).ResetSelectedItem();

            IsSelected = true;

            var NavSer = DependencyService.Resolve<INavigationService>();

            await NavSer.PushPageFromMenu(SubText_TargetType, Title);
        }
    }
}
