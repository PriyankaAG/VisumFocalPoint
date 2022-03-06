using DevExpress.XamarinForms.Editors;
using Visum.Services.Mobile.Entities;
using FocalPoint.Modules.FrontCounter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuickOrderHeaderView : ContentPage
    {
        //private DateTime? startdte;
        //private DateTime? enddte;
        //public DateTime? StartDate
        //{
        //    get { return startdte; }
        //    set
        //    {
        //        if (startdte != value)
        //        {
        //            startdte = value;
        //        }
        //    }
        //}
        //public DateTime? EndDate
        //{
        //    get { return enddte; }
        //    set
        //    {
        //        if (enddte != value)
        //        {
        //            enddte = value;
        //        }
        //    }
        //}
        private List<OrderDtl> orderdtls;
        public List<OrderDtl> OrderDtls
        {
            get { return orderdtls; }
            set
            {
                if (orderdtls != value)
                {
                    orderdtls = value;
                }
            }
        }




        public QuickOrderHeaderView()
        {
            this.Title = "Header";

            InitializeComponent();
            //startdte = new DateTime();
            //enddte = new DateTime();
            GetStatesAndLengths();
        }

        private void GetStatesAndLengths()
        {
           ((QuickOrderHeaderViewModel)this.BindingContext).GetStates();
            ((QuickOrderHeaderViewModel)this.BindingContext).GetOrderSettings();
        }

        private async void SimpleButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void DateEdit_DateChanged(object sender, EventArgs e)
        {
            //if list count is not 0 add date else popup cancel/cont change date back
            var startdateEdit = sender as DateEdit;
            if (((QuickOrderHeaderViewModel)this.BindingContext).StartDate != null || ((QuickOrderHeaderViewModel)this.BindingContext).EndDate != null)
            {
                string headerText = "Start: " + ((QuickOrderHeaderViewModel)this.BindingContext).StartDate.ToString() + Environment.NewLine + "End: " + ((QuickOrderHeaderViewModel)this.BindingContext).EndDate;
                MessagingCenter.Send<QuickOrderHeaderView, string>(this, "Hi", headerText);
            }
        }

        private void TimeEdit_TimeChanged(object sender, EventArgs e)
        {
            var timeEdit = sender as TimeEdit;
            var selectedComboItem = timeEdit.Time;
            if (((QuickOrderHeaderViewModel)this.BindingContext).StartDate != null || ((QuickOrderHeaderViewModel)this.BindingContext).EndDate != null)
            {
                string headerText = "Start: " + ((QuickOrderHeaderViewModel)this.BindingContext).StartDate.ToString() + Environment.NewLine + "End: " + ((QuickOrderHeaderViewModel)this.BindingContext).EndDate;
                MessagingCenter.Send<QuickOrderHeaderView, string>(this, "Hi", headerText);
            }
        }

        private void DateEdit_DateChanged_1(object sender, EventArgs e)
        {
            var enddateEdit = sender as DateEdit;
            var endDte = enddateEdit.Date;
            if (((QuickOrderHeaderViewModel)this.BindingContext).StartDate != null || ((QuickOrderHeaderViewModel)this.BindingContext).EndDate != null)
            {
                string headerText = "Start: " + ((QuickOrderHeaderViewModel)this.BindingContext).StartDate.ToString() +Environment.NewLine + "End: " + ((QuickOrderHeaderViewModel)this.BindingContext).EndDate;
                MessagingCenter.Send<QuickOrderHeaderView, string>(this, "Hi", headerText);

            }
        }

        private void TimeEdit_TimeChanged_1(object sender, EventArgs e)
        {
            var timeEdit = sender as TimeEdit;
            var selectedComboItem = timeEdit.Time;
            if (((QuickOrderHeaderViewModel)this.BindingContext).StartDate != null || ((QuickOrderHeaderViewModel)this.BindingContext).EndDate != null)
            {
                string headerText = "Start: " + ((QuickOrderHeaderViewModel)this.BindingContext).StartDate.ToString() + Environment.NewLine + "End: " + ((QuickOrderHeaderViewModel)this.BindingContext).EndDate;
                MessagingCenter.Send<QuickOrderHeaderView, string>(this, "Hi", headerText);

            }
        }
        private DateTime GetDateTimeAndRange()
        {
            return new DateTime();
        }

        private void TextEdit_TextChanged(object sender, EventArgs e)
        {
            var TaxCodeBox = sender as TextEdit;
            var selectedComboItem = TaxCodeBox.Text;

        }

        private void ComboBoxEdit_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var selectedLengthCombo = sender as ComboBoxEdit;
            if (selectedLengthCombo.SelectedItem != null)
            {
                var selectedComboItem = selectedLengthCombo.SelectedItem.ToString();
                if (((QuickOrderHeaderViewModel)this.BindingContext).StartDate != null || ((QuickOrderHeaderViewModel)this.BindingContext).EndDate != null)
                {
                    string headerText = "Start: " + ((QuickOrderHeaderViewModel)this.BindingContext).StartDate.ToString() + "End: " + ((QuickOrderHeaderViewModel)this.BindingContext).EndDate;
                    MessagingCenter.Send<QuickOrderHeaderView, string>(this, "Hi", headerText);

                }
            }

        }

        private void ComboBoxEdit_PropertyChanged_1(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var selectedStateCombo = sender as ComboBoxEdit;
            if (selectedStateCombo.SelectedItem != null)
            {
                var selectedComboItem = selectedStateCombo.SelectedItem.ToString();
                //StartDate = ((QuickOrderHeaderViewModel)this.BindingContext).GetStartDateAndTimeValues();
                if (((QuickOrderHeaderViewModel)this.BindingContext).StartDate != null || ((QuickOrderHeaderViewModel)this.BindingContext).EndDate != null)
                {
                    ((QuickOrderHeaderViewModel)this.BindingContext).GetEndDateAndTimeValues(selectedComboItem);
                    string headerText = "Start: " + ((QuickOrderHeaderViewModel)this.BindingContext).StartDate.ToString() + "End: " + ((QuickOrderHeaderViewModel)this.BindingContext).EndDate;

                }
            }
        }
    }
}