using FocalPtMbl.Modules.Orders.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XamarinForms.DataForm;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPtMbl.Modules.Orders.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerFormView : ContentPage
    {
        public CustomerFormView() { 
            DevExpress.XamarinForms.DataForm.Initializer.Init();
            InitializeComponent();
            BindingContext = new CustomerFormViewModel();
            dataForm.ValidateProperty += DataFormOnValidateProperty;
        }

        void DataFormOnValidateProperty(object sender, DataFormPropertyValidationEventArgs e)
        {
            //if (e.PropertyName == nameof(CustomerAddInfo.DeliveryTimeFrom))
            //{
            //    ((CustomerAddInfo)dataForm.DataObject).DeliveryTimeFrom = (DateTime)e.NewValue;
            //    Device.BeginInvokeOnMainThread(() => {
            //        dataForm.Validate(nameof(CustomerAddInfo.DeliveryTimeTo));
            //    });
            //}
            //if (e.PropertyName == nameof(CustomerAddInfo.DeliveryTimeTo))
            //{
            //    DateTime timeFrom = ((CustomerAddInfo)dataForm.DataObject).DeliveryTimeFrom;
            //    if (timeFrom > (DateTime)e.NewValue)
            //    {
            //        e.HasError = true;
            //        e.ErrorText = "The end time cannot be less than the start time";
            //        return;
            //    }
            //}
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            ((CustomerFormViewModel)this.BindingContext).Rotate(dataForm, height > width);
            base.OnSizeAllocated(width, height);
        }

        private void SimpleButton_Clicked(object sender, EventArgs e)
        {
        dataForm.Commit();
        if (dataForm.Validate())
            DisplayAlert("Success", "Your delivery information has been successfully saved", "OK");
    }
    }
}