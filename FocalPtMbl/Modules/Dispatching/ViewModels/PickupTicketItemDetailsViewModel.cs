using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using FocalPtMbl.MainMenu.ViewModels;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.Dispatching.ViewModels
{
    public class PickupTicketItemDetailsViewModel : ThemeBaseViewModel
    {
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
        [DataMember]
        public decimal PuDtlQty { get; set; }
        public decimal Totals
        {
            get
            {
                return SelectedDetail.PuDtlCntQty + SelectedDetail.PuDtlOutQty + SelectedDetail.PuDtlSoldQty + SelectedDetail.PuDtlStolenQty + SelectedDetail.PuDtlLostQty + SelectedDetail.PuDtlDmgdQty;
            }
        }
    }
}
