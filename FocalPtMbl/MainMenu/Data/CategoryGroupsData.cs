using FocalPoint.MainMenu.Data;
using FocalPtMbl.MainMenu.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPtMbl.MainMenu.Data
{
    public class CategoryGroupsData
    {
       
        static List<PageItem> pageItems;
        public static List<PageItem> PageItems => pageItems;
        static CategoryGroupsData()
        {
            pageItems = new List<PageItem>()
            {
                new PageItem()
                {
                    Title = "Front Counter",
                    Description = "Front Counter",
                    Module = typeof(FrontCounter)
                },
                new PageItem()
                {
                    Title = "Dispatch",
                    Description = "Dispatch",
                    Module = typeof(DispatchingViewData)
                },
                new PageItem()
                {
                    Title = "Service Department",
                    Description = "Service Department",
                    Module = typeof(ServiceViewData)
                },
                 new PageItem()
                {
                    Title = "Customer Relations",
                    Description = "Customer Relations",
                    Module = typeof(CustomerRelationsViewData)
                },
                new PageItem()
                {
                    Title = "Inventory",
                    Description = "Inventory",
                    Module = typeof(InventoryViewData)
                },
                new PageItem()
                {
                    Title = "Dashboard",
                    Description = "Dashboard summarys",
                    Module = typeof(AdministrativeViewData)
                }
            };
            //if(/*testSecurity == */true)
            //{
            //    pageItems.Add(new PageItem()
            //    {
            //        Title = "TestSecurity",
            //        Description = "TestSecurity",
            //        Module = typeof(AdministrativeViewData)
            //    }
            //    );
            //}
        }
    }
}
