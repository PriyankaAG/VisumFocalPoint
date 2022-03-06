using FocalPtMbl.MainMenu.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.Dispatching.ViewModels
{
    public class PickupTicketViewModel : ThemeBaseViewModel
    {
        readonly PickupTicket order;
        //private PickupTicketItem SelectedTicket = new PickupTicketItem();
        private ObservableCollection<PickupTicketItem> details = new ObservableCollection<PickupTicketItem>();
        HttpClient clientHttp;
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        public ObservableCollection<PickupTicketItem> Details
        {
            get => this.details;
            set
            {
                if (details.Count < 0)
                    this.details = value;
                else
                {
                    this.details.Clear();
                    this.details = value;
                }
                OnPropertyChanged(nameof(Details));
            }

        }
        public decimal Totals
        {
            get
            {
                return SelectedDetail.PuDtlCntQty + SelectedDetail.PuDtlOutQty + SelectedDetail.PuDtlSoldQty + SelectedDetail.PuDtlStolenQty + SelectedDetail.PuDtlLostQty + SelectedDetail.PuDtlDmgdQty;
            }
        }

        internal List<string> GetPopUpCount()
        {
            List<string> popUpList = new List<string>();
            if (SelectedDetail.PuDtlMeterIn != null && SelectedDetail.OrderDtlMeterType != "N")
                popUpList.Add("Input Meter");
            if (SelectedDetail.OrderDtlFuelType != "N")
                popUpList.Add("Add Fuel");
            if (SelectedDetail.PuDtlCounted)
                popUpList.Add("Select Count");

                return popUpList;
        }

        //ImageSource _image = null;
        //public ImageSource Image
        //{
        //    get
        //    {
        //        var str = string.Format("FocalPoint.Images.{0}.png", classId);

        //        _image = ImageSource.FromResource(str);
        //        _image.ClassId = classId;

        //        return _image;
        //    }
        //}
        private ObservableCollection<PickupTicketItem> selectedDetails = new ObservableCollection<PickupTicketItem>();
        public ObservableCollection<PickupTicketItem> SelectedDetails
        {
            get => this.details;
            set
            {
                if (details.Count < 0)
                    this.details = value;
                else
                {
                    this.details.Clear();
                    this.details = value;
                }
                OnPropertyChanged(nameof(Details));
            }

        }

        internal string GetImageString()
        {
            var str = string.Empty;
            var classId = string.Empty;
            if (Totals > 0)
            {
                //SelectedTicket.Checked = true;

                if (Totals > SelectedDetail.PuDtlQty)
                    classId = "RedCheckedBox";
                else if (this.Totals == SelectedDetail.PuDtlQty)
                    classId = "CheckedBox";
                else
                    classId = "YellowCheckedBox";
            }
            else
            {
                classId = "UnCheckedBox";
            }


                str = string.Format("{0}.png", classId);
            return str;
            }
        internal string LoadImageString(PickupTicketItem pickupTicketItem)
        {
            var str = string.Empty;
            var classId = string.Empty;
            if (Totals > 0)
            {
                //SelectedTicket.Checked = true;

                if (Totals > pickupTicketItem.PuDtlQty)
                    classId = "RedCheckedBox";
                else if (this.Totals == pickupTicketItem.PuDtlQty)
                    classId = "CheckedBox";
                else
                    classId = "YellowCheckedBox";
            }
            else
            {
                classId = "UnCheckedBox";
            }


            str = string.Format("{0}.png", classId);
            return str;
        }

        internal void PickupTicketItemToSubmit()
        {
            //if (SelectedPickupTicketItems.Contains(SelectedDetail))
            //    SelectedPickupTicketItems.Remove(SelectedDetail);
            //else
            //    SelectedPickupTicketItems.Add(SelectedDetail);
        }
        

        public PickupTicketViewModel(PickupTicket pickupTicket)
        {
            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();
            order = pickupTicket;
            if (pickupTicket.Details != null)
            {
                foreach (var dtl in pickupTicket.Details)
                {
                    dtl.CurrentTotalCnt = dtl.PuDtlCntQty + dtl.PuDtlOutQty + dtl.PuDtlSoldQty + dtl.PuDtlStolenQty + dtl.PuDtlLostQty + dtl.PuDtlDmgdQty;
                    dtl.ImageName = LoadImageString(dtl);
                    this.Details.Add(dtl);
                }
                this.order = pickupTicket;
            }
        }

        internal void SelectedItemChecked(bool isChecked)
        {
            bool isAtEndOfIndex = false; 
            int selectedIndex = this.Details.IndexOf(SelectedDetail);
            if (Details.Count - 1 == selectedIndex)
                isAtEndOfIndex = true;
            this.Details.Remove(SelectedDetail);
            //then change the selected detail to reflect those changes
            if (!isChecked)
            {
                SelectedDetail.PuDtlCntQty = SelectedDetail.PuDtlQty;
                SelectedDetail.ImageName = GetImageString();
            }
            else
            {
                SelectedDetail.PuDtlCntQty = 0;
                SelectedDetail.PuDtlOutQty = 0;
                SelectedDetail.PuDtlSoldQty = 0;
                SelectedDetail.PuDtlStolenQty = 0;
                SelectedDetail.PuDtlLostQty = 0;
                SelectedDetail.PuDtlDmgdQty = 0;
                SelectedDetail.ImageName = GetImageString();
            }
            if (isAtEndOfIndex)
            {
                this.Details.Add(SelectedDetail);
                OnPropertyChanged(nameof(Details));
                //this.Details.
            }
            else
                this.Details.Insert(selectedIndex, SelectedDetail);
            //this.Details.
        }

        internal void SelectedItemEdit()
        {
            //throw new NotImplementedException();
        }

        public string Address1
        {
            get => this.order.Address1;
        }
        public string Address2
        {
            get => this.order.Address2;
        }
        public string CityStateZip
        {
            get => this.order.CityStateZip;
        }

        public string CustomerName
        {
            get => this.order.CustomerName;
        }

        public string OrderNumberT
        {
            get => this.order.OrderNumberT;
        }

        public string Phone
        {
            get => this.order.Phone;
        }
        public string Phone2
        {
            get => this.order.Phone2;
        }
        public string PhoneType
        {
            get => this.order.PhoneType;
        }
        public string PhoneType2
        {
            get => this.order.PhoneType2;
        }
        public string PuCDte
        {
            get => this.order.PuCDte.ToString();
        }
        public string PuContact
        {
            get => this.order.PuContact;
        }
        public string PuEDte
        {
            get => this.order.PuEDte.ToString();
        }
        public string PuEEmpid
        {
            get => this.order.PuEEmpid;
        }
        public string PuMobile
        {
            get => this.order.PuMobile.ToString();
        }
        public string PuNote
        {
            get => this.order.PuNote;
        }
        public string PuPDte
        {
            get => this.order.PuPDte.ToString();
        }
        public string PuTNo
        {
            get => this.order.PuTNo.ToString();
        }
        private PickupTicketItem selectedDetail = new PickupTicketItem();
        public PickupTicketItem SelectedDetail
        {
            get => this.selectedDetail;
            set
            {
                this.selectedDetail = value;
                OnPropertyChanged(nameof(SelectedDetail));
            }

        }
    }
}
