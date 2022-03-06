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
    public partial class CashDrawerSummaryView : ContentPage
    {
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
            ((CashDrawerSummaryViewModel)this.BindingContext).DateChanged(Date);
        }
        public CashDrawerSummaryView()
        {
            DevExpress.XamarinForms.CollectionView.Initializer.Init();
            DevExpress.XamarinForms.Editors.Initializer.Init();
            //BindingContext = new CashDrawerSummaryViewModel();
            InitializeComponent();
            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((CashDrawerSummaryViewModel)this.BindingContext).GetCashDrawers();
        }


        private void ComboBoxEdit_SelectionChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBoxEdit;
            ((CashDrawerSummaryViewModel)this.BindingContext).SelectedCashDrawer = (CashDrawer)comboBox.SelectedItem;
        }
        private void SimpleButton_Clicked(object sender, EventArgs e)
        {
            ((CashDrawerSummaryViewModel)this.BindingContext).GetCashDrawerSummary();
            // await Navigation.PopModalAsync();
        }
    }
}