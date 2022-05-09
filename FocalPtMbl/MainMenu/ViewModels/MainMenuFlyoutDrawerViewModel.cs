using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using FocalPoint.MainMenu.Views;
using FocalPoint.Modules.Dispatching.Views;
using FocalPtMbl.MainMenu.Services;
using FocalPtMbl.MainMenu.ViewModels;
using FocalPtMbl.MainMenu.ViewModels.Services;
using FocalPtMbl.MainMenu.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FocalPoint.MainMenu.ViewModels
{
    public class MainMenuFlyoutDrawerViewModel : AboutPageViewModel, INotifyPropertyChanged
    {
        public ObservableCollection<MainMenuFlyoutFlyoutMenuItem> MenuItems { get; set; }
        public ICommand TapSubItemCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));

        public ICommand TimeClockCommand { get; }
        public ICommand ManageProfileCommand { get; }
        public ICommand LogOutCommand { get; }

        NavigationService NavService;

        public MainMenuFlyoutDrawerViewModel(IOpenUriService openService, NavigationService _navService ) : base(openService)
        {
            NavService = _navService;

            MenuItems = new ObservableCollection<MainMenuFlyoutFlyoutMenuItem>(new[]
            {
                    new MainMenuFlyoutFlyoutMenuItem(NavService) {
                        Id = 0,
                        Title = "Dashboard",
                        Icon = "dashboard.png",
                        TargetType = typeof(MainPage) ,
                        SubMenuItems = new List<MainMenuFlyoutSubItem>
                        {
                            new MainMenuFlyoutSubItem(NavService) {
                                SubItemText = "Daily Revenue",
                                IsVisible = true,
                                SubText_TargetType = typeof(MainPage)
                            },
                            new MainMenuFlyoutSubItem(NavService) {
                                SubItemText = "Cash Drawer",
                                IsVisible = true,
                                SubText_TargetType = typeof(MainPage)
                            },
                            new MainMenuFlyoutSubItem(NavService) {
                                SubItemText = "Rental Valuation",
                                IsVisible = true,
                                SubText_TargetType = typeof(MainPage)
                            },
                        }
                    },
                    new MainMenuFlyoutFlyoutMenuItem(NavService) {
                        Id = 1,
                        Title = "Front Counter" ,
                        Icon = "storefront.png"  ,
                        SubMenuItems = new List<MainMenuFlyoutSubItem>
                        {
                            new MainMenuFlyoutSubItem(NavService) {
                                SubItemText = "Quick Rental",
                                IsVisible = true,
                                SubText_TargetType = typeof(MainPage)
                            },
                            new MainMenuFlyoutSubItem(NavService) {
                                SubItemText = "View Orders",
                                IsVisible = true,
                                SubText_TargetType = typeof(MainPage)
                            },
                            new MainMenuFlyoutSubItem(NavService) {
                                SubItemText = "Returns",
                                IsVisible = true,
                                SubText_TargetType = typeof(MainPage)
                            },
                        }
                    },
                    new MainMenuFlyoutFlyoutMenuItem(NavService) {
                        Id = 2,
                        Title = "Dispatch" ,
                        Icon = "dispatch.png",
                        BadgeCount = 5,
                        TargetType = typeof(ScheduleDispatchingPageView) ,
                        SubMenuItems = new List<MainMenuFlyoutSubItem>
                        {
                            new MainMenuFlyoutSubItem(NavService) {
                                SubItemText = "Dispatching",
                                IsVisible = true,
                                SubText_TargetType = typeof(ScheduleDispatchingPageView)
                            },
                            new MainMenuFlyoutSubItem(NavService) {
                                SubItemText = "Pickup Tickets",
                                IsVisible = true,
                                SubText_TargetType = typeof(PickupTicketsSelectView)
                            },
                            new MainMenuFlyoutSubItem(NavService) {
                                IsVisible = false
                            }
                        }
                    },
                    new MainMenuFlyoutFlyoutMenuItem(NavService) {
                        Id = 3,
                        Title = "Service Department" ,
                        Icon = "service_department.png" ,
                        SubMenuItems = new List<MainMenuFlyoutSubItem>
                        {
                            new MainMenuFlyoutSubItem(NavService) {
                                SubItemText = "View Work Order",
                                IsVisible = true,
                                SubText_TargetType = typeof(MainPage)
                            },
                            new MainMenuFlyoutSubItem(NavService) {
                                IsVisible = false
                            },
                            new MainMenuFlyoutSubItem(NavService) {
                                IsVisible = false
                            }
                        }
                    },
                    new MainMenuFlyoutFlyoutMenuItem(NavService) {
                        Id = 4,
                        Title = "Customer Relations" ,
                        Icon = "customer_relations.png"  ,
                        SubMenuItems = new List<MainMenuFlyoutSubItem>
                        {
                            new MainMenuFlyoutSubItem(NavService) {
                                SubItemText = "Customers",
                                IsVisible = true,
                                SubText_TargetType = typeof(MainPage)
                            },
                            new MainMenuFlyoutSubItem(NavService) {
                                IsVisible = false
                            },
                            new MainMenuFlyoutSubItem(NavService) {
                                IsVisible = false
                            }
                        }
                    },
                    new MainMenuFlyoutFlyoutMenuItem(NavService) { Id = 5,
                        Title = "Inventory" ,
                        BadgeCount = 11,
                        Icon = "inventory.png"  ,
                        SubMenuItems = new List<MainMenuFlyoutSubItem>
                        {
                            new MainMenuFlyoutSubItem(NavService) {
                                SubItemText = "Rental File List",
                                IsVisible = true,
                                SubText_TargetType = typeof(MainPage)
                            },
                            new MainMenuFlyoutSubItem(NavService) {
                                SubItemText = "Vendors",
                                IsVisible = true,
                                SubText_TargetType = typeof(MainPage)
                            },
                            new MainMenuFlyoutSubItem(NavService) {
                                SubItemText = "Rental Availability",
                                IsVisible = true,
                                SubText_TargetType = typeof(MainPage)
                        },
                        }
                    }
             });
            TimeClockCommand = new Command(() => OnTimeClock());
            ManageProfileCommand = new Command(() => OnManageProfile());
            LogOutCommand = new Command(() => OnLogOut());
            NavService = _navService;
        }

        public void OnTimeClock()
        { }
        public void OnManageProfile()
        { }
        public void OnLogOut()
        { }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
