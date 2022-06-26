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
        public TotalBreakoutViewModel(OrderTotals totals)
        {
            Totals = new ObservableCollection<OrderTotals>();
            Totals.Add(totals);
            OnPropertyChanged(nameof(Totals));
        }

        ObservableCollection<OrderTotals> totals;
        public ObservableCollection<OrderTotals> Totals
        {
            get => this.totals;
            private set
            {
                this.totals = value;
                OnPropertyChanged(nameof(Totals));
            }
        }

    }
}
