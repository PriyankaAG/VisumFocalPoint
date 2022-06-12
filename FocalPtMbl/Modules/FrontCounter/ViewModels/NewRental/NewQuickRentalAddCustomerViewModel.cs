using System;
using System.Collections.Generic;
using System.Text;
using FocalPtMbl.MainMenu.ViewModels;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.FrontCounter.ViewModels.NewRental
{
    public class NewQuickRentalAddCustomerViewModel : ThemeBaseViewModel
    {
        public Customer CustomerToAdd { get; set; }
        public string[] ReferredByList { get; set; }
        public string[] CountryList { get; set; }
        public string[] StateList { get; set; }
        public string[] CityList { get; set; }
        public List<string> RentalList { get; set; }
        public NewQuickRentalAddCustomerViewModel()
        {
            ReferredByList = new string[] { "Select","AAAA", "BBBB", "CCCC" };
            CountryList = new string[] { "Select", "USA", "India" };
            StateList = new string[] { "Select", "Florida", "Georgia", "MaharashtraAAAABBBBCCCCDDDDMaharashtraAAAABBBBCCCCDDDD", "Uttar Pradesh" };
            CityList = new string[] { "Select", "TX", "NY", "Mumbai", "Pune" };

        }
    }
}
