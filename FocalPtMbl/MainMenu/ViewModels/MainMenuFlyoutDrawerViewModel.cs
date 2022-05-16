﻿using FocalPoint.Data;
using FocalPoint.MainMenu.Views;
using FocalPoint.Modules.Administrative.Views;
using FocalPoint.Modules.CustomerRelations.Views;
using FocalPoint.Modules.Dispatching.Views;
using FocalPoint.Modules.FrontCounter.Views;
using FocalPoint.Modules.FrontCounter.Views.Rentals;
using FocalPoint.Modules.Inventory.Views;
using FocalPoint.Modules.ServiceDepartment.Views;
using FocalPtMbl.MainMenu.ViewModels;
using FocalPtMbl.MainMenu.ViewModels.Services;
using FocalPtMbl.MainMenu.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
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

        public INavigation NavigationObject { get; set; }

        public MainMenuFlyoutDrawerViewModel(IOpenUriService openService) : base(openService)
        {
            MenuItems = new ObservableCollection<MainMenuFlyoutFlyoutMenuItem>(new[]
            {
                    new MainMenuFlyoutFlyoutMenuItem() {
                        Id = 0,
                        Title = "Dashboard",
                        Icon = "dashboard.png",
                        TargetType = typeof(MainPage) ,
                        SubMenuItems = new List<MainMenuFlyoutSubItem>
                        {
                            new MainMenuFlyoutSubItem() {
                                Title = "Daily Revenue",
                                SubItemText = "Daily Revenue",
                                IsVisible = true,
                                SubText_TargetType = typeof(DailyRevenueView)
                            },
                            new MainMenuFlyoutSubItem() {
                                Title = "Cash Drawer",
                                SubItemText = "Cash Drawer",
                                IsVisible = true,
                                SubText_TargetType = typeof(CashDrawerSummaryView)
                            },
                            new MainMenuFlyoutSubItem() {
                                Title = "Rental Valuation",
                                SubItemText = "Rental Valuation",
                                IsVisible = true,
                                SubText_TargetType = typeof(RentalValuationSummaryView)
                            },
                        }
                    },
                    new MainMenuFlyoutFlyoutMenuItem() {
                        Id = 1,
                        Title = "Front Counter" ,
                        Icon = "storefront.png"  ,
                        TargetType=typeof(FrontCounter),
                        SubMenuItems = new List<MainMenuFlyoutSubItem>
                        {
                             new MainMenuFlyoutSubItem() {
                                Title = "Dashboard",
                                SubItemText = "Front Counter Dashboard",
                                IsVisible = true,
                                SubText_TargetType = typeof(FrontCounter)
                            },
                            new MainMenuFlyoutSubItem() {
                                Title = "Quick Rental",
                                SubItemText = "Quick Rental",
                                IsVisible = true,
                                SubText_TargetType = typeof(QuickOrderCreationOutlineView)
                            },
                            new MainMenuFlyoutSubItem() {
                                Title = "View Orders",
                                SubItemText = "View Orders",
                                IsVisible = true,
                                SubText_TargetType = typeof(EditExistingOrdersView)
                            },
                            new MainMenuFlyoutSubItem() {
                                Title = "Returns",
                                SubItemText = "Returns",
                                IsVisible = true,
                                SubText_TargetType = typeof(ReturnsView)
                            },
                        }
                    },
                    new MainMenuFlyoutFlyoutMenuItem() {
                        Id = 2,
                        Title = "Dispatch" ,
                        Icon = "dispatch.png",
                        TargetType = typeof(ScheduleDispatchingPageView) ,
                        SubMenuItems = new List<MainMenuFlyoutSubItem>
                        {
                            new MainMenuFlyoutSubItem() {
                                Title = "Dispatching",
                                SubItemText = "Dispatching",
                                IsVisible = true,
                                SubText_TargetType = typeof(ScheduleDispatchingPageView)
                            },
                            new MainMenuFlyoutSubItem() {
                                Title = "Pickup Tickets",
                                SubItemText = "Pickup Tickets",
                                IsVisible = true,
                                SubText_TargetType = typeof(PickupTicketsSelectView)
                            }
                        }
                    },
                    new MainMenuFlyoutFlyoutMenuItem() {
                        Id = 3,
                        Title = "Service Department" ,
                        Icon = "service_department.png" ,
                        SubMenuItems = new List<MainMenuFlyoutSubItem>
                        {
                            new MainMenuFlyoutSubItem() {
                                Title = "View Work Orders",
                                SubItemText = "View Work Order",
                                IsVisible = true,
                                SubText_TargetType = typeof(WorkOrderFormView)
                            }
                        }
                    },
                    new MainMenuFlyoutFlyoutMenuItem() {
                        Id = 4,
                        Title = "Customer Relations" ,
                        Icon = "customer_relations.png"  ,
                        SubMenuItems = new List<MainMenuFlyoutSubItem>
                        {
                            new MainMenuFlyoutSubItem() {
                                Title = "Customers",
                                SubItemText = "Customers",
                                IsVisible = true,
                                SubText_TargetType = typeof(CustomerSimpleView)
                            }
                        }
                    },
                    new MainMenuFlyoutFlyoutMenuItem() { Id = 5,
                        Title = "Inventory" ,
                        Icon = "inventory.png"  ,
                        SubMenuItems = new List<MainMenuFlyoutSubItem>
                        {
                            new MainMenuFlyoutSubItem() {
                                Title = "Rental File List",
                                SubItemText = "Rental File List",
                                IsVisible = true,
                                SubText_TargetType = typeof(OpenRentalsView)
                            },
                            new MainMenuFlyoutSubItem() {
                                Title = "Vendors",
                                SubItemText = "Vendors",
                                IsVisible = true,
                                SubText_TargetType = typeof(VendorsView)
                            },
                            new MainMenuFlyoutSubItem() {
                                Title = "Rental Availability",
                                SubItemText = "Rental Availability",
                                IsVisible = true,
                                SubText_TargetType = typeof(RentalsView)
                        },
                        }
                    }
             });
            TimeClockCommand = new Command(() => OnTimeClock());
            ManageProfileCommand = new Command(() => OnManageProfile());
            LogOutCommand = new Command(() => OnLogOut());
        }

        public void OnTimeClock()
        { }
        public void OnManageProfile()
        { }
        public async void OnLogOut()
        {
            bool loggedOut = await FocalPtMbl.App.Current.MainPage.DisplayAlert("Logout", "Are you sure you want to logout", "Ok", "Cancel");
            if (loggedOut)
            {
                Logoff();
                DataManager.ClearSettings();
                await Application.Current.MainPage.Navigation.PushModalAsync(new LoginPageNew());
            }
        }

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
