using FocalPoint.Modules.Customer_Relations.Views;
using FocalPoint.Modules.CustomerRelations.Views;
using FocalPtMbl.MainMenu.Data;
using FocalPtMbl.MainMenu.Models;
using FocalPtMbl.Modules.Orders.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
namespace FocalPoint.MainMenu.Data
{
    public class CustomerRelationsViewData : IPageData
    {

        readonly List<PageItem> pageItems;
        readonly INestedTabView nestedTabView;
        readonly ITabPages tabPages;

        public CustomerRelationsViewData()
        {
            this.nestedTabView = DependencyService.Get<INestedTabView>();
            this.tabPages = DependencyService.Get<ITabPages>();
            this.pageItems = new List<PageItem>()
            {
                new PageItem()
                {
                    Title = "Customers",
                    ControlsPageTitle = "Customers",
                    PageTitle = "Customers",
                    Description = "Search for Customers",
                    Module = typeof(CustomerSimpleView),
                    Icon = "our_customers_b_32.png",
                    IconOverlayText = IconOverlayText.None
                }
                //new PageItem()
                //{
                //    Title = "Prospects",
                //    ControlsPageTitle = "",
                //    PageTitle = "",
                //    Description = "",
                //    Module = typeof(ProspectsView),
                //    Icon = "grid_firstlook",
                //    IconOverlayText = IconOverlayText.None
                //},
                //new PageItem()
                //{
                //    Title = "Quick Add",
                //    ControlsPageTitle = "",
                //    PageTitle = "",
                //    Description = "",
                //    Module = typeof(CustomerFormView),
                //    Icon = "our_customers_b_add_32.png",
                //    IconOverlayText = IconOverlayText.None
                //}
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
        public string Title => "Inventory";
        public List<PageItem> PageItems => this.pageItems;
    }
}
