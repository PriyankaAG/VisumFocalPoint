using FocalPoint.Modules.FrontCounter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XamarinForms.DataForm;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FocalPtMbl;
using Visum.Services.Mobile.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerLupView : ContentPage
    {
        private string billaddr = "TestLabel";
        private string billcity = "";
        private string billstate = "";
        private string billzip = "";
        private bool inNavigation;

        //public delegate void EventHandler(Customer cust);
        //public event EventHandler EventPassNewCust;
        public CustomerLupView()
        {
            DevExpress.XamarinForms.DataForm.Initializer.Init();
            InitializeComponent();
            dataForm.PickerSourceProvider = new ComboBoxDataProvider();
            BindingContext = new CustomerLupViewModel();
            //dataForm.ValidateProperty += DataFormOnValidateProperty;
            this.Title = "Customer Add";
            //SortGroups();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            foreach (var group in dataForm.Groups)
            {
                group.IsCollapsed = true;
            }
            //set defaults
            //foreach (var item in dataForm.Items)
            //{
            //    if(item.FieldName == "State")
            //    {
            //        var itemValue = item.GetValue();
            //        item.SetValue( "IL");
            //    }
            //}
        }
        private void SortGroups()
        { 
        DataFormGroup Names = new DataFormGroup();
            DataFormGroup Address = new DataFormGroup();
            DataFormGroup BillingAddress = new DataFormGroup();
            DataFormGroup Phones = new DataFormGroup();
            DataFormGroup Emails = new DataFormGroup();
            DataFormGroup Misc = new DataFormGroup();
            DataFormGroup AccountTaxInfo = new DataFormGroup();
            DataFormGroup PersonalInfo = new DataFormGroup();
            DataFormGroup SMS = new DataFormGroup();
            //dataForm.groGroups.
            foreach (var group in dataForm.Groups)
            {
                if (group.GroupName == "Names")
                    Names = group;
                else if (group.GroupName == "Address")
                 Address = group;
                else if (group.GroupName == "Billing Address")
                    BillingAddress = group;
                else if (group.GroupName == "Phones")
                    Phones = group;
                else if (group.GroupName == "Emails")
                    Emails = group;
                else if (group.GroupName == "Misc")
                    Misc = group;
                else if (group.GroupName == "Account /Tax Info")
                    AccountTaxInfo = group;
                else if (group.GroupName == "Personal Info")
                    PersonalInfo = group;
                else if (group.GroupName == "SMS")
                    SMS = group;
            }
            dataForm.Groups.Clear();
            dataForm.Groups.Add(Names);
            dataForm.Groups.Add(BillingAddress);
            dataForm.Groups.Add(Phones);
            dataForm.Groups.Add(Emails);
            dataForm.Groups.Add(Misc);
            dataForm.Groups.Add(AccountTaxInfo);
            dataForm.Groups.Add(SMS);
        }
        protected override void OnDisappearing()
        {
           // dataForm.ValidateProperty -= DataFormOnValidateProperty;
            base.OnDisappearing();
        }
        public async void testMessage2()
        {
            //DisplayAlert("Number of users", "There are not enough licenses for the amount of mobile licenses active", "OK","Cancel").RunSynchronously();
            string action = await DisplayActionSheet("ActionSheet: Send to?", "Cancel", null, "Email", "Twitter", "Facebook");
        }
        Task TestMessage()
        {

                return Task.CompletedTask;

                return Task.CompletedTask;

            this.inNavigation = true;
            bool CustomerAdded = false;
            if (CustomerAdded)
            {
                return Navigation.PopModalAsync();
            }

        }

        void DataFormOnValidateProperty(object sender, DataFormPropertyValidationEventArgs e)
        {
           if (e.PropertyName != null )
            ((CustomerLupViewModel)this.BindingContext).UpdateModel(e.PropertyName, e.NewValue);
            if (e.PropertyName == "DOB")
            {
               var testval = (DateTime?)e.NewValue;
            }
            //    if (e.PropertyName == "FirstName")
            //{
            //    e.ErrorText = "Duplicate Name";
            //}
            //if (e.PropertyName == "LastName")
            //{
            //    if (NameHasError)
            //        e.HasError = true;
            //    else
            //        e.HasError = false;
            //    e.ErrorText = "Duplicate Name";
            //}
            //if (e.PropertyName == "PhoneNumber")
            //{
            //    if (PhoneHasError)
            //        e.HasError = true;
            //    else
            //        e.HasError = false;
            //    e.ErrorText = "Duplicate Number";
            //}
            //if (e.PropertyName == "PhoneNumber2")
            //{
            //    if (PhoneHasError)
            //        e.HasError = true;
            //    else
            //        e.HasError = false;
            //    e.ErrorText = "Duplicate Number";
            //}
            //if (e.PropertyName == "PhoneNumber3")
            //{
            //    if (PhoneHasError)
            //        e.HasError = true;
            //    else
            //        e.HasError = false;
            //    e.ErrorText = "Duplicate Number";
            //}
            // && property is true that address = bill address 
            if (e.PropertyName == "Address")
            {
                //((CustomerLupViewModel)this.BindingContext).Model.AddressToBilling();
                //((CustomerLupViewModel)this.BindingContext).AddressSameAsBillAdress(true);
                if(((CustomerLupViewModel)this.BindingContext).Model.CopyAddress)
                {
                   billaddr = e.NewValue.ToString();
                    foreach (var dataItem in dataForm.Items)
                    {
                        if (dataItem.FieldName == "BillAddress")
                        {
                            var binding = dataItem.BindingContext; //SetValue(e.NewValue);
                        }
                    }
                    if (e.NewValue.ToString().Length < 6)
                    {

                    }
                }
            }
            if (e.PropertyName == "BillAddress")
            {
                //((CustomerLupViewModel)this.BindingContext).AddressSameAsBillAdress(true);

                if (e.NewValue.ToString().Length < 6)
                {

                }
            }
            if (e.PropertyName == "CopyAddress")
            {
                ((CustomerLupViewModel)this.BindingContext).Model.CopyAddress = (bool)e.NewValue;
                if (e.NewValue.Equals(true))
                {
                    
                    foreach (var dataItem in dataForm.Items)
                    {
                        if (dataItem.GroupName == "Billing Address")
                        {
                            if( dataItem.FieldName == "BillAddress")
                            {
                                dataItem.LabelText = billaddr;
                            }
                            ((CustomerLupViewModel)this.BindingContext).AddressSameAsBillAdress(true, dataItem);
                            //dataItem.IsReadOnly = true;
                            
                        }
                    }
                }
                else
                {
                    
                    foreach (var dataItem in dataForm.Items)
                    {
                        if (dataItem.GroupName == "Billing Address")
                            ((CustomerLupViewModel)this.BindingContext).AddressSameAsBillAdress(false, dataItem);
                        //dataItem.IsReadOnly = false;
                    }
                }
            }
            
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            ((CustomerLupViewModel)this.BindingContext).Rotate(dataForm, height > width);
            base.OnSizeAllocated(width, height);
        }
        Task OpenDetailPage(Customer cust)
        {
            if (cust == null)
                return Task.CompletedTask;

            if (this.inNavigation)
                return Task.CompletedTask;

            this.inNavigation = true;
            //EventPassNewCust(((CustomerLupViewModel)this.BindingContext).NewCustomer);
            for (var counter = 1; counter < 2; counter++)
            {
               // Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            }
            MessagingCenter.Send<CustomerLupView, Customer>(this, "Hi", cust);
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 1]);

            MessagingCenter.Send<CustomerLupView, Customer>(this, "Hi", cust);

            //return Navigation.PopModalAsync();
            return Navigation.PopAsync();
        }
        public async void SimpleButton_Clicked(object sender, EventArgs e)
        {

            //Check Existing Phone Numbers
            //if (((CustomerLupViewModel)this.BindingContext).CheckNumbers())
            //{
            //    // Add in notification for yes and no
            //    bool stayOrLeave = await DisplayAlert("New customer # duplicate", "The New customer has the same phone number as one already in the database", "Cancel", "Continue");
            //    if (stayOrLeave)
            //    {
                    
            //        await Navigation.PopAsync();
            //    }
            //}
            ////Check Existing name
            //if (((CustomerLupViewModel)this.BindingContext).CheckNames())
            //{
            //    // Add in notification for yes and no
            //    bool stayOrLeave = await DisplayAlert("New customer Name duplicate ", "The New customer has the same name as one already in the database", "Cancel", "Continue");
            //    if (stayOrLeave)
            //    {
            //        await Navigation.PopAsync();
            //    }
            //}
            //hack
            //foreach (var dataItem in dataForm.Items)
            //{
            //    if (dataItem.GroupName == "Billing Address")
            //        ((CustomerLupViewModel)this.BindingContext).AddressSameAsBillAdress(false, dataItem);
            //    dataItem.IsReadOnly = false;
            //    if(dataItem.FieldName == "Company Name")
            //    {
            //        string compName = (string)dataItem.GetValue();
            //        if(compName == null || compName == "")
            //        {
            //            //Get Store settings
            //            // StoreSettings storeSettings = 
            //            if (((CustomerLupViewModel)this.BindingContext).Model.FirstName != "" && ((CustomerLupViewModel)this.BindingContext).Model.LastName != "")
            //                dataItem.SetValue(((CustomerLupViewModel)this.BindingContext).Model.FirstName + " " + ((CustomerLupViewModel)this.BindingContext).Model.LastName);
            //        }
            //    }
            //}
            //((CustomerLupViewModel)this.BindingContext).UpdateModel();
            dataForm.Commit();
            if (dataForm.Validate())
            {
                ((CustomerLupViewModel)this.BindingContext).SetCustomerData();
                if (((CustomerLupViewModel)this.BindingContext).SendNewCustomer() == true)
                {

                    //CustomerAdded = await App.Current.MainPage.DisplayAlert("Logout", "Are you sure you want to logout", "Ok", "Cancel");

                    await OpenDetailPage(((CustomerLupViewModel)this.BindingContext).NewCustomer);

                }// DisplayAlert("Success", "Your delivery information has been successfully saved", "OK");
                else
                {
                    //customer save failed invaild call etc
                    bool stayOrLeave = await DisplayAlert("Failed to save", "Customer Add Failed", "Leave", "Stay");
                    if (stayOrLeave)
                    {
                        await Navigation.PopAsync();
                    }
                }
            }
            else
            {
                // bool continueBack = true;
                //// continueBack = await App.Current.MainPage.DisplayAlert("Add Required Fields", "Are you sure you want to go back", "Ok", "Cancel");
                // if (continueBack)
                // {
                //     await Navigation.PopModalAsync();
                // }
                bool stayOrLeave = await DisplayAlert("Validation", "One or more required Fields need to be filled out. Would you like to stay or go back", "Leave", "Stay");
                if (stayOrLeave)
                {
                    await Navigation.PopAsync();
                }
                //string action = await DisplayActionSheet("ActionSheet: Send to?", "Cancel", null, "Email", "Twitter", "Facebook");
            }
        }

        //private async void SimpleButton_Clicked_1(object sender, EventArgs e)
        //{

        //    string action = await DisplayActionSheet("ActionSheet: Send to?", "Cancel", null, "Email", "Twitter", "Facebook");
        // //   TestMessage();
        //}
    }
}