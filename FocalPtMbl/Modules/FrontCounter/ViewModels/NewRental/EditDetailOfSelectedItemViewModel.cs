using FocalPtMbl.MainMenu.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.FrontCounter.ViewModels.NewRental
{
    public class EditDetailOfSelectedItemViewModel : ThemeBaseViewModel
    {
        public OrderDtl OrderDetails { get; set; }

        public EditDetailOfSelectedItemViewModel(OrderDtl ordDetails)
        {
            OrderDetails = ordDetails;
        }

        public string ItemName
        {
            get
            {
                return OrderDetails.OrderDtlDscr2;
            }
        }
        public decimal Quantity
        {
            get
            {
                return OrderDetails.OrderDtlQty;
            }
            set 
            {
                OrderDetails.OrderDtlQty = value;
            }
        }
        public decimal Discount
        {
            get
            {
                return OrderDetails.OrderDtlDiscount;
            }
            set
            {
                OrderDetails.OrderDtlDiscount = value;
            }
        }
        public bool Taxable
        {
            get
            {
                return OrderDetails.OrderDtlTaxable;
            }
            set
            {
                OrderDetails.OrderDtlTaxable = value;
            }
        }
        public bool OverridePrice
        {
            get
            {
                return OrderDetails.OrderDtlORide;
            }
            set
            {
                OrderDetails.OrderDtlORide = value;
            }
        }
    }
}
