using FocalPoint.Data;
using FocalPtMbl.MainMenu.Data;
using FocalPtMbl.MainMenu.Models;
using FocalPtMbl.MainMenu.ViewModels.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPtMbl.MainMenu.ViewModels
{
    public class MainPageViewModel : ThemeBaseViewModel
    {
        public string Version => "0.0.0.0";
        public string TitleText => "FocalPoint";

        public string FPImage = "Icon_Small.png";
        public string SubTitle => "";

        private bool _isPageBusy;
        public bool IsPageBusy
        {

            get => _isPageBusy; set => SetProperty(ref _isPageBusy, value);
        }
            public List<PageItem> Items
        {
            get => GetItems();
        }
        public ICommand NavigationControlCommand { get; }
        public ICommand NavigationPageCommand { get; }

        public MainPageViewModel(INavigationService navigationService)
        {
            try
            {
                IsPageBusy = true;
                NavigationControlCommand = new DelegateCommand<Type>(async (p) =>
                {
                    ControlPageViewModel vm = new ControlPageViewModel(navigationService, (IPageData)Activator.CreateInstance(p));
                    await navigationService.Push(vm);
                });
                NavigationPageCommand = new DelegateCommand<PageItem>(async (p) => await navigationService.PushPage(p));
            }
            catch(Exception ex)
            {
                IsPageBusy = false;
            }

        }
        public MainPageViewModel(INavigationService navigationService, bool getHttpClient)
        {
            try
            {
                IsPageBusy = true;
                NavigationControlCommand = new DelegateCommand<Type>(async (p) =>
                {
                    ControlPageViewModel vm = new ControlPageViewModel(navigationService, (IPageData)Activator.CreateInstance(p));
                    await navigationService.Push(vm);
                });
                NavigationPageCommand = new DelegateCommand<PageItem>(async (p) => await navigationService.PushPage(p));
                var httpClientCache = DependencyService.Resolve<FocalPoint.MainMenu.Services.IHttpClientCacheService>();
                this.httpClient = httpClientCache.GetHttpClientAsync();
                LoadHttpClient();
            }
            catch (Exception ex)
            {
                IsPageBusy = false;
            }
        }
        HttpClient httpClient;
        internal void LoadHttpClient()
        {
            try
            {
                while (httpClient == null)
                {
                    var httpClientCache = DependencyService.Resolve<FocalPoint.MainMenu.Services.IHttpClientCacheService>();
                    this.httpClient = httpClientCache.GetHttpClientAsync();
                    //Task.Delay(10000).Wait();
                }
                int counter = 0;
                Uri uriStores = new Uri(string.Format(DataManager.Settings.ApiUri + "LoginStores"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
                var responseDR = httpClient.GetAsync(uriStores).GetAwaiter().GetResult();
                if (responseDR.IsSuccessStatusCode)
                {
                    var content = responseDR.Content.ReadAsStringAsync().Result;
                    var Stores = JsonConvert.DeserializeObject<List<Company>>(content);
                }
                else
                {
                    while (!responseDR.IsSuccessStatusCode)
                    {
                        if (counter > 5)
                            break;
                        responseDR = httpClient.GetAsync(uriStores).GetAwaiter().GetResult();
                        //Task.Delay(10000).Wait();
                        counter++;
                    }
                }
            }
            catch (Exception ex)
            {
                LoadHttpClient();
            }
            // throw new NotImplementedException();
        }

        List<PageItem> GetItems()
        {
            List<PageItem> result = new List<PageItem>();
            result.Add(new PageItem()
            {
                Header = true,
                //Title = SubTitle
            });
            result.AddRange(CategoryGroupsData.PageItems);
            return result;
        }
    }
}
