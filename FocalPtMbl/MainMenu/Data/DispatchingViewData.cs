using FocalPoint.Modules.Dispatching.Views;
using FocalPoint.Modules.Payments.Views;
using FocalPtMbl.MainMenu.Data;
using FocalPtMbl.MainMenu.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace FocalPoint.MainMenu.Data
{
    public interface INestedTabView
    {
        bool CanBeShown();
    }
    public interface ITabPages
    {
        bool CanBeShown();
    }
    public class DispatchingViewData : IPageData
    {

        readonly List<PageItem> pageItems;
        readonly INestedTabView nestedTabView;
        readonly ITabPages tabPages;

        public DispatchingViewData()
        {
            this.nestedTabView = DependencyService.Get<INestedTabView>();
            this.tabPages = DependencyService.Get<ITabPages>();
            this.pageItems = new List<PageItem>()
            {
                new PageItem()
                {
                    Title = "Dispatching",
                    ControlsPageTitle = "Dispatching",
                    PageTitle = "Dispatching",
                    Description = "A Calendar View For The Delivery Schedule",
                    Module = typeof(ScheduleDispatchingPageView),
                    Icon = "Dispatch_Schedule_32.png",
                    IconOverlayText = IconOverlayText.None
                },

                new PageItem()
                {
                    Title = "Pickup Tickets",
                    ControlsPageTitle = "Pickup Tickets",
                    PageTitle = "Pickup Tickets",
                    Description = "The Stores Pickup Tickets",
                    //Module = typeof(PickupTicketsSelectView),
                    Module = typeof(PaymentPageView),
                    Icon = "laden_pickup_48.png",
                    IconOverlayText = IconOverlayText.None
                }

                //new PageItem()
                //{
                //    Title = "Add new Customer",
                //    ControlsPageTitle = "",
                //    PageTitle = "",
                //    Description = "",
                //    Module = typeof(CustomerFormView),
                //    Icon = "",
                //    IconOverlayText = IconOverlayText.None
                //}
            };
            this.pageItems[this.pageItems.Count - 1].ShowItemUnderline = false;
        }
        public string Title => "Dispatch";
        public List<PageItem> PageItems => this.pageItems;
    }
}
