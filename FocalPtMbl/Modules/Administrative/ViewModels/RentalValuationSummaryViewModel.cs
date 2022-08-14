using FocalPoint.Data;
using Visum.Services.Mobile.Entities;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace FocalPoint.Modules.Administrative.ViewModels
{
    public class RentalValuationSummaryViewModel : ThemeBaseViewModel
    {
        ObservableCollection<RentalValResultView> recent;
        public ObservableCollection<RentalValResultView> Recent
        {
            get => this.recent;
            private set
            {
                this.recent = value;
                OnPropertyChanged(nameof(Recent));
            }
        }
        public ObservableCollection<Company> currentStores;
        private Company selectedStore;
        public Company SelectedStore
        {
            get => this.selectedStore;
            set
            {
                this.selectedStore = value;
                OnPropertyChanged(nameof(SelectedStore));
            }
        }
        public ObservableCollection<Company> CurrentStores
        {
            get => this.currentStores;
            private set
            {
                this.currentStores = value;
                OnPropertyChanged(nameof(CurrentStores));
            }
        }
        public RentalValuationSummaryViewModel()
        {
            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();

            CurrentStores = new ObservableCollection<Company>();
            Task.Run(() =>
            {
                GetCurrentStores();
            });
            // GetRentVal();
        }

        private async void GetCurrentStores()
        {
            try
            {
                Indicator = true;
                Uri uriStores = new Uri(string.Format(DataManager.Settings.ApiUri + "LoginStores"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
                var responseDR = await ClientHTTP.GetAsync(uriStores);
                if (responseDR.IsSuccessStatusCode)
                {
                    var content = responseDR.Content.ReadAsStringAsync().Result;
                    var Stores = JsonConvert.DeserializeObject<List<Company>>(content);
                    foreach (var store in Stores)
                        CurrentStores.Add(store);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Indicator = false;
            }
        }

        private RentalValResult rentval = new RentalValResult();
        public RentalValResult RentalVal
        {
            get { return rentval; }
            set
            {
                if (rentval != value)
                {
                    rentval = value;
                    OnPropertyChanged("RentalVal");
                }
            }
        }
        HttpClient clientHttp;
        HttpClient baseCilentHttp = new HttpClient();
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        public async void GetRentVal()
        {
            try
            {
                int StoreID = 1;
                if (SelectedStore != null)
                    StoreID = SelectedStore.CmpNo;


                Uri uriDailyRev = new Uri(string.Format(DataManager.Settings.ApiUri + "Reports/RentalValSummary/"+StoreID));
                var responseDR = await ClientHTTP.GetAsync(uriDailyRev);
                if (responseDR.IsSuccessStatusCode)
                {
                    var content = responseDR.Content.ReadAsStringAsync().Result;
                    var rentalvalr = JsonConvert.DeserializeObject<RentalValResult>(content);
                    RentalVal = rentalvalr;
                    //TotalAmount = RentalVal.TotalAmt.ToString();
                    RentalValResultView rvrv = new RentalValResultView();
                    rvrv.TotalAmt = rentalvalr.TotalAmt;
                    rvrv.TotalCount = rentalvalr.TotalCnt;
                    rvrv.RentAmount = rentalvalr.RentAmt;
                    rvrv.RentCount = rentalvalr.RentCnt;
                    rvrv.OffRentCount = (rentalvalr.TotalCnt - rentalvalr.RentCnt);
                    rvrv.OffRentAmount = (rentalvalr.TotalAmt - rentalvalr.RentAmt);
                    rvrv.SerialTotalAmount = rentalvalr.SerialTotalAmt;
                    rvrv.SerialTotalCount = rentalvalr.SerialTotalCnt;
                    rvrv.SerialRentAmount = rentalvalr.SerialRentAmt;
                    rvrv.SerialRentCount = rentalvalr.SerialRentCnt;
                    rvrv.SerialRentAmount = rentalvalr.SerialRentAmt;
                    rvrv.SerialOffRentAmount = (rentalvalr.SerialTotalAmt - rentalvalr.SerialRentAmt);
                    rvrv.SerialOffRentCount = (rentalvalr.SerialTotalCnt - rentalvalr.SerialRentCnt);

                    if (rentalvalr.TotalAmt <= 0)
                        rvrv.UtilizationPercentAmount = 0;
                    else
                        rvrv.UtilizationPercentAmount = (rentalvalr.RentAmt / rentalvalr.TotalAmt * 100);

                    if (rentalvalr.TotalAmt <= 0)
                        rvrv.UtilizationPercentCount = 0;
                    else
                        rvrv.UtilizationPercentCount = (rentalvalr.RentCnt / rentalvalr.TotalCnt * 100);

                    if (rentalvalr.SerialTotalAmt <= 0)
                        rvrv.UtilizationSerialTotalAmount = 0;
                    else
                        rvrv.UtilizationSerialTotalAmount = (rentalvalr.SerialRentAmt / rentalvalr.SerialTotalAmt * 100);


                    if (rentalvalr.SerialTotalCnt <= 0)
                        rvrv.UtilizationSerialTotalCount = 0;
                    else
                        rvrv.UtilizationSerialTotalCount = (rentalvalr.SerialRentCnt / rentalvalr.SerialTotalCnt * 100);

                    if (rentalvalr.TotalAmt <= 0)
                        rvrv.UtilizationPercentAmount = 0;
                    else
                    {
                        if (rentalvalr.TotalAmt == 0)
                            rvrv.UnUtilizationPercentAmount = 100;

                        var val = 1 - (rentalvalr.RentAmt / rentalvalr.TotalAmt) * 100;
                        rvrv.UnUtilizationPercentAmount = val;
                        if (val < 0)
                            rvrv.UnUtilizationPercentAmount = 100;
                    }
                    if (Recent == null)
                    {
                        Recent = new ObservableCollection<RentalValResultView>();
                        Recent.Add(rvrv);
                    }
                    else
                    {
                        Recent.Clear();
                        Recent.Add(rvrv);
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }
        public string TotalAmount
        {
            get { return RentalVal.TotalAmt.ToString(); }
            set
            {
                if (RentalVal.TotalAmt.ToString() != value)
                {
                    RentalVal.TotalAmt = Decimal.Parse(value);
                    OnPropertyChanged("TotalAmount");
                }
            }
        }
        public string TotalCount
        {
            get { return RentalVal.TotalCnt.ToString(); }
        }
        public string RentAmount
        {
            get { return RentalVal.RentAmt.ToString(); }
        }
        public string RentCount
        {
            get { return RentalVal.RentCnt.ToString(); }
        }
        public string OffRentAmount
        {
            get { return (RentalVal.TotalAmt - RentalVal.RentAmt).ToString(); }
        }
        public string OffRentCount
        {
            get { return (RentalVal.TotalCnt - RentalVal.RentCnt).ToString(); }
        }
        public string SerialTotalAmount
        {
            get { return RentalVal.SerialTotalAmt.ToString(); }
        }
        public string SerialRentAmount
        {
            get { return RentalVal.SerialRentAmt.ToString(); }
        }




        public string SerialRentCount
        {
            get { return RentalVal.SerialRentCnt.ToString(); }
        }
        public string SerialOffRentAmount
        {
            get { return (RentalVal.SerialTotalAmt - RentalVal.SerialRentAmt).ToString(); }
        }
        public string SerialOffRentCount
        {
            get { return (RentalVal.SerialTotalCnt - RentalVal.SerialRentCnt).ToString(); }
        }
        public string UtilizationPercentAmount
        {
            get
            {
                if (RentalVal.TotalAmt <= 0)
                    return "0";

                return (RentalVal.RentAmt / RentalVal.TotalAmt * 100).ToString();
            }
        }
        public string UtilizationPercentCount
        {
            get
            {
                if (RentalVal.TotalAmt <= 0)
                    return "0";

                return (RentalVal.RentCnt / RentalVal.TotalCnt * 100).ToString();
            }
        }
        public string UtilizationSerialTotalAmount
        {
            get
            {
                if (RentalVal.SerialTotalAmt <= 0)
                    return "0";

                return (RentalVal.SerialRentAmt / RentalVal.SerialTotalAmt).ToString();
            }
        }
        public string UtilizationSerialTotalCount
        {
            get
            {
                if (RentalVal.SerialTotalCnt <= 0)
                    return "0";
                return (RentalVal.SerialRentCnt / RentalVal.SerialTotalCnt).ToString();
            }
        }
        public string UnUtilizationPercentAmount
        {
            get
            {
                if (RentalVal.TotalAmt == 0)
                    return "100";

                var val = 1 - (RentalVal.RentAmt / RentalVal.TotalAmt) * 100;

                if (val < 0)
                    return "100";

                return val.ToString();
            }
        }
    }
}
