using FocalPtMbl.MainMenu.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.Inventory.ViewModels
{
    public class VendorDetailViewModel : ThemeBaseViewModel
    {
        readonly Vendor vend;
        public VendorDetailViewModel(Vendor contact)
        {
            this.vend = contact;
        }
        public int No
        {
            get => this.vend.VenNo;
        }
        public string Name
        {
            get => this.vend.VenName;
        }
        public string Addr1
        {
            get => this.vend.VenAddr1;
        }

        public string City
        {
            get => this.vend.VenCity;
        }

        public string Zip
        {
            get => this.vend.VenZip;
        }

        public string State
        {
            get => this.vend.VenState;
        }
        public string Email
        {
            get => this.vend.VenEMail;
        }
        public string Ext
        {
            get => this.vend.VenExt;
        }
        public string Fax
        {
            get => this.vend.VenFax;
        }
        public string AcctNo
        {
            get => this.vend.VenAcctNo;
        }
        public decimal Limit
        {
            get => this.vend.VenLimit;
        }
        public decimal MinPO
        {
            get => this.vend.VenMinPO;
        }
        public string Notes
        {
            get => this.vend.VenNotes;
        }
    }
}
