using System;
using System.Collections.Generic;
using System.Text;
using FocalPtMbl.MainMenu.ViewModels;

namespace FocalPoint.Modules.FrontCounter.ViewModels.NewRental
{
    public class NewQuickRentalAddItemViewModel : ThemeBaseViewModel
    {
        public List<Items> RentalItems { get; set; }

        public NewQuickRentalAddItemViewModel()
        {
            RentalItems = new List<Items>
            {
                new Items
                {
                    AvailableQty = 1,
                    MinimumCharge = 4,
                    HourlyCharge = 5,
                    DailyCharge = 11,
                    MonthlyCharge = 34,
                    WeeklyCharge = 22
                },
                new Items
                {
                    AvailableQty = 1,
                    MinimumCharge = 4,
                    HourlyCharge = 5,
                    DailyCharge = 11,
                    MonthlyCharge = 34,
                    WeeklyCharge = 22
                },
                new Items
                {
                    AvailableQty = 1,
                    MinimumCharge = 4,
                    HourlyCharge = 5,
                    DailyCharge = 11,
                    MonthlyCharge = 34,
                    WeeklyCharge = 22
                },
                new Items
                {
                    AvailableQty = 1,
                    MinimumCharge = 4,
                    HourlyCharge = 5,
                    DailyCharge = 11,
                    MonthlyCharge = 34,
                    WeeklyCharge = 22
                },
                new Items
                {
                    AvailableQty = 1,
                    MinimumCharge = 4,
                    HourlyCharge = 5,
                    DailyCharge = 11,
                    MonthlyCharge = 34,
                    WeeklyCharge = 22
                },
                new Items
                {
                    AvailableQty = 1,
                    MinimumCharge = 4,
                    HourlyCharge = 5,
                    DailyCharge = 11,
                    MonthlyCharge = 34,
                    WeeklyCharge = 22
                }
            };
        }
    }

    public class Items
    {
        public int AvailableQty { get; set; }
        public int MinimumCharge { get; set; }
        public int HourlyCharge { get; set; }
        public int DailyCharge { get; set; }
        public int WeeklyCharge { get; set; }
        public int MonthlyCharge { get; set; }
    }
}
