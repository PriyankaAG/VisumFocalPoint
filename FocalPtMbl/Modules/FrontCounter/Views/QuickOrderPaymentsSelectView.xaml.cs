using FocalPoint.Modules.FrontCounter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuickOrderPaymentsSelectView : ContentPage
    {
        public QuickOrderPaymentsSelectView()
        {
            this.Title = "Select Payment Type";
            InitializeComponent();
            // Populate Options for selecting a payment using the images
            GetCurrentPaymentOptions();
        }

        private void GetCurrentPaymentOptions()
        {
            ((QuickOrderPaymentsSelectViewModel)this.BindingContext).GetPaymentOptions();
        }

        private async void collectionView_Tap(object sender, DevExpress.XamarinForms.CollectionView.CollectionViewGestureEventArgs args)
        {
            if (args.Item != null)
                await OpenCashDetailPage();
               // await OpenDetailPage(GetPayInfo(args.Item));
        }

        private Task OpenCashDetailPage()
        {
            return Navigation.PushAsync(new CashPaymentView());
        }

        private void TextEdit_TextChanged(object sender, EventArgs e)
        {

        }

        private void ComboBoxEdit_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void SimpleButton_Clicked(object sender, EventArgs e)
        {

        }
        private Payment GetPayInfo(object item)
        {
            if (item is PayTypeAndImage payInfo)
                return SelectedPaymentType(payInfo);
            return new Payment();
        }

        private Payment SelectedPaymentType(PayTypeAndImage payInfo)
        {

            if (payInfo.PaymentName == "Cash")
                return new Payment();
            return new Payment();
        }

        //Task OpenDetailPage(WorkOrder workOrder)
        //{
        //    if (workOrder == null)
        //        return Task.CompletedTask;

        //    if (this.inNavigation)
        //        return Task.CompletedTask;

        //    this.inNavigation = true;
        //    WorkOrder detailedWordOrder = ((WorkOrderFormViewModel)this.BindingContext).GetWorkOrderDetail(workOrder);
        //    return Navigation.PushAsync(new WorkOrderFormTabDetailsView(detailedWordOrder));
        //}
    }
}