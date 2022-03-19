using FocalPoint.Components.Interface;
using FocalPoint.Data.API;
using FocalPoint.Utils;
using FocalPtMbl.MainMenu.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.Dispatching.ViewModels
{
    public class PickupTicketViewModel : CommonViewModel
    {
        private PickupTicket order;

        #region constructor
        public PickupTicketViewModel(PickupTicket pickupTicket)
        {
            PickupTicketEntityComponent = new PickupTicketEntityComponent();
            Init(pickupTicket);
            OpenPhoneDialerCommand = new Command<string>(async phoneNo => await OpenPhoneDialerTask(phoneNo));
            OpenMapApplicationCommand = new Command<string>(async address => await OpenMapApplicationTask(address));
            OpenEmailApplicationCommand = new Command<string>(async address => await OpenEmailApplicationTask(address));

        }


        #endregion

        #region Properties
        public IPickupTicketEntityComponent PickupTicketEntityComponent { get; set; }

        private ObservableCollection<PickupTicketItem> details = new ObservableCollection<PickupTicketItem>();
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

        internal void setSelectedDetail(int index)
        {
            SelectedDetail = Details[index];
        }

        public decimal Totals
        {
            get
            {
                return SelectedDetail.PuDtlCntQty + SelectedDetail.PuDtlOutQty + SelectedDetail.PuDtlSoldQty + SelectedDetail.PuDtlStolenQty + SelectedDetail.PuDtlLostQty + SelectedDetail.PuDtlDmgdQty;
            }
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
            get => order.CityStateZip == null || order.CityStateZip.Trim().Equals(",") ? "" : order.CityStateZip;
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
        public bool IsSelectPickupItemVisible
        {
            get => order.PuMobile;
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
        public int ToBeCounted
        {
            get
            {
                return Details.Count(x => x.PuDtlCounted == false);
            }
        }
        public PickupTicketItem OriginalDetailItem { get; set; }
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
        public decimal Counted
        {
            get => selectedDetail.PuDtlCntQty;
            set
            {
                selectedDetail.PuDtlCntQty = value;
                OnPropertyChanged(nameof(SelectedDetail));
            }
        }
        public decimal Qty
        {
            get => selectedDetail.PuDtlQty;
            set
            {
                selectedDetail.PuDtlQty = value;
                OnPropertyChanged(nameof(SelectedDetail));
            }
        }

        /*ImageSource _image = null;
        public ImageSource Image
        {
            get
            {
                var classId = string.Empty;

                if (Totals > 0)
                {
                    SelectedDetail.Checked = true;

                    if (Totals > SelectedDetail.PuDtlQty)
                        classId = "RedCheckedBox";
                    else if (Totals == SelectedDetail.PuDtlQty)
                        classId = "GreenCheckedBox";
                    else
                        classId = "YellowCheckedBox";
                }
                else
                {
                    classId = "UnCheckedBox";
                }


                if (_image == null || classId != _image.ClassId)
                {
                    var str = string.Format("FocalPoint.Images.{0}.png", classId);

                    _image = ImageSource.FromResource(str);
                    _image.ClassId = classId;
                }

                return _image;
            }
        }*/
        #endregion

        #region Methods
        public void Init(PickupTicket pickupTicket)
        {
            order = pickupTicket;
            if (pickupTicket.Details == null)
                Details = null;
            foreach (var item in pickupTicket.Details)
            {
                item.CurrentTotalCnt = item.PuDtlCntQty + item.PuDtlOutQty + item.PuDtlSoldQty + item.PuDtlStolenQty + item.PuDtlLostQty + item.PuDtlDmgdQty;
                item.ImageName = LoadImageString(item);
                Details.Add(item);
            }
            order = pickupTicket;
        }
        internal List<string> GetPopUpCount()
        {
            List<string> popUpList = new List<string>();
            if (SelectedDetail.OrderDtlMeterType != "N")
                popUpList.Add("Input Meter");
            if (SelectedDetail.OrderDtlFuelType != "N")
                popUpList.Add("Add Fuel");
            if (SelectedDetail.PuDtlCounted)
                popUpList.Add("Select Count");

            return popUpList;
        }

        internal bool PickupTicketCounted()
        {
            var requestObj = new PickupTicketCounted 
            {
                PuTNo = Convert.ToInt32(this.PuTNo),
                UTCDte = DateTime.Now 
            };
            return PickupTicketEntityComponent.PostPickupTicketCounted(requestObj).GetAwaiter().GetResult();
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

        internal void ClearQuantities()
        {
            SelectedDetail.PuDtlCntQty = 0;
            SelectedDetail.PuDtlOutQty = 0;
            SelectedDetail.PuDtlSoldQty = 0;
            SelectedDetail.PuDtlStolenQty = 0;
            SelectedDetail.PuDtlLostQty = 0;
            SelectedDetail.PuDtlDmgdQty = 0;
        }

        internal bool PickupTicketItemCount()
        {
            return PickupTicketEntityComponent.PostPickupTicketItemCount(SelectedDetail).GetAwaiter().GetResult();
        }

        internal void setPopupValue(string popupString, string result)
        {
            switch (popupString)
            {
                case "Input Meter":
                    SelectedDetail.PuDtlMeterIn = Convert.ToDouble(result);
                    break;
                case "Select Count":
                    SelectedDetail.LastCntOutQty = Convert.ToDecimal(result);
                    break;
                case "Add Fuel":
                    SelectedDetail.PuDtlTank = Convert.ToDouble(result);
                    break;
                default:
                    break;
            }
        }

        internal void UpdateSelectedDetails()
        {
            Counted = Qty;
            SelectedDetail.CurrentTotalCnt = Totals;
            //SelectedDetail.ImageName = GetImageString();
            SelectedDetail.UTCCountDte = DateTime.UtcNow;
            SelectedDetail.ImageName = "CheckedBox.png";
            //Details[0] = SelectedDetail;
        }

        internal double GetPopupType(string popupString)
        {
            double value = 0;
            switch (popupString)
            {
                case "Input Meter":
                    value = SelectedDetail.PuDtlMeterIn;
                    break;
                case "Select Count":
                    value = (double)SelectedDetail.LastCntOutQty;
                    break;
                case "Add Fuel":
                    value = (double)SelectedDetail.PuDtlTank;
                    break;
                default:
                    break;
            }
            return value;
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

        #region OpenPhoneDialer

        public ICommand OpenPhoneDialerCommand { get; }

        private async Task OpenPhoneDialerTask(string phoneNumber)
        {
            try
            {
                await Ultils.OpenPhoneDialer(phoneNumber);
            }
            catch (Exception exception)
            {
                //TODO: Log Error
            }
            finally
            {
            }
        }

        #endregion OpenPhoneDialer


        #region OpenMapApplication

        public ICommand OpenMapApplicationCommand { get; }

        private async Task OpenMapApplicationTask(string address)
        {
            try
            {
                await Ultils.OpenMapApplication(address);
            }
            catch (Exception exception)
            {
                //TODO: log error
            }
            finally
            {
            }
        }

        #endregion OpenMapApplication

        #region OpenEmailApplication

        public ICommand OpenEmailApplicationCommand { get; }

        private async Task OpenEmailApplicationTask(string emailAddress)
        {
            try
            {
                await Ultils.OpenEmailApplication(string.Empty, string.Empty, new List<string> { emailAddress });
            }
            catch (Exception exception)
            {
                //TODO: Log error
            }
            finally
            {
            }
        }

        #endregion OpenEmailApplication

        public override async Task<bool> SaveSignature()
        {
            SignatureInputDTO signatureInputDTO = new SignatureInputDTO
            {
                DocKind = (int)DocKinds.PickupTicket,
                RecordID = Convert.ToInt32(PuTNo),
                Stat = "R", //TODO confirm with Kirk
                Format = 4,//Base64 String of Image
                Signature = SignatureImage
            };
            return await GeneralComponent.SaveSignature(signatureInputDTO);
        }
        #endregion

        //private PickupTicketItem SelectedTicket = new PickupTicketItem();

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
    }
}
