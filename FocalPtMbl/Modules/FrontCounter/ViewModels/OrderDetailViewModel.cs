using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using FocalPtMbl.MainMenu.Data;

namespace FocalPtMbl.Modules.Orders.ViewModels
{
    public class OrderDetailViewModel : BindableObject
    {
        /// <summary>
        /// 
        /// </summary>
        public OrderDtl Detail { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string ImageSource
        {
            get
            {
                if (this.Detail.OrderDtlType == "M")
                    return "merchandise_item_16";

                if (this.Detail.OrderDtlType == "R")
                    return "rental_item_16";

                return null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string Line1
        {
            get
            {
                return this.Detail.OrderDtlDscr.Trim();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string Line2
        {
            get
            {
                return string.Format("  {0}, Amount: {1}, Qty: {2}", this.Detail.OrderDtlDscr2.Trim(), string.Format("{0:C}", this.Detail.OrderDtlAmt), this.Detail.OrderDtlQty);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtl"></param>
        public OrderDetailViewModel(OrderDtl dtl)
        {
            this.Detail = dtl;
        }
    }
}