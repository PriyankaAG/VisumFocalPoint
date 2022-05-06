namespace Visum.Services.Mobile.Entities
{
    public class Security
    {
        public enum Areas
        {
            MBL = 325,
            MBL_FC = 487,
            MBL_FC_Order = 488,
            MBL_FC_Return = 489,
            MBL_FC_Orders = 382,
            MBL_Dispatch = 383,
            MBL_Serv = 490,
            MBL_Serv_WorkOrders = 384,
            MBL_Cust = 326,
            MBL_Cust_Customers = 491,
            MBL_Cust_Customers_Balance = 327,
            MBL_Cust_Customers_EditNotes = 328,
            MBL_Inv = 492,
            MBL_Inv_Rental = 329,
            MBL_Inv_Rental_Revenue = 371,
            MBL_Inv_Vendor = 369,
            MBL_Inv_Vendor_Balance = 370,
            MBL_Dash = 330,
            MBL_Dash_CashDrawer = 372,
            MBL_Dash_RentalVal = 373,
            MBL_Dash_Revenue = 331,
            MBL_TimeClock = 436,
            MBL_TimeClock_BehalfOf = 437
        }

        public int Area { get; set; }
        public bool Allowed { get; set; }
    }
}
