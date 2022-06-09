using System;
using System.Collections.Generic;
using System.Text;
using FocalPtMbl.MainMenu.ViewModels;

namespace FocalPoint.Modules.FrontCounter.ViewModels.NewRental
{
    public class NewQuickRentalAddCustomerViewModel : ThemeBaseViewModel
    {
        public List<string> RentalList { get; set; }
        public NewQuickRentalAddCustomerViewModel()
        {
            RentalList = new List<string>()
            {
                "Rentals",
                "Merchandise",
                "Rate Table",
                "Rental Saleable"
            };
        }
    }
}
