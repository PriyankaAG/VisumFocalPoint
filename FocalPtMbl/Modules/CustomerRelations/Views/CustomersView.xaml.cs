using DevExpress.XamarinForms.CollectionView;
using FocalPoint.Modules.Customer_Relations.ViewModels;
using FocalPoint.Modules.CustomerRelations.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Customer_Relations.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomersView : ContentPage
    {
        bool inNavigation = false;
        public CustomersView()
        {
            DevExpress.XamarinForms.CollectionView.Initializer.Init();
            InitializeComponent();
            BindingContext = new CustomersViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.inNavigation = false;
        }
        public async void ItemSelected(object sender, CollectionViewGestureEventArgs args)
        {
            if (args.Item != null)
                await OpenDetailPage(GetCustInfo(args.Item));
        }
        private Customer GetCustInfo(object item)
        {
            if (item is Customer custInfo)
                return custInfo;
            return null;
        }
        
        Task OpenDetailPage(Customer cust)
        {
            if (cust == null)
                return Task.CompletedTask;

            if (this.inNavigation)
                return Task.CompletedTask;

            this.inNavigation = true;
            return Navigation.PushAsync(new CustomerDetailView(cust));
        }
    }
    public class ContactItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DetailTemplate { get; set; }
        public DataTemplate SelectCustomerTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            //if (item is Data.DataLayer.Customer)
            //    return contact.HasPhoto ? PhotoTemplate : IconTemplate;

            return DetailTemplate;
        }
    }
}