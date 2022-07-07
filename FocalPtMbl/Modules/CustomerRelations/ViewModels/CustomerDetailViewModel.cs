using Visum.Services.Mobile.Entities;
using FocalPtMbl.MainMenu.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPoint.Modules.CustomerRelations.ViewModels
{
    public class CustomerDetailViewModel : ThemeBaseViewModel
    {
        readonly Customer cust;
        public CustomerDetailViewModel(Customer contact)
        {
            this.cust = contact;
        }

        public int No
        {
            get => this.cust.CustomerNo;
        }
        public string Name
        {
            get => this.cust.CustomerName;
        }
        public string Addr1
        {
            get => this.cust.CustomerAddr1;
        }

        public string Addr2
        {
            get => this.cust.CustomerAddr2;
        }

        public string City
        {
            get => this.cust.CustomerCity;
        }

        public string Zip
        {
            get => this.cust.CustomerZip;
        }

        public string State
        {
            get => this.cust.CustomerState;
        }

        public string BillAddr1
        {
            get => this.cust.CustomerBillAddr1;
        }

        public string BillAddr2
        {
            get => this.cust.CustomerBillAddr2;
        }

        public string BillCity
        {
            get => this.cust.CustomerBillCity;
        }

        public string BillZip
        {
            get => this.cust.CustomerBillZip;
        }

        public string BillState
        {
            get => this.cust.CustomerBillState;
        }

        public string Phone
        {
            get => Utils.Utils.ConvertPhone(this.cust.CustomerPhone);
        }

        public string PhoneType
        {
            get => this.cust.CustomerPhoneType;
        }

        public string Phone2
        {
            get => Utils.Utils.ConvertPhone(cust.CustomerPhone2);
        }

        public string PhoneType2
        {
            get => this.cust.CustomerPhoneType2;
        }

        public string Phone3
        {
            get => Utils.Utils.ConvertPhone(this.cust.CustomerPhone3);
        }

        public string PhoneType3
        {
            get => this.cust.CustomerPhoneType3;
        }

        public string Email
        {
            get => this.cust.CustomerEmail;
        }

        public string Email2
        {
            get => this.cust.CustomerEMail2;
        }

        public string Email3
        {
            get => this.cust.CustomerEMail3;
        }

        public string Type
        {
            get => this.cust.CustomerType;
        }

        public string Notes
        {
            get => this.cust.CustomerNotes;
        }

        public int Store
        {
            get => this.cust.CustomerStore;
        }

        public string Status
        {
            get => this.cust.CustomerStatus;
        }

        public string Status_Display
        {
            get => this.cust.CustomerStatus_Display;
        }

        public string Type_Display
        {
            get => this.cust.CustomerType_Display;
        }
    }
}
