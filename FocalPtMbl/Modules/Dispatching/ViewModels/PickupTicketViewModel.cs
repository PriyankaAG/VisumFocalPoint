using FocalPoint.Components.Interface;
using FocalPoint.Utils;
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
        #region constructor
        public PickupTicketViewModel(PickupTicket pickupTicket)
        {
            PickupTicketEntityComponent = new PickupTicketEntityComponent();
            ticket = pickupTicket;
            Init();
            //Init(pickupTicket);
            SetEntityDetails(DocKinds.PickupTicket, pickupTicket.PuTNo, "R");
            OpenPhoneDialerCommand = new Command<string>(async phoneNo => await OpenPhoneDialerTask(phoneNo));
            OpenMapApplicationCommand = new Command<string>(async address => await OpenMapApplicationTask(address));
            OpenEmailApplicationCommand = new Command<string>(async address => await OpenEmailApplicationTask(address));
        }
        #endregion

        #region Properties
        private PickupTicket ticket = null;
        public PickupTicket Ticket
        {
            get
            {
                return ticket;
            }
            set
            {
                ticket = value;
                Init();
            }
        }
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
            SelectedItem = Details[index];
        }
        public decimal Totals
        {
            get
            {
                return SelectedItem.PuDtlCntQty + SelectedItem.PuDtlOutQty + SelectedItem.PuDtlSoldQty + SelectedItem.PuDtlStolenQty + SelectedItem.PuDtlLostQty + SelectedItem.PuDtlDmgdQty;
            }
        }
        public decimal Counted 
        {
            get { return SelectedItem.PuDtlCntQty + SelectedItem.PuDtlDmgdQty; }
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
            get => Utils.Utils.ConvertPhone(Ticket.Phone);
        }
        public string Phone2
        {
            get => Utils.Utils.ConvertPhone(Ticket.Phone2);
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
            get => this.Ticket.PuCDte.ToFormattedDate();
        }
        public string PuContact
        {
            get => this.Ticket.PuContact;
        }
        public string PuEDte
        {
            get => this.Ticket.PuEDte.ToFormattedDate();
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
            get => this.Ticket.PuPDte.ToFormattedDate();
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

        public PickupTicketItem SelectedItem { get; set; }
        #endregion

        #region Methods

        private void Init()
        {
            UpdateItems();

            OnPropertyChanged(nameof(Ticket));
            OnPropertyChanged(nameof(Details));
            OnPropertyChanged(nameof(ToBeCounted));
        }

        private void UpdateItems()
        {
            if (ticket.Details == null || ticket.Details.Count() == 0)
            {
                //Assume there have been counted/completed - confirm with Kirk
                Details.Clear();
                return;
            }
            foreach (var item in ticket.Details)
            {
                this.SelectedItem = item;
                item.CurrentTotalCnt = item.TotalCounted  = GetTotalCount(item);
                item.ImageName = LoadImageString(item);

                var existing = Details.FirstOrDefault(x => x.PuDtlTNo == item.PuDtlTNo);
                if (existing == null)
                {
                    //Add it
                    Details.Add(item);
                    continue;
                }

                if (item.UTCCountDte > existing.UTCCountDte)
                {
                    int selectedIndex = Details.IndexOf(this.SelectedItem);
                    //Always overwrite - even if modified locally
                    Details.Remove(existing);
                    Details.Insert(selectedIndex, item);
                    continue;
                }
            }
            var tmp = ticket.Details;
            for (int i = Details.Count() - 1; i >= 0; i--)
            {
                if (tmp.Exists(x => x.PuDtlTNo == Details[i].PuDtlTNo))
                    continue;

                Details.RemoveAt(i);
            }
        }

        internal void SetTotalCount(PickupTicketItem item)
        {
            item.CurrentTotalCnt = item.PuDtlCntQty + item.PuDtlOutQty + item.PuDtlSoldQty + item.PuDtlStolenQty + item.PuDtlLostQty + item.PuDtlDmgdQty;
        }

        public void Init(PickupTicket pickupTicket)
        {
            Ticket = pickupTicket;
            Details.Clear();
            if (pickupTicket.Details == null)
                Details = null;
            foreach (var item in pickupTicket.Details)
            {
                this.SelectedItem = item;
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
            if (SelectedItem.OrderDtlMeterType != "N")
                popUpList.Add("Input Meter");
            if (SelectedItem.OrderDtlFuelType != "N")
                popUpList.Add("Add Fuel");
            if (SelectedItem.PuDtlCounted)
                popUpList.Add("Select Count");

            return popUpList;
        }
        internal string LoadImageString(PickupTicketItem item)
        {
            var str = string.Empty;
            var classId = string.Empty;
            if (Totals > 0)
            {
                this.SelectedItem.Checked = true;

                if (Totals > item.PuDtlQty)
                    classId = "RedCheckedBox";
                else if (this.Totals == item.PuDtlQty)
                    classId = "GreenCheckedBox";
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
        internal void setPopupValue(string popupString, string result)
        {
            switch (popupString)
            {
                case "Input Meter":
                    SelectedItem.PuDtlMeterIn = Convert.ToDouble(result);
                    break;
                case "Select Count":
                    SelectedItem.LastCntOutQty = Convert.ToDecimal(result);
                    break;
                case "Add Fuel":
                    SelectedItem.PuDtlTank = Convert.ToDouble(result);
                    break;
                default:
                    break;
            }
        }
        internal async Task UpdateItem()
        {
            PickupTicketItem updatedItem = await UpdateItem(SelectedItem);
            if (updatedItem != null)
            {
                bool isAtEndOfIndex = false;
                int selectedIndex = Details.IndexOf(SelectedItem);
                if (Details.Count - 1 == selectedIndex)
                    isAtEndOfIndex = true;
                Details.Remove(SelectedItem);
                if (isAtEndOfIndex)
                    Details.Add(SelectedItem);
                else
                    Details.Insert(selectedIndex, SelectedItem);
            }
        }


        internal double GetPopupType(string popupString)
        {
            double value = 0;
            switch (popupString)
            {
                case "Input Meter":
                    value = SelectedItem.PuDtlMeterIn;
                    break;
                case "Select Count":
                    value = (double)SelectedItem.LastCntOutQty;
                    break;
                case "Add Fuel":
                    value = (double)SelectedItem.PuDtlTank;
                    break;
                default:
                    break;
            }
            return value;
        }
        internal void SelectedItemChecked(PickupTicketItem ticketItem, bool isChecked, bool isFromCountAdjustment = false)
        {
            try
            {
                bool isAtEndOfIndex = false;
                var item = Details.FirstOrDefault(x => x.PuDtlTNo == ticketItem.PuDtlTNo);
                int selectedIndex = this.Details.IndexOf(item);

                if (Details.Count - 1 == selectedIndex)
                    isAtEndOfIndex = true;
                this.Details.Remove(item);

                SelectedItem = ticketItem;
                SelectedItem.TotalCounted = GetTotalCount(SelectedItem);
                //then change the selected detail to reflect those changes
                if (isChecked)
                {
                    if (!isFromCountAdjustment)
                        SelectedItem.PuDtlCntQty = SelectedItem.PuDtlQty;
                    SelectedItem.ImageName = LoadImageString(item);
                    SelectedItem.UTCCountDte = DateTime.UtcNow;
                    SelectedItem.PuDtlCounted = true;
                }
                else
                {
                    SelectedItem.PuDtlCntQty = 0;
                    SelectedItem.PuDtlOutQty = 0;
                    SelectedItem.PuDtlSoldQty = 0;
                    SelectedItem.PuDtlStolenQty = 0;
                    SelectedItem.PuDtlLostQty = 0;
                    SelectedItem.PuDtlDmgdQty = 0;
                    SelectedItem.ImageName = LoadImageString(SelectedItem);
                    SelectedItem.UTCCountDte = DateTime.UtcNow;
                    SelectedItem.PuDtlCounted = false;
                }

                if (isAtEndOfIndex)
                    Details.Add(SelectedItem);
                else
                    Details.Insert(selectedIndex, SelectedItem);

                OnPropertyChanged(nameof(SelectedItem));
                OnPropertyChanged(nameof(Details));
                OnPropertyChanged(nameof(ToBeCounted));
            }
            catch (Exception ex)
            {
                //TODO: log error
            }
        }

        internal async Task RefreshTicket()
        {
            var PickupTicketEntityComponent = new PickupTicketEntityComponent();
            var detailedTicket = await PickupTicketEntityComponent.GetPickupTicket(SelectedItem.PuTNo.ToString());
            Init(detailedTicket);
        }

        #region OpenPhoneDialer

        public ICommand OpenPhoneDialerCommand { get; }

        private async Task OpenPhoneDialerTask(string phoneNumber)
        {
            try
            {
                await Utils.Utils.OpenPhoneDialer(phoneNumber);
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
                await Utils.Utils.OpenMapApplication(address);
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
                await Utils.Utils.OpenEmailApplication(string.Empty, string.Empty, new List<string> { emailAddress });
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
        }
        internal Task<PickupTicket> GetTicketInfo(string PuTNo)
        {
            try
            {
                return PickupTicketEntityComponent.GetPickupTicket(PuTNo);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        internal Task<List<PickupTicketOrder>> PickupTicketOrder(int puTNo)
        {
            try
            {
                return PickupTicketEntityComponent.GetPickupTicketOrder(puTNo);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        internal Task<bool> PickupTicketCreate(List<PickupTicketOrder> pickupTicketOrders)
        {
            return PickupTicketEntityComponent.PostPickupTicketCreate(pickupTicketOrders);
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
        internal Task<bool> PickupTicketItemCount(PickupTicketItem selectedDetail)
        {
            return PickupTicketEntityComponent.PostPickupTicketItemCount(selectedDetail);
        }
        internal async Task<PickupTicketItem> UpdateItem(PickupTicketItem item)
        {
            try
            {
                var updatedItem = await PickupTicketEntityComponent.GetPickupTicketItem(item.PuDtlTNo);
                if (updatedItem == null)
                    return null;

                return updatedItem;
            }
            catch { }

            return item;
        }
        internal decimal GetTotalCount(PickupTicketItem item)
        {
            return item.PuDtlCntQty + item.PuDtlOutQty + item.PuDtlSoldQty + item.PuDtlStolenQty + item.PuDtlLostQty + item.PuDtlDmgdQty;

        }
        #endregion
    }
}
