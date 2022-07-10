using DevExpress.XamarinForms.CollectionView;
using DevExpress.XamarinForms.Editors;
using Visum.Services.Mobile.Entities;
using FocalPoint.Modules.ServiceDepartment.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.ServiceDepartment.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkOrderFormView : ContentPage
    {
        private bool inNavigation;
        public WorkOrderFormView()
        {
            InitializeComponent();
            BindingContext = new WorkOrderFormViewModel();
            //detailsPage = new WorkOrderFormTabDetailsView();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.inNavigation = false;
            ((WorkOrderFormViewModel)this.BindingContext).GetSearchedWorkOrdersInfo("");
            //((CustomerSimpleViewModel)this.BindingContext).GetCustomerInfo();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // EventPass("Back Code");
        }

        public async void ItemSelected(object sender, CollectionViewGestureEventArgs args)
        {
            if (args.Item != null)
                await OpenDetailPage(GetWOInfo(args.Item));
        }
        private WorkOrder GetWOInfo(object item)
        {
            if (item is WorkOrder WoInfo)
                return WoInfo;
            return new WorkOrder();
        }
        Task OpenDetailPage(WorkOrder workOrder)
        {
            if (workOrder == null)
                return Task.CompletedTask;

            if (this.inNavigation)
                return Task.CompletedTask;

            this.inNavigation = true;
            WorkOrder detailedWordOrder = ((WorkOrderFormViewModel)this.BindingContext).GetWorkOrderDetail(workOrder);
            return Navigation.PushAsync(new WorkOrderFormTabDetailsView(detailedWordOrder));
        }
        private void TextEdit_Completed(object sender, EventArgs e)
        {
            ((WorkOrderFormViewModel)this.BindingContext).GetSearchedWorkOrdersInfo((sender as TextEdit).Text);
        }
        private void TextEdit_Cleared(object sender, EventArgs e)
        {
            ((WorkOrderFormViewModel)this.BindingContext).GetSearchedWorkOrdersInfo("");
        }

        private void searchorderText_ClearIconClicked(object sender, System.ComponentModel.HandledEventArgs e)
        {
            ((WorkOrderFormViewModel)this.BindingContext).GetSearchedWorkOrdersInfo("");
        }
    }
}