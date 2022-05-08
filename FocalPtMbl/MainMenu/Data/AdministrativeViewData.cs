using FocalPoint.Modules.Administrative.Views;
using FocalPtMbl.MainMenu.Data;
using FocalPtMbl.MainMenu.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FocalPoint.MainMenu.Data
{
    public class AdministrativeViewData : IPageData
    {

         List<PageItem> pageItems;
        readonly INestedTabView nestedTabView;
        readonly ITabPages tabPages;

        public AdministrativeViewData()
        {
            this.nestedTabView = DependencyService.Get<INestedTabView>();
            this.tabPages = DependencyService.Get<ITabPages>();
            if (true)
            {
                this.pageItems = new List<PageItem>()
            {
                new PageItem()
                {
                    Title = "Daily Revenue",
                    ControlsPageTitle = "Daily Revenue",
                    PageTitle = "Daily Revenue",
                    Description = "A Snapshot of Daily Revenue",
                    Module = typeof(DailyRevenueView),
                    Icon = "summary_dollar_48",
                    IconOverlayText = IconOverlayText.None
                },
                new PageItem()
                {
                    Title = "Cash Drawer",
                    ControlsPageTitle = "Cash Drawer",
                    PageTitle = "Cash Drawer",
                    Description = "A Snapshot of the Cash Drawer",
                    Module = typeof(CashDrawerSummaryView),
                    Icon = "summary_cashbox_48",
                    IconOverlayText = IconOverlayText.None
                },
                new PageItem()
                {
                    Title = "Rental Valuation",
                    ControlsPageTitle = "Rental Valuation",
                    PageTitle = "Rental Valuation",
                    Description = "A Snapshot of Rental Valuation",
                    Module = typeof(RentalValuationSummaryView),
                    Icon = "summary_rental_48",
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
            }
            else
            {
                this.pageItems = new List<PageItem>();
            }

            //if(DataManager)
            //if (/*hasSecurity ==*/ true)
            //{
            //    this.pageItems.Add(
            //    new PageItem()
            //    {
            //        Title = "TestSecurity",
            //        ControlsPageTitle = "TestSecurity",
            //        PageTitle = "Daily Revenue",
            //        Description = "A Snapshot of Daily Revenue",
            //        Module = typeof(DailyRevenueView),
            //        Icon = "grid_firstlook",
            //        IconOverlayText = IconOverlayText.None
            //    });
            //}
            //this.pageItems[this.pageItems.Count - 1].ShowItemUnderline = false;
        }
        public string Title => "Inventory";
        public List<PageItem> PageItems => this.pageItems;
    }
}
