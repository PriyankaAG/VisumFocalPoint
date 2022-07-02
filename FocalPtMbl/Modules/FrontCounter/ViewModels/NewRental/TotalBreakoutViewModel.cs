using FocalPtMbl.MainMenu.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.FrontCounter.ViewModels.NewRental
{
    public class TotalBreakoutViewModel : ThemeBaseViewModel
    {
        public TotalBreakoutViewModel(Order order)
        {
            OrdersObj = order;
            OnPropertyChanged(nameof(OrdersObj));
        }

        Order ordersObj;
        public Order OrdersObj
        {
            get => this.ordersObj;
            private set
            {
                this.ordersObj = value;
                OnPropertyChanged(nameof(OrdersObj));
            }
        }

    }
}
