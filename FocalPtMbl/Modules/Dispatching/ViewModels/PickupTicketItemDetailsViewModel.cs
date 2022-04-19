using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using FocalPoint.Components.Interface;
using FocalPoint.Utils;
using FocalPtMbl.MainMenu.ViewModels;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.Dispatching.ViewModels
{
    public class PickupTicketItemDetailsViewModel : CommonViewModel
    {
        public IPickupTicketEntityComponent PickupTicketEntityComponent { get; set; }
        public PickupTicketItemDetailsViewModel(PickupTicketItem pickupTicketItem)
        {
            PickupTicketEntityComponent = new PickupTicketEntityComponent();
             selectedDetail = pickupTicketItem;
            OriginalPickupItem = (PickupTicketItem)pickupTicketItem.Clone();
            Refresh();
        }

        #region Properties
        public PickupTicketItem OriginalPickupItem;
        private PickupTicketItem selectedDetail;
        public PickupTicketItem SelectedDetail
        {
            get => selectedDetail;
            set
            {
                selectedDetail = value;
                Refresh();
            }

        }
        public decimal Totals
        {
            get
            {
                return Counted + StillOut + Sold + Stolen + Lost + Damaged;
            }
        }
        public decimal ToBePickedUp
        {
            get => selectedDetail.PuDtlQty;
        }
        public decimal Counted
        {
            get => selectedDetail.PuDtlCntQty;
            set
            {
                selectedDetail.PuDtlCntQty = value;
                Refresh();
            }
        }
        public decimal StillOut
        {
            get => selectedDetail.PuDtlOutQty;
            set
            {
                selectedDetail.PuDtlOutQty = value;
                Refresh();
            }
        }
        public decimal Sold
        {
            get => selectedDetail.PuDtlSoldQty;
            set
            {
                selectedDetail.PuDtlSoldQty = value;
                Refresh();
            }
        }
        public decimal Stolen
        {
            get => selectedDetail.PuDtlStolenQty;
            set
            {
                selectedDetail.PuDtlStolenQty = value;
                Refresh();
            }
        }
        public decimal Lost
        {
            get => selectedDetail.PuDtlLostQty;
            set
            {
                selectedDetail.PuDtlLostQty = value;
                Refresh();
            }
        }
        public decimal Damaged
        {
            get => selectedDetail.PuDtlDmgdQty;
            set
            {
                selectedDetail.PuDtlDmgdQty = value;
                Refresh();
            }
        }
        public string UserId
        {
            get => selectedDetail.LastCntEmpID;
        }
        public DateTime? Time
        {
            get => selectedDetail.LastCntDte;
        }
        public decimal LastCounted
        {
            get => selectedDetail.LastCntQty;
        }
        public decimal LastStillOut
        {
            get => selectedDetail.LastCntOutQty;
        }
        public decimal LastSold
        {
            get => selectedDetail.LastCntSoldQty;
        }
        public decimal LastStolen
        {
            get => selectedDetail.LastCntStolenQty;
        }
        public decimal LastLost
        {
            get => selectedDetail.LastCntLostQty;
        }
        public decimal LastDamaged
        {
            get => selectedDetail.LastCntDmgdQty;
        }
        #endregion

        #region Methods
        internal bool? IsCountedGreaterThanToBeCounted()
        {
            return Totals > ToBePickedUp ? true : Totals < ToBePickedUp ? false : (bool?)null;
        }
        internal Task<bool> PickupTicketItemCount()
        {
            return PickupTicketEntityComponent.PostPickupTicketItemCount(SelectedDetail);
        }
        internal void Refresh()
        {
            OnPropertyChanged(nameof(SelectedDetail));
            OnPropertyChanged(nameof(Totals));
        }
        internal void UpdateTicket(bool isSuccess)
        {
            MessagingCenter.Send(this, "ItemDetails", new Tuple<PickupTicketItem, bool>(SelectedDetail, isSuccess));
        }
        #endregion
    }
}
