using FocalPoint.Components.Common.Interface;
using FocalPoint.Data;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FocalPoint.Components.Common
{
    public class APIComponent: IAPICompnent
    {
        HttpClient clientHttp = new HttpClient();
        string baseURL;
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }

        public APIComponent()
        {
            var httpClientCache  = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();
            //clientHttp.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Task.Delay(10000).Wait();
            baseURL = DataManager.Settings.ApiUri;
        }

        public async Task<HttpResponseMessage> PostAsyc(string url, string requestConentString)
        {
            HttpResponseMessage responseMessage = null;
            try
            {
                var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
                this.clientHttp = httpClientCache.GetHttpClientAsync();
                StringContent requestContent = new StringContent(requestConentString);                
                string absoluteUrl = GetCompleteURL(url);
                HttpContent content = new StringContent(requestConentString, Encoding.UTF8, "application/json");

                responseMessage = await ClientHTTP.PostAsync(absoluteUrl, content);
                var content2 = responseMessage.Content.ReadAsStringAsync().Result.ToString();
                /*var response = await PostTest(requestContent, absoluteUrl);
                var content2 = response;*/
            }
            catch (Exception ex)
            {
            }

            return responseMessage;
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            HttpResponseMessage responseMessage = null;
            try
            {
                Uri absoluteUrl = new Uri(GetCompleteURL(url));
                responseMessage = await ClientHTTP.GetAsync(absoluteUrl, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                var content2 = responseMessage.Content.ReadAsStringAsync().Result.ToString();
            }
            catch (Exception ex)
            {
            }

            return responseMessage;

        }

        private string GetCompleteURL(string url)
        {
            return baseURL + url;
        }
    }
}


