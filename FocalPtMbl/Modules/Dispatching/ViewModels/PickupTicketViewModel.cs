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
        public PickupTicket Ticket { get; set; }

        #region constructor
        public PickupTicketViewModel(PickupTicket pickupTicket)
        {
            PickupTicketEntityComponent = new PickupTicketEntityComponent();
            Init(pickupTicket);
            OpenPhoneDialerCommand = new Command<string>(async phoneNo => await OpenPhoneDialerTask(phoneNo));
            OpenMapApplicationCommand = new Command<string>(async address => await OpenMapApplicationTask(address));
            OpenEmailApplicationCommand = new Command<string>(async address => await OpenEmailApplicationTask(address));
        }

        internal Task<List<PickupTicketOrder>> PickupTicketOrder(int puTNo)
        {
            return PickupTicketEntityComponent.GetPickupTicketOrder(puTNo);
        }

        internal Task<bool> PickupTicketCreate(List<PickupTicketOrder> pickupTicketOrders)
        {
            return PickupTicketEntityComponent.PostPickupTicketCreate(pickupTicketOrders.ToListPickupTicketOrder());
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
                this.details = value;
                OnPropertyChanged(nameof(Details));
            }
        }
        ObservableCollection<PickupTicketOrder> _orders = new ObservableCollection<PickupTicketOrder>();
        public ObservableCollection<PickupTicketOrder> Orders
        {
            get
            {
                return _orders;
            }
            set
            {
                _orders = value;
                OnPropertyChanged("Orders");
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
            get => Ticket.Address1;
        }
        public string Address2
        {
            get => this.Ticket.Address2;
        }
        public string CityStateZip
        {
            get => Ticket.CityStateZip == null || Ticket.CityStateZip.Trim().Equals(",") ? "" : Ticket.CityStateZip;
        }

        public string CustomerName
        {
            get => this.Ticket.CustomerName;
        }

        public string OrderNumberT
        {
            get => this.Ticket.OrderNumberT;
        }

        public string Phone
        {
            get => this.Ticket.Phone;
        }
        public string Phone2
        {
            get => this.Ticket.Phone2;
        }
        public string PhoneType
        {
            get => this.Ticket.PhoneType;
        }
        public string PhoneType2
        {
            get => this.Ticket.PhoneType2;
        }
        public string PuCDte
        {
            get => this.Ticket.PuCDte.ToString();
        }
        public string PuContact
        {
            get => this.Ticket.PuContact;
        }
        public string PuEDte
        {
            get => this.Ticket.PuEDte.ToString();
        }
        public string PuEEmpid
        {
            get => this.Ticket.PuEEmpid;
        }
        public bool PuMobile
        {
            get => this.Ticket.PuMobile;
        }
        public bool IsSelectPickupItemVisible
        {
            //get => true;
            get => Ticket.PuMobile && Ticket.Details.Count == 0;
        }
        public string PuNote
        {
            get => this.Ticket.PuNote;
        }
        public string PuPDte
        {
            get => this.Ticket.PuPDte.ToString();
        }
        public string PuTNo
        {
            get => this.Ticket.PuTNo.ToString();
        }
        public int ToBeCounted
        {
            get
            {
                return Details.Count(x => x.PuDtlCounted == false);
            }
        }

        public PickupTicketItem SelectedDetail { get; set; }
        #endregion

        #region Methods
        public void Init(PickupTicket pickupTicket)
        {
            Ticket = pickupTicket;
            Details.Clear();
            if (pickupTicket.Details == null)
                Details = null;
            foreach (var item in pickupTicket.Details)
            {
                SelectedDetail = item;
                item.CurrentTotalCnt = item.PuDtlCntQty + item.PuDtlOutQty + item.PuDtlSoldQty + item.PuDtlStolenQty + item.PuDtlLostQty + item.PuDtlDmgdQty;
                item.ImageName = LoadImageString(item);
                Details.Add(item);
            }
            /*if (pickupTicket.Details != null && pickupTicket.Details.Count > 0)
                Details.Add(new PickupTicketItem() { IsItemVisible = false });*/
            Ticket = pickupTicket;
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

        async internal Task<bool> PickupTicketCounted()
        {
            var requestObj = new PickupTicketCounted
            {
                PuTNo = Convert.ToInt32(this.PuTNo),
                UTCDte = DateTime.Now
            };
            return await PickupTicketEntityComponent.PostPickupTicketCounted(requestObj);
        }

        internal string LoadImageString(PickupTicketItem item)
        {
            var str = string.Empty;
            var classId = string.Empty;
            if (Totals > 0)
            {
                SelectedDetail.Checked = true;

                if (Totals > item.PuDtlQty)
                    classId = "RedCheckedBox";
                else if (this.Totals == item.PuDtlQty)
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

        internal Task<bool> PickupTicketItemCount()
        {
            return PickupTicketEntityComponent.PostPickupTicketItemCount(SelectedDetail.ToPickupTicketItemDTO());
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

        internal void SelectedItemChecked(bool isChecked, bool isFromCountAdjustment = false)
        {
            try
            {
                bool isAtEndOfIndex = false;
                int selectedIndex = this.Details.IndexOf(SelectedDetail);
                if (Details.Count - 1 == selectedIndex)
                    isAtEndOfIndex = true;
                this.Details.Remove(SelectedDetail);
                //then change the selected detail to reflect those changes
                if (isChecked)
                {
                    if (!isFromCountAdjustment)
                        SelectedDetail.PuDtlCntQty = SelectedDetail.PuDtlQty;
                    SelectedDetail.ImageName = LoadImageString(SelectedDetail);
                    SelectedDetail.UTCCountDte = DateTime.UtcNow;
                    SelectedDetail.PuDtlCounted = true;
                }
                else
                {
                    SelectedDetail.PuDtlCntQty = 0;
                    SelectedDetail.PuDtlOutQty = 0;
                    SelectedDetail.PuDtlSoldQty = 0;
                    SelectedDetail.PuDtlStolenQty = 0;
                    SelectedDetail.PuDtlLostQty = 0;
                    SelectedDetail.PuDtlDmgdQty = 0;
                    SelectedDetail.ImageName = LoadImageString(SelectedDetail);
                    SelectedDetail.UTCCountDte = DateTime.UtcNow;
                    SelectedDetail.PuDtlCounted = false;
                }

                if (isAtEndOfIndex)
                    Details.Add(SelectedDetail);
                else
                    Details.Insert(selectedIndex, SelectedDetail);

                OnPropertyChanged(nameof(SelectedDetail));
                //OnPropertyChanged(nameof(Details));
                OnPropertyChanged(nameof(ToBeCounted));
            }
            catch (Exception ex)
            {
                //TODO: log error
            }
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

        async internal Task<bool> AttemptLock(string apiLocked)
        {
            try
            {
                return await PickupTicketEntityComponent.LockPickupTicket(Ticket.PuTNo.ToString(), apiLocked);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
        #endregion
    }
}
