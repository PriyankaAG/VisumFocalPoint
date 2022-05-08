using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Visum.Services.Mobile.Entities;
using DevExpress.XamarinForms.DataGrid.Internal;
using System.Collections.ObjectModel;

namespace FocalPoint.Modules.Customer_Relations.ViewModels
{
    public class CustomersViewModel : NotificationObject
    {
        //ObservableCollection<CallInfo> recent;
        //public ObservableCollection<CallInfo> Recent
        //{
        //    get => this.recent;
        //    private set
        //    {
        //        this.recent = value;
        //        OnPropertyChanged(nameof(Recent));
        //    }
        //}
        public CustomersViewModel()
        {
            //GridOrdersRepository repository = new GridOrdersRepository();

            //get list from database

           // IList<PhoneContact> customersList = repository.Customers.ToList().ConvertAll((customer) => new PhoneContact(customer));

        }
    }
}
