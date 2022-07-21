using DevExpress.XamarinForms.Editors;
using Visum.Services.Mobile.Entities;
using FocalPoint.Modules.Administrative.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FocalPoint.Modules.Payments.Views;

namespace FocalPoint.Modules.Administrative.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DailyRevenueView : ContentPage
    {
        public DailyRevenueView()
        {
           // BindingContext = new DailyRevenueViewModel();
            InitializeComponent();
        }
        private DateTime? dte = new DateTime();
        public DateTime? Date
        {
            get { return dte; }
            set
            {
                if (dte != value)
                {
                    dte = value;
                }
            }
        }
        private void DateEdit_DateChanged(object sender, EventArgs e)
        {
            //if list count is not 0 add date else popup cancel/cont change date back
            var startdateEdit = sender as DateEdit;
            Date = startdateEdit.Date;
            ((DailyRevenueViewModel)this.BindingContext).DateChanged(Date);
        }
        private async void SimpleButton_Clicked(object sender, EventArgs e)
        {
            var validationMsg = ((DailyRevenueViewModel)this.BindingContext).Validate();
            if (!string.IsNullOrEmpty(validationMsg))
            {
                await DisplayAlert("FocalPoint", validationMsg, "OK");
                return;
            }
            ((DailyRevenueViewModel)this.BindingContext).GetDailyRev();

            //ViewOrderEntityComponent viewOrderEntityComponent = new ViewOrderEntityComponent();
            //var orderDetails = await viewOrderEntityComponent.GetOrderDetails(501842);
            //if (orderDetails != null)
            //{
            //    await Navigation.PushAsync(new PaymentView(orderDetails));
            //}

        }
        private void ComboBoxEdit_SelectionChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBoxEdit;
            ((DailyRevenueViewModel)this.BindingContext).SelectedStore = (Company)comboBox.SelectedItem;
        }
        private void ComboBoxEdit_SelectionChangedPost(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBoxEdit;
            ((DailyRevenueViewModel)this.BindingContext).SelectedPostCode = (PostCode)comboBox.SelectedItem;
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            var startdateEdit = sender as DatePicker;
            Date = startdateEdit.Date;
            ((DailyRevenueViewModel)this.BindingContext).DateChanged(Date);
        }
    }
}