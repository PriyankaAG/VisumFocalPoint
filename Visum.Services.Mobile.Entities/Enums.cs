using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Visum.Services.Mobile.Entities
{
    public enum OrderQuestions
    {
        ChangeCurrentDateLength = 1000,
        // Show Message Question and return True/False
        RentalOverbooking = 1001,
        // Call OrderOverbookings If OrderSettings.OverbookingAllowed Ask to Continue return True/False, If Not allowed show only
        MeterOutUpdated = 1002,
        // call GET OrderMeterOut and show frmOrderMeterOut and allow meter entry, pass back to PUT OrderMeterOut Answer = True/False 
        ResetBinsReturn = 1003,
        // Show Message Question and return True/False
        ResetBinsVoid = 1004,
        // Show Message Question and return True/False
        MerchAddSerial = 1005,
        // Show Message and return no numbers for not assigning else return number to assign equal to qty
        KitsVoid = 1006,
        // Show Message Question and return True/False
        LengthMoveDueDate = 1007,
        // Show Message Question and return True/False
        ConvertReservation = 1008,
        // Show Message Question and return True/False
        RentalSellInstead = 1009,
        // Show Message Question and return True/False
        RentalNotFromStore = 1010,
        // Show Message Question and return True/False
        RentalGeneric = 1011,
        // Show Message Question and return True/False
        RentalMeter = 1012,
        // Get Meter Value in Double 3 decimal places
        RentalPackage = 1013,
        // See Rentals.frmRateTablePackage, display rate and use selected package as short
        JobNoRequired = 1014,
        // call GET JobSites and return selected value
        PONoRequired = 1015,
        // Show Message and return string entry
        SalesNoRequired = 1016,
        // call GET SalesPeople and return selected value
        ConvertSalesOnly = 1017,
        // Show Message Question and return True/False
        CreateInvoice = 1018,
        // Show Message Question and return True/False
        OpenCredit = 1019,
        // Show Message Question and return True/False
        JobSiteRequired = 1020
        // call JobSites and return selected value
    }

    public enum RateTypes
    {
        [Description("None")]
        None = 0,
        [Description("Minimum")]
        Minimum = 1,
        [Description("Minimum 2")]
        Minimum2 = 2,
        [Description("Minimum 3")]
        Minimum3 = 3,
        [Description("Hourly")]
        Hourly = 4,
        [Description("Daily")]
        Daily = 5,
        [Description("Multi-Day")]
        MultiDay = 12,
        [Description("Weekly")]
        Weekly = 6,
        [Description("Monthly")]
        Monthly = 7,
        [Description("Over-Night")]
        OverNight = 8,
        [Description("Sat-Mon")]
        SatToMon = 9,
        [Description("Fri-Mon")]
        FriToMon = 10,
        [Description("Open-Close")]
        OpenToClose = 11
    }
}
