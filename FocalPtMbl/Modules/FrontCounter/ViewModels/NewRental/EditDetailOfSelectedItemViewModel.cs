﻿using FocalPoint.Components.EntityComponents;
using FocalPoint.Components.Interface;
using FocalPtMbl.MainMenu.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.FrontCounter.ViewModels.NewRental
{
    public class EditDetailOfSelectedItemViewModel : ThemeBaseViewModel
    {
        public INewQuickRentalEntityComponent NewQuickRentalEntityComponent { get; set; }
        public OrderDtlUpdate OrderDetailUpdate { get; set; }
        public OrderDtl OrderDetails { get; set; }
        public Order CurrentOrder { get; set; }
        public ICommand DiscountEnteredCommand { get; set; }
        public bool IsPageLoading { get; set; }

        public EditDetailOfSelectedItemViewModel(OrderDtlUpdate ordDetailsUpdate, Order cOrder, ICommand cmdDiscount)
        {
            OrderDetailUpdate = ordDetailsUpdate;
            OrderDetails = OrderDetailUpdate.Detail;
            CurrentOrder = cOrder;
            NewQuickRentalEntityComponent = new NewQuickRentalEntityComponent();
            DiscountEnteredCommand = cmdDiscount;
        }

        public string ItemName
        {
            get
            {
                return OrderDetails.OrderDtlDscr;
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
                OnPropertyChanged("Quantity");
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
                OnPropertyChanged("Discount");
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
                OnPropertyChanged("Taxable");
            }
        }
        public bool IsOverridePrice
        {
            get
            {
                return OrderDetails.OrderDtlORide;
            }
            set
            {
                OrderDetails.OrderDtlORide = value;
                OnPropertyChanged("IsOverridePrice");
            }
        }
        public decimal OverridePrice
        {
            get
            {
                return OrderDetails.OrderDtlAmt;
            }
            set
            {
                OrderDetails.OrderDtlAmt = value;
                OnPropertyChanged("OverridePrice");
            }
        }

        internal async Task<OrderUpdate> UpdateOrderDetail()
        {
            Indicator = true;
            OrderUpdate responseOrderUpdateDetail = null;
            try
            {
                responseOrderUpdateDetail = await NewQuickRentalEntityComponent.UpdateOrderDetail(OrderDetailUpdate);
                if (responseOrderUpdateDetail != null && responseOrderUpdateDetail.Order != null)
                {
                    CurrentOrder = responseOrderUpdateDetail.Order;
                    OrderDetails = CurrentOrder.OrderDtls.First(p => p.OrderNo == OrderDetails.OrderNo
                    && p.OrderDtlLine == OrderDetails.OrderDtlLine
                    && p.OrderDtlNewLine == OrderDetails.OrderDtlNewLine);

                    if (responseOrderUpdateDetail.Answers == null || responseOrderUpdateDetail.Answers.Count == 0)
                    {
                        OrderDetailUpdate.Answers.Clear();

                        _ = Task.Delay(500).ContinueWith((a) =>
                        {
                            Indicator = false;
                        });
                    }
                }
            }
            catch (Exception)
            {
                Indicator = false;
            }
            return responseOrderUpdateDetail;
        }
    }
}
