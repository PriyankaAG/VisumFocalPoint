using FocalPoint.Modules.FrontCounter.Views;
using FocalPoint.Modules.FrontCounter.Views.NewRentals;
using FocalPtMbl.MainMenu.Models;
using FocalPtMbl.Modules.Orders.Views;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace FocalPtMbl.MainMenu.Data
{
    public interface INestedTabView
    {
        bool CanBeShown();
    }
    public interface ITabPages
    {
        bool CanBeShown();
    }
    public class FrontCounter : IPageData 
    {
        
        readonly List<PageItem> pageItems;
        readonly INestedTabView nestedTabView;
        readonly ITabPages tabPages;

        public FrontCounter()
        {
            this.nestedTabView = DependencyService.Get<INestedTabView>();
            this.tabPages = DependencyService.Get<ITabPages>();
            this.pageItems = new List<PageItem>()
            {
                new PageItem()
                {
                    Title = "Quick Rental",
                    ControlsPageTitle = "Quick Rental",
                    PageTitle = "Quick Rental",
                    Description = "Create Quotes, Reservations and Orders",
                    Module = typeof(NewQuickRentalMainPage),
                    Icon = "QuickRental_32.png",
                    IconOverlayText = IconOverlayText.None
                },
                new PageItem()
                {
                    Title = "View Orders",
                    ControlsPageTitle = "View Orders",
                    PageTitle = "View Existing",
                    Description = "View Open Quotes, Reservations and Orders",
                    Module = typeof(EditExistingOrdersView),
                    Icon = "Order_32.png",
                    IconOverlayText = IconOverlayText.None
                },
                new PageItem()
                {
                    Title = "Returns",
                    ControlsPageTitle = "Returns",
                    PageTitle = "Returns",
                    Description = "Close and Finish Open Orders",
                    Module = typeof(ReturnsView),
                    Icon = "Order_return_32.png",
                    IconOverlayText = IconOverlayText.None
                }
            };
            this.pageItems[this.pageItems.Count - 1].ShowItemUnderline = false;
    }
        public string Title => "Orders";
        public List<PageItem> PageItems => this.pageItems;
    }
}
