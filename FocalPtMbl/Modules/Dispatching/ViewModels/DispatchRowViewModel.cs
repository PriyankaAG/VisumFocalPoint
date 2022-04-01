using System;
using System.Collections.Generic;
using System.Text;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.Dispatching.ViewModels
{
    public class DispatchRowViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public Dispatches Dispatch { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Line1
        {
            get
            {
                return this.Dispatch.DispatchSubject;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string Line2
        {
            get
            {
                if (this.Dispatch.DispatchType == 0)
                    return string.Format("  Misc Time: {0:t}", this.Dispatch.DispatchStartDte);

                if (this.Dispatch.DispatchType == 1)
                    return string.Format("  Delivery Time: {0:t}", this.Dispatch.DispatchStartDte);

                if (this.Dispatch.DispatchType == 2)
                    return string.Format("  Pickup Time: {0:t}", this.Dispatch.DispatchStartDte);

                return string.Format("  Time: {0:t}", this.Dispatch.DispatchStartDte);
            }
        }


        public bool HasLine3
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Dispatch.DispatchAddr1);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string Line3
        {
            get
            {
                return string.Format("  {0}", this.Dispatch.DispatchAddr1);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string Type
        {
            get
            {
                if (this.Dispatch.DispatchType == 0)
                    return "Misc";

                if (this.Dispatch.DispatchType == 1)
                    return "Delivery";

                if (this.Dispatch.DispatchType == 2)
                    return "Pickup";

                return string.Empty;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string TimeWindow
        {
            get
            {
                //var span = this.Dispatch.DispatchRangeEndDte.Subtract(this.Dispatch.DispatchRangeStartDte);
                return string.Format("{0:t} - {1:t}", this.Dispatch.DispatchRangeStartDte, this.Dispatch.DispatchRangeEndDte);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string AddressText
        {
            get
            {
                return string.Format("{0}{1}", this.Dispatch.DispatchAddr1, !string.IsNullOrWhiteSpace(this.Dispatch.DispatchAddr2) ? ", " + this.Dispatch.DispatchAddr2 : string.Empty);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string AddressText2
        {
            get
            {
                return string.Format("{0}, {1} {2}", this.Dispatch.DispatchCity, this.Dispatch.DispatchState, this.Dispatch.DispatchZip);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string OriginNotesHeader
        {
            get
            {
                var lower = this.Dispatch.DispatchOrigin?.ToLower();

                if (lower == "FC")
                    return "Order Title:  ";

                if (lower == "PT")
                    return "Pickup Ticket:  ";

                if (lower == "WO")
                    return "Work Order Notes:  ";

                return string.Empty;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dis"></param>
        public DispatchRowViewModel(Dispatches dis)
        {
            this.Dispatch = dis;
        }
    }
}
