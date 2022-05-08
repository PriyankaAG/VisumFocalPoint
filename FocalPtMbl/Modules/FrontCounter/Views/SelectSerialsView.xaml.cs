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
    public partial class SelectSerialsView : ContentPage
    {
        public SelectSerialsView(OrderDtl dtl)
        {
            this.Title = "Edit Merch";
            InitializeComponent();
            ((SelectSerialsViewModel)this.BindingContext).QuantOut = dtl.OrderDtlQty;
            ((SelectSerialsViewModel)this.BindingContext).Discount = dtl.OrderDtlDiscount;
            ((SelectSerialsViewModel)this.BindingContext).OverridePricing = dtl.OrderDtlAmt;
        }

        private void SimpleButton_Clicked_Continue(object sender, EventArgs e)
        {
            //save order here
            Navigation.PopAsync();
        }

        private void SimpleButton_Clicked_Cancel(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void TextEdit_Completed(object sender, EventArgs e)
        {

        }

        private void TextEdit_Completed_1(object sender, EventArgs e)
        {

        }

        private void TextEdit_Completed_2(object sender, EventArgs e)
        {

        }
    }
}