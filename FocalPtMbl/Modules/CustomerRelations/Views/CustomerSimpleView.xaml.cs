using DevExpress.XamarinForms.CollectionView;
using DevExpress.XamarinForms.Editors;
using FocalPoint.Modules.CustomerRelations.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
            try
            {
                if (args.Item != null)
                {
                    var customer = args.Item as Customer;
                    if (customer != null)
                    {
                        var balance = await ((CustomerSimpleViewModel)this.BindingContext).GetCustomerBalance(customer.CustomerNo);
                        await OpenDetailPage(await GetCustInfo(args.Item), balance);
                        collectionView.SelectedItem = null;
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return;
            }
        }
        private async Task<Customer> GetCustInfo(object item)
        {
            return await ((CustomerSimpleViewModel)this.BindingContext).GetCustomerInfo((item as Customer).CustomerNo);
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