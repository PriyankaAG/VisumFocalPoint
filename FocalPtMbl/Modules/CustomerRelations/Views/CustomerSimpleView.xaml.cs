using DevExpress.XamarinForms.CollectionView;
using DevExpress.XamarinForms.Editors;
using FocalPoint.Modules.CustomerRelations.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.CustomerRelations.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerSimpleView : ContentPage
    {
        bool inNavigation = false;
        public CustomerSimpleView()
        {
            InitializeComponent();
            BindingContext = new CustomerSimpleViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.inNavigation = false;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        public async void ItemSelected(object sender, CollectionViewGestureEventArgs args)
        {
            if (args.Item != null)
            {
                var customer = args.Item as Customer;
                if (customer != null)
                {
                    var balance = await ((CustomerSimpleViewModel)this.BindingContext).GetCustomerBalance(customer.CustomerNo);
                    await OpenDetailPage(GetCustInfo(args.Item), balance);
                    collectionView.SelectedItem = null;
                }
            }
        }
        private Customer GetCustInfo(object item)
        {
            if (item is Customer custInfo)
                return custInfo;
            return new Customer();
        }
        Task OpenDetailPage(Customer cust, CustomerBalance balance)
        {
            if (cust == null)
                return Task.CompletedTask;

            if (this.inNavigation)
                return Task.CompletedTask;

            this.inNavigation = true;
            return Navigation.PushAsync(new CustomerDetailView(cust, balance));
        }
        private void TextEdit_Completed(object sender, EventArgs e)
        {
            ((CustomerSimpleViewModel)this.BindingContext).GetSearchedCustomersInfo((sender as TextEdit).Text);
        }
        private void TextEdit_Cleared(object sender, EventArgs e)
        {
            ((CustomerSimpleViewModel)this.BindingContext).GetSearchedCustomersInfo("");
        }
    }
}