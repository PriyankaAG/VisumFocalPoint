using FocalPoint.Data;
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
using Visum.Services.Mobile.Entities;
using System.Linq;
using FocalPoint.Modules.FrontCounter.Views.NewRentals;
using FocalPoint.Modules.Payments.Views;

namespace FocalPoint.MainMenu.ViewModels
{
    public class MainMenuFlyoutDrawerViewModel : AboutPageViewModel, INotifyPropertyChanged
    {
        public ObservableCollection<MainMenuFlyoutFlyoutMenuItem> MenuItems { get; set; }
        public ICommand TapSubItemCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));

        public ICommand TimeClockCommand { get; }
        public bool IsTimeClockVisible { get; }
        public ICommand ManageProfileCommand { get; }
        public ICommand LogOutCommand { get; }

        public INavigation NavigationObject { get; set; }

        public MainMenuFlyoutDrawerViewModel(IOpenUriService openService) : base(openService)
        {
            DataManager dataManager = new DataManager();
            IsTimeClockVisible = DataManager.UserIsAllowed(Security.Areas.MBL_TimeClock);

            MenuItems = new ObservableCollection<MainMenuFlyoutFlyoutMenuItem>();

            /*As per DOC : 
            Mbl_Dash = Dashboard, previously named MBL_Snapshot
            Mbl_Dash_CashDrawers = Cash Drawers, previously named MBL_Snapshot_CashDrawer
            Mbl_Dash_RentalVal = Rental Valuation, previously named MBL_Snapshot_RentalVal
            Mbl_Dash_Revenue = Revenue, previously named MBL_Snapshot_Revenue
            •	User would not have access to Mbl_Dash if they don’t have access to others
            */
            if (DataManager.UserIsAllowed(Security.Areas.MBL_Dash) &&
                DataManager.UserIsAllowed(Security.Areas.MBL_Dash_CashDrawer) &&
                DataManager.UserIsAllowed(Security.Areas.MBL_Dash_RentalVal) &&
                DataManager.UserIsAllowed(Security.Areas.MBL_Dash_Revenue))
            {
                MenuItems.Add(new MainMenuFlyoutFlyoutMenuItem()
                {
                    Id = 0,
                    Title = "Dashboard",
                    Icon = "dashboard.png",
                    SubMenuItems = new List<MainMenuFlyoutSubItem>
                    {
                        new MainMenuFlyoutSubItem() {
                            Title = "Dashboard",
                            SubItemText = "Dashboard",
                            IsVisible = true,
                            SubText_TargetType = typeof(MainPage)
                        },
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
                        }
                    }
                });
            }

            /*
             Mbl_FC = Front Counter
            Mbl_FC_Order = Quick Rental
            Mbl_FC_Orders = View Orders, previously named Mbl_Orders
            Mbl_FC_Returns = Return
            •	If a user does not have access to Rentals, Orders, or Returns Mbl_FC would be false for access
            */
            if (DataManager.UserIsAllowed(Security.Areas.MBL_FC) &&
                DataManager.UserIsAllowed(Security.Areas.MBL_FC_Order) &&
                DataManager.UserIsAllowed(Security.Areas.MBL_FC_Orders) &&
                DataManager.UserIsAllowed(Security.Areas.MBL_FC_Return))
            {
                MenuItems.Add(new MainMenuFlyoutFlyoutMenuItem()
                {
                    Id = 1,
                    Title = "Front Counter",
                    Icon = "storefront.png",
                    SubMenuItems = new List<MainMenuFlyoutSubItem>
                    {
                        new MainMenuFlyoutSubItem() {
                            Title = "Quick Rental",
                            SubItemText = "Quick Rental",
                            IsVisible = true,
                            SubText_TargetType = typeof(NewQuickRentalMainPage)
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
                });
            }

            /*
             Mbl_Dispatch = Dispatching, previously named Mbl_Dispatching
            •	Both Schedule and Pickup Tickets use the above, plans to expand in future
            */
            if (DataManager.UserIsAllowed(Security.Areas.MBL_Dispatch))
            {
                MenuItems.Add(new MainMenuFlyoutFlyoutMenuItem()
                {
                    Id = 2,
                    Title = "Dispatch",
                    Icon = "dispatch.png",
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
                });
            }

            /*
            Mbl_Serv = Service Department
            Mbl_Serv_WorkOrders = View Work Orders, previously named Mbl_WorkOrders
            •	If a user does not have access to WorkOrders, Mbl_Serv would be false for access
            */
            if (DataManager.UserIsAllowed(Security.Areas.MBL_Serv) &&
                DataManager.UserIsAllowed(Security.Areas.MBL_Serv_WorkOrders))
            {
                MenuItems.Add(new MainMenuFlyoutFlyoutMenuItem()
                {
                    Id = 3,
                    Title = "Service Department",
                    Icon = "service_department.png",
                    SubMenuItems = new List<MainMenuFlyoutSubItem>
                    {
                        new MainMenuFlyoutSubItem() {
                            Title = "View Work Orders",
                            SubItemText = "View Work Order",
                            IsVisible = true,
                            SubText_TargetType = typeof(WorkOrderFormView)
                        }
                    }
                });
            }

            /*
            Mbl_Cust = Customer Relations
            Mbl_Cust_Customers = Customers
            Mbl_Cust_Customers_Balance = Balance tab in Customer View, previously named MBL_Cust_Balance
            Mbl_Cust_Customers_EditNotes = Notes tab in Customer View, allows edit otherwise view only, previously named MBL_Cust_Notes
            •	If a user does not have access to Balance or EditNotes they still could have access to Customers
            •	If a user does not have access to Customers, they would not have access to Balance or EditNotes and Mbl_Cust would be false for access
            */
            if (DataManager.UserIsAllowed(Security.Areas.MBL_Cust) &&
                DataManager.UserIsAllowed(Security.Areas.MBL_Cust_Customers))
            {
                MenuItems.Add(new MainMenuFlyoutFlyoutMenuItem()
                {
                    Id = 4,
                    Title = "Customer Center",
                    Icon = "customer_relations.png",
                    SubMenuItems = new List<MainMenuFlyoutSubItem>
                        {
                            new MainMenuFlyoutSubItem() {
                                Title = "Customers",
                                SubItemText = "Customers",
                                IsVisible = true,
                                SubText_TargetType = typeof(CustomerSimpleView)
                            }
                        }
                });
            }


            /*Mbl_Inv = Inventory
            Mbl_Inv_Rentals = Rentals, previously named MBL_Rental
            Mbl_Inv_Rentals_Rev = Revenue tab in Rental View, previously named MBL_Rental_Revenue
            Mbl_Inv_Vendors = Vendors, previously named MBL_Vendor
            Mbl_Inv_Vendors_Balance = Balance tab in Vendor View, previously named MBL_Vendor_Balance
            Mbl_Inv_Availability = Rental Availability
            •	If a user does not have access to Revenue they still could have access to Rentals
            •	If a user does not have access to Balance they still could have access to Vendors
            •	If a user does not have access to Rentals, they would not have access to Revenue
            •	If a user does not have access to Vendors, they would not have access to Balance
            •	User would not have access to Mbl_Inv if they don’t have access to others */
            if (DataManager.UserIsAllowed(Security.Areas.MBL_Inv) &&
                DataManager.UserIsAllowed(Security.Areas.MBL_Inv_Rental) &&
                DataManager.UserIsAllowed(Security.Areas.MBL_Inv_Vendor))
            {
                //DataManager.UserIsAllowed(Security.Areas.MBL_Inv_Availability)
                MenuItems.Add(new MainMenuFlyoutFlyoutMenuItem()
                {
                    Id = 5,
                    Title = "Inventory",
                    Icon = "inventory.png",
                    SubMenuItems = new List<MainMenuFlyoutSubItem>
                    {
                        new MainMenuFlyoutSubItem() {
                            Title = "Rental List",
                            SubItemText = "Rental List",
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
                });
            }


            TimeClockCommand = new Command(() => OnTimeClock());
            ManageProfileCommand = new Command(() => OnManageProfile());
            LogOutCommand = new Command(() => OnLogOut());
        }
        public void ResetSelectedItem()
        {
            foreach (var item in MenuItems)
            {
                foreach (var subItem in item.SubMenuItems)
                {
                    if (subItem.IsSelected)
                    {
                        subItem.IsSelected = false;
                        OnPropertyChanged("MenuItems");
                    }
                }
            }
            //MenuItems.Where(c => c.SubMenuItems.Where(i => i.IsSelected == true).Any())
            //                .Select(c =>
            //                {
            //                    c.SubMenuItems.Where(i => i.IsSelected == true).
            //                    Select(j => { j.IsSelected = false; return j; });
            //                    return c;
            //                }).ToList();
        }

        public void OnTimeClock()
        {
            var NavSer = DependencyService.Resolve<INavigationService>();

            NavSer.PushPageFromMenu(typeof(FocalPoint.MainMenu.Views.TimeClockView), "Time Clock");

        }
        public void OnManageProfile()
        {
            var NavSer = DependencyService.Resolve<INavigationService>();
            NavSer.PushPageFromMenu(typeof(AboutPageNew), "FocalPoint Mobile");

        }
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
