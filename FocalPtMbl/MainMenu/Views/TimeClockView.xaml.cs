using DevExpress.XamarinForms.Editors;
using Visum.Services.Mobile.Entities;
using FocalPoint.MainMenu.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.MainMenu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimeClockView : ContentPage
    {
        public TimeClockView()
        {
            BindingContext = new TimeClockViewModel();
            this.Title = "Time Clock";
            //((TimeClockViewModel)this.BindingContext).GetCurrentStoreAndUsers();
            InitializeComponent();
        }

        private void Store_SelectionChanged(object sender, EventArgs e)
        {
            //change stats
            var comboBox = sender as ComboBoxEdit;
            TimeClockStore store = (TimeClockStore)comboBox.SelectedItem;
            ((TimeClockViewModel)this.BindingContext).StoreChanged(store);
        }
        private void User_SelectionChanged(object sender, EventArgs e)
        {
            //clear stores and change stats
                        var comboBox = sender as ComboBoxEdit;
            TimeClockUser user = (TimeClockUser)comboBox.SelectedItem;
            ((TimeClockViewModel)this.BindingContext).UserChanged(user);
            
        }
        private double longit = 0;
        private double latit = 0;
        Task<Location> GetLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                // whats me current permissions
                var location = Geolocation.GetLocationAsync(request);
                return location;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception               
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception                
            }
            catch (Exception ex)
            {
                // Unable to get location              
            }
            //return location;
            return null;
        }
        private async void ClockIn_Clicked(object sender, EventArgs e)
        {
            bool okToContinue = true;
            bool success = false;
            longit = 0;
            latit = 0;
            var location = await GetLocation();
            clockQuestionsAndStatus clockQuestions = ((TimeClockViewModel)this.BindingContext).ClockInQuestions();
            if (clockQuestions.returnMessage != "")
            {
                if (clockQuestions.YesNoOrNotification)
                {
                    bool clockOutThenIn = await DisplayAlert("Clock In", clockQuestions.returnMessage, "Yes", "No");
                    if (clockOutThenIn)
                    {
                        // try clockOut
                       success = ((TimeClockViewModel)this.BindingContext).ClockOut(location.Longitude, location.Latitude);
                        if (!success)
                            await DisplayAlert("Clock In", "Failed to Clock Out", "OK");
                        else
                        {
                            success = ((TimeClockViewModel)this.BindingContext).ClockIn(location.Longitude, location.Latitude);
                            if (!success)
                                await DisplayAlert("Clock In", "Failed to Clock In", "OK");
                        }
                    }
                    else
                    {
                        // try failed to clock in
                        await DisplayAlert("Clock In", "Failed to Clock In", "OK");
                        //success = ((TimeClockViewModel)this.BindingContext).ClockIn(location.Longitude, location.Latitude);
                        okToContinue = false;
                    }
                }
                else
                {
                    await DisplayAlert("Clock In", clockQuestions.returnMessage, "OK");
                }
            }
            else
            {
                //try clockIN
                success = ((TimeClockViewModel)this.BindingContext).ClockIn(location.Longitude, location.Latitude);
            }
        }
        private async void ClockOut_Clicked(object sender, EventArgs e)
        {
            bool success = false;
            bool okToContinue = true;
            longit = 0;
            latit = 0;
            var location = await GetLocation();
            clockQuestionsAndStatus clockQuestions = ((TimeClockViewModel)this.BindingContext).ClockOutQuestions();
            if (clockQuestions.returnMessage != "")
            {
                if (clockQuestions.YesNoOrNotification)
                {
                    bool clockInThenOut = await DisplayAlert("Clock Out", clockQuestions.returnMessage, "Yes", "No");
                    if (clockInThenOut)
                    {
                        // try clockIn
                        success = ((TimeClockViewModel)this.BindingContext).ClockIn(location.Longitude, location.Latitude);
                        if (!success)
                            await DisplayAlert("Clock In", "Failed to Clock In", "OK");
                        else
                        {
                            success = ((TimeClockViewModel)this.BindingContext).ClockOut(location.Longitude, location.Latitude);
                            if (!success)
                                await DisplayAlert("Clock In", "Failed to Clock Out", "OK");
                        }
                    }
                    else
                    {
                        // failed to clockout
                         await DisplayAlert("Clock In", "Failed to Clock Out", "OK");
                        okToContinue = false;
                    }
                }
                else
                {
                    await DisplayAlert("Clock Out", clockQuestions.returnMessage, "OK");
                    okToContinue = false;
                }
            }
            else
            {
                //try clockIN
                success = ((TimeClockViewModel)this.BindingContext).ClockOut(location.Longitude, location.Latitude);
            }
        }

        private async void SimpleButton_Clicked(object sender, EventArgs e)
        {
            await OpenDetailPage();
        }
        async Task OpenDetailPage()
        {
            await Navigation.PopAsync();
        }

        private void ComboBoxEdit_ClearIconClicked(object sender, System.ComponentModel.HandledEventArgs e)
        {

        }
    }
}