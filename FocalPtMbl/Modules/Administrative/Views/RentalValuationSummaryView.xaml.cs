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
    public partial class RentalValuationSummaryView : ContentPage
    {
        public RentalValuationSummaryView()
        {
            BindingContext = new RentalValuationSummaryViewModel();
            InitializeComponent();
        }
        private void SimpleButton_Clicked(object sender, EventArgs e)
        {
            ((RentalValuationSummaryViewModel)this.BindingContext).GetRentVal();
            // await Navigation.PopModalAsync();
        }

        private void ComboBoxEdit_SelectionChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBoxEdit;
            ((RentalValuationSummaryViewModel)this.BindingContext).SelectedStore = (Company)comboBox.SelectedItem;
        }
    }
}