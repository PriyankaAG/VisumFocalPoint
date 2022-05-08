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
        private  void SimpleButton_Clicked(object sender, EventArgs e)
        {
            ((DailyRevenueViewModel)this.BindingContext).GetDailyRev();
           // await Navigation.PopModalAsync();
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
    }
}