using DevExpress.XamarinForms.DataGrid;
using FocalPoint.Modules.CustomerRelations.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XamarinForms.DataGrid;
using DevExpress.XamarinForms.Navigation;
using Visum.Services.Mobile.Entities;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Windows.Input;

namespace FocalPoint.Modules.CustomerRelations.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerDetailView
    {
        private string number1;
        public ICommand CallPhone { get; }
       // private Command CallPhone = new Command(() => AttemptCall(number1));
        readonly CustomerDetailViewModel viewModel;
        public CustomerDetailView(Customer cust, CustomerBalance balance)
        {
            //DevExpress.XamarinForms.Navigation.Initializer.Init();
            this.viewModel = new CustomerDetailViewModel(cust, balance);
            InitializeComponent();
            BindingContext = this.viewModel;
            number1 = this.viewModel.Phone;
            CallPhone = new Command(() => AttemptCall(number1));

        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            //if web request is pending try to reload token and wait if()
            ActivityIndicator activityIndicator = new ActivityIndicator { IsRunning = true };
            await Task.Delay(10000);
            //when task is complete turn indicator off
            activityIndicator.IsRunning = false;
        }
        public async void On_ItemSelected(object sender, DataGridGestureEventArgs args)
        {

        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            // do the update to Balance
        }

        private void Phone1_Focused(object sender, FocusEventArgs e)
        {
            try
            {
                PhoneDialer.Open((sender as Label).Text);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }
        private void AttemptCall(string number)
        {
            try
            { 
            PhoneDialer.Open(number);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }
    }

    public class UpperCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            return value?.ToString().ToUpperInvariant();
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            return value?.ToString().ToLowerInvariant();
        }
    }

    public class CallTypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                             object parameter, CultureInfo culture)
        {
            return String.Format("demotabview_{0}", value.ToString().ToLowerInvariant());
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ContactIconTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PhotoTemplate { get; set; }
        public DataTemplate IconTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            //if (item is CellData gridData)
            //    return OnSelectTemplate(gridData.Item, container);
            //if (item is PhoneContact contact)
            //    return contact.HasPhoto ? PhotoTemplate : IconTemplate;
            //if (item is CallInfo callInfo)
            //    return OnSelectTemplate(callInfo.Contact, container);
            return IconTemplate;
        }
    }
}