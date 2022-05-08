using FocalPoint.Modules.ServiceDepartment.Views;
using FocalPtMbl.MainMenu.Data;
using FocalPtMbl.MainMenu.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FocalPoint.MainMenu.Data
{
    public class ServiceViewData : IPageData
    {

        readonly List<PageItem> pageItems;
        readonly INestedTabView nestedTabView;
        readonly ITabPages tabPages;

        public ServiceViewData()
        {
            this.nestedTabView = DependencyService.Get<INestedTabView>();
            this.tabPages = DependencyService.Get<ITabPages>();
            this.pageItems = new List<PageItem>()
            {
                 new PageItem()
                {
                    Title = "View Work Orders",
                    ControlsPageTitle = "Work Orders",
                    PageTitle = "Work Orders",
                    Description = "View Work Orders",
                    Module = typeof(WorkOrderFormView),
                    Icon = "Work_Order_32.png",
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
        public string Title => "Service Counter";
        public List<PageItem> PageItems => this.pageItems;
    }
}