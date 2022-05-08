using FocalPoint.Data;
using Visum.Services.Mobile.Entities;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Timers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FocalPoint.MainMenu.ViewModels
{
    public class TimeClockViewModel : ThemeBaseViewModel
    {
        private string dateString = "";
        public string DateString
        {
            get => this.dateString;
            set
            {
                this.dateString = value;
                OnPropertyChanged(nameof(DateString));
            }
        }
        private string timeString = "";
        public string TimeString
        {
            get =>  this.timeString;
            set
            {
                this.timeString = value;
                OnPropertyChanged(nameof(TimeString));
            }
        }
        private DateTime utcTime = new DateTime();

        public DateTime UtcTime
        {
            get => this.utcTime;
            set
            {
                this.utcTime = value;
                OnPropertyChanged(nameof(UtcTime));
            }
        }
        private string lastActivity = "";
        public string LastActivity
        {
            get => this.lastActivity;
            set
            {
                this.lastActivity = value;
                OnPropertyChanged(nameof(LastActivity));
            }
        }
        private string timeClockStatus = "";
        public string TimeClockStatus
        {
            get => this.timeClockStatus;
            set
            {
                this.timeClockStatus = value;
                OnPropertyChanged(nameof(TimeClockStatus));
            }
        }
        public ObservableCollection<TimeClockStore> currentStores;
        private TimeClockStore selectedStore;
        public TimeClockStore SelectedStore
        {
            get => this.selectedStore;
            set
            {
                this.selectedStore = value;
                OnPropertyChanged(nameof(SelectedStore));
            }
        }
        public ObservableCollection<TimeClockStore> CurrentStores
        {
            get => this.currentStores;
            private set
            {
                this.currentStores = value;
                OnPropertyChanged(nameof(CurrentStores));
            }
        }
        private TimeClockStat timeClockStat;
        public TimeClockStat TimeClockStatCu
        {
            get { return timeClockStat; }
            set
            {
                if (timeClockStat != value)
                {
                    timeClockStat = value;
                    OnPropertyChanged("TimeClockStat");
                }
            }
        }

        private TimeClockUser selectedUser;
        public TimeClockUser SelectedUser
        {
            get { return selectedUser; }
            set
            {
                if (selectedUser != value)
                {
                    selectedUser = value;
                    OnPropertyChanged("SelectedUser");
                }
            }
        }
        private TimeClockUser currentUser;
        public TimeClockUser CurrentUser
        {
            get => this.currentUser;
            private set
            {
                this.currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }
        private ObservableCollection<TimeClockUser> currentUsers;
        public ObservableCollection<TimeClockUser> CurrentUsers
        {
            get => this.currentUsers;
            private set
            {
                this.currentUsers = value;
                OnPropertyChanged(nameof(CurrentUsers));
            }
        }
        HttpClient clientHttp = new HttpClient();
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        public TimeClockViewModel()
        {
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Elapsed += TimerElapsed;
            timer.AutoReset = true;
            timer.Start();
            CurrentStores = new ObservableCollection<TimeClockStore>();
            SelectedStore = new TimeClockStore();
            SelectedUser = new TimeClockUser();
            CurrentUsers = new ObservableCollection<TimeClockUser>();
            //SelectedDate = DateTime.Today;

            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();
            GetUsers();
            //GetCurrentStoreAndUsers();
        }
        
        internal void GetCurrentStoreAndUsers()
        {
            throw new NotImplementedException();
        }

        private void GetStores()
        {
            if (CurrentStores.Count > 0)
                CurrentStores.Clear();

            if (selectedUser != null)
            {
               foreach (var store in selectedUser.Stores)
                   CurrentStores.Add(store);
            }
        }
        public void UserChanged(TimeClockUser selUser)
        {
            CurrentStores.Clear();
            TimeClockStore selNewStore = new TimeClockStore();
            SelectedStore = selNewStore;
            SelectedUser = selUser;
            
            if (selectedUser != null)
            {
                CurrentUser = selUser;
                GetStatus();
                GetStores();
            }
        }
        internal void StoreChanged(TimeClockStore store)
        {
            SelectedStore = store;
        }
        private void GetUsers()
        {
            try
            {
                Uri uriStores = new Uri(string.Format(DataManager.Settings.ApiUri + "TimeClock/Users"));
                var responseDR = ClientHTTP.GetAsync(uriStores).GetAwaiter().GetResult();
                if (responseDR.IsSuccessStatusCode)
                {
                    var content = responseDR.Content.ReadAsStringAsync().Result;
                    var Users = JsonConvert.DeserializeObject<List<TimeClockUser>>(content);
                    foreach (var user in Users)
                        CurrentUsers.Add(user);
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void GetStatus()
        {
            try
            {
                if (CurrentUser.UserID != null)
                { 
                    Uri uriStores = new Uri(string.Format(DataManager.Settings.ApiUri + "TimeClock/Status/" + CurrentUser.UserID.ToString()));
                var responseDR = ClientHTTP.GetAsync(uriStores).GetAwaiter().GetResult();
                if (responseDR.IsSuccessStatusCode)
                {
                    var content = responseDR.Content.ReadAsStringAsync().Result;
                    var tcs = JsonConvert.DeserializeObject<TimeClockStat>(content);
                        TimeClockStatCu = tcs;
                }
                    if (TimeClockStatCu.ClockedIn)
                        TimeClockStatus = "Status: Clocked In";
                    else
                        TimeClockStatus = "Status: Clocked Out";
                    if (TimeClockStatCu.LastActivity != null)
                        LastActivity = "Last Activity: " + TimeClockStatCu.LastActivity;
            }
            }
            catch (Exception ex)
            {

            }
        }

        public clockQuestionsAndStatus ClockInQuestions()
        {
            clockQuestionsAndStatus clockResult = new clockQuestionsAndStatus();
            clockResult.returnMessage = "";
            clockResult.YesNoOrNotification = false;
            if (SelectedUser.FullName == null)
            {
                clockResult.returnMessage = "Please select a user.";
                clockResult.YesNoOrNotification = false;
                return clockResult;
            }

            if (SelectedStore.CmpName == null)
            {
                clockResult.returnMessage = "Please select a store.";
                clockResult.YesNoOrNotification = false;
                return clockResult;
            }
            if (TimeClockStatCu.ClockedIn)
            {
                //do the clockout and then clock in
                clockResult.returnMessage = "You are Currently Clocked - IN, do you want to Clock - OUT before Clocking - IN?";
                clockResult.YesNoOrNotification = true;
                return clockResult;
            }
                return clockResult;
        }
        public bool ClockIn(double longit, double lat)
        {
            try
            {
                TimeClockIn ClockIn = new TimeClockIn();
                ClockIn.UserID = SelectedUser.UserID;
                ClockIn.HomeStore = SelectedUser.HomeStore;
                ClockIn.Latitude = lat;
                ClockIn.Longitude = longit;
                ClockIn.UTCTime = UtcTime;
                var stringContent = new StringContent(
                  JsonConvert.SerializeObject(new { ClockIn }),
                  Encoding.UTF8,
                  "application/json");

                Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "TimeClock/ClockIn/"));
                    var responseDR = ClientHTTP.PostAsync(uri, stringContent).GetAwaiter().GetResult();
                    if (responseDR.IsSuccessStatusCode)
                    {
                        var content = responseDR.Content.ReadAsStringAsync().Result;
                        var clocked = JsonConvert.DeserializeObject<bool>(content);
                    GetStatus();
                        return clocked;
                    }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool ClockOut(double longit, double lat)
        {
            try
            {
                TimeClockOut ClockOut = new TimeClockOut();
                ClockOut.UserID = SelectedUser.UserID;
                ClockOut.LastClockID = TimeClockStatCu.LastClockID;
                ClockOut.Latitude = lat;
                ClockOut.Longitude = longit;
                ClockOut.UTCTime = UtcTime;
                var stringContent = new StringContent(
                  JsonConvert.SerializeObject(new { ClockOut }),
                  Encoding.UTF8,
                  "application/json");

                Uri uri = new Uri(string.Format(DataManager.Settings.ApiUri + "TimeClock/ClockOut/"));
                var responseDR = ClientHTTP.PostAsync(uri, stringContent).GetAwaiter().GetResult();
                if (responseDR.IsSuccessStatusCode)
                {
                    var content = responseDR.Content.ReadAsStringAsync().Result;
                    var clocked = JsonConvert.DeserializeObject<bool>(content);
                    GetStatus();
                    return clocked;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public clockQuestionsAndStatus ClockOutQuestions()
        {
            clockQuestionsAndStatus clockResult = new clockQuestionsAndStatus();
            clockResult.returnMessage = "";
            if (SelectedUser.FullName == null)
            {
                clockResult.returnMessage = "Please select a user.";
                clockResult.YesNoOrNotification = false;
                return clockResult;
            }

            if (SelectedStore.CmpName == null)
            {
                clockResult.returnMessage = "Please select a store.";
                clockResult.YesNoOrNotification = false;
                return clockResult;
            }
            if (!TimeClockStatCu.ClockedIn)
            {
                //do the clockout and then clock in
                clockResult.returnMessage = "You are Currently Clocked - OUT, do you want to Clock - IN with the Current Date/Time before Clocking - OUT?";
                clockResult.YesNoOrNotification = true;
                return clockResult;
            }
            return clockResult;
        }
        DateTime currentTime = new DateTime();
        private void TimerElapsed(Object source, ElapsedEventArgs e)
        {
            currentTime = new DateTime(e.SignalTime.Ticks);
            DateString = currentTime.ToString("dddd, MMMM d, yyyy");
            TimeString = currentTime.ToString("T");
            UtcTime = e.SignalTime.ToUniversalTime();
            //DateString = e.SignalTime.DayOfWeek.
           // DateString =  e.SignalTime.Date.ToShortDateString();
           // TimeString = e.SignalTime.Hour.ToString() + ":" + e.SignalTime.Minute.ToString() + ":" + e.SignalTime.Second.ToString();
            //TimeString = string.Format("{0:HH:mm:ss tt}",e.SignalTime.TimeOfDay.ToString()); // String.Format("{0:dddd, MMMM d, yyyy}", e.SignalTime.ToString());
        }
    }

}
public class clockQuestionsAndStatus
{
    public string returnMessage { get; set; }
    public bool YesNoOrNotification { get; set; }

}
