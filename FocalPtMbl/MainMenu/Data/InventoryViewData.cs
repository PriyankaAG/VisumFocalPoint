using FocalPoint.Modules.FrontCounter.Views;
using FocalPoint.Modules.FrontCounter.Views.Rentals;
using FocalPoint.Modules.Inventory.Views;
using FocalPtMbl.MainMenu.Data;
using FocalPtMbl.MainMenu.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FocalPoint.MainMenu.Data
{
    public class InventoryViewData : IPageData
    {

        readonly List<PageItem> pageItems;
        readonly INestedTabView nestedTabView;
        readonly ITabPages tabPages;

        public InventoryViewData()
        {
            this.nestedTabView = DependencyService.Get<INestedTabView>();
            this.tabPages = DependencyService.Get<ITabPages>();
            this.pageItems = new List<PageItem>()
            {
                new PageItem()
                {
                    Title = "Rental File List",
                    ControlsPageTitle = "Rental File List",
                    PageTitle = "Rental File List",
                    Description = "The Rental File List",
                    Module = typeof(OpenRentalsView),
                    Icon = "user_filter_32.png",
                    IconOverlayText = IconOverlayText.None
                },
                new PageItem()
                {
                    Title = "Vendors",
                    ControlsPageTitle = "Vendors",
                    PageTitle = "Vendors",
                    Description = "A List Of Vendors",
                    Module = typeof(VendorsView),
                    Icon = "user_filter_32.png",
                    IconOverlayText = IconOverlayText.None
                },

                new PageItem()
                {
                    Title = "Rental Availability",
                    ControlsPageTitle = "Rental Availability",
                    PageTitle = "Rental Availability",
                    Description = "Rental Availability",
                    Module = typeof(RentalsView),
                    Icon = "Rental_Schedule_24",
                    IconOverlayText = IconOverlayText.None
                }
            };
            this.pageItems[this.pageItems.Count - 1].ShowItemUnderline = false;
        }
        public string Title => "Inventory";
        public List<PageItem> PageItems => this.pageItems;
    }
}