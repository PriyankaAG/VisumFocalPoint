using FocalPoint.Data;
using FocalPoint.MainMenu.Views;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint
{
    public class APIComponent : IAPICompnent
    {
        private string mediaType = "application/json";

        public APIComponent()
        {
            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();
            //clientHttp.Timeout = new TimeSpan(0, 0, 10);
            baseURL = DataManager.Settings.ApiUri;
        }

        HttpClient clientHttp { get; set; }
        string baseURL;

        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }

        public void AddStoreToHeader(string storeNo)
        {
            if (clientHttp.DefaultRequestHeaders.Contains("StoreNo"))
                clientHttp.DefaultRequestHeaders.Remove("StoreNo");
            clientHttp.DefaultRequestHeaders.Add("StoreNo", storeNo);
        }

        public void AddTerminalToHeader(string terminalNo)
        {
            if (clientHttp.DefaultRequestHeaders.Contains("TerminalNo"))
                clientHttp.DefaultRequestHeaders.Remove("TerminalNo");
            clientHttp.DefaultRequestHeaders.Add("TerminalNo", terminalNo);
        }

        public async Task<T> GetAsync<T>(string url)
        {
            T typedRequestContent = default;
            try
            {
                HttpResponseMessage httpResponseMessage = await GetAsync(url);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string content = await httpResponseMessage.Content.ReadAsStringAsync();
                    typedRequestContent = JsonConvert.DeserializeObject<T>(content);
                }
                else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleTokenExpired();
                }
                else
                {
                    //TODO: Handle failure API's, add logs to server
                }
            }
            catch
            {
                throw;
            }
            return typedRequestContent;
        }

        public async Task<T> PostAsync<T>(string url, string requestContent)
        {
            T typedRequestContent = default;
            try
            {
                HttpResponseMessage httpResponseMessage = await PostAsync(url, requestContent);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string content = await httpResponseMessage.Content.ReadAsStringAsync();
                    typedRequestContent = JsonConvert.DeserializeObject<T>(content);
                }
                else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleTokenExpired();
                }
                else
                {
                    string contentStr = await httpResponseMessage.Content.ReadAsStringAsync();
                    //TODO: Handle failure API's, add logs to server
                }
            }
            catch
            {
                throw;
            }
            return typedRequestContent;
        }

        public async Task<T> SendAsync<T>(string url, string requestContent, bool isLoginMethod = false)
        {
            T typedRequestContent = default;
            try
            {
                HttpResponseMessage httpResponseMessage = await SendAsync(url, requestContent, isLoginMethod);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string content = await httpResponseMessage.Content.ReadAsStringAsync();
                    typedRequestContent = JsonConvert.DeserializeObject<T>(content);
                }
                else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleTokenExpired();
                }
                else
                {
                    //TODO: Handle failure API's, add logs to server
                }
            }
            catch
            {
                throw;
            }
            return typedRequestContent;
        }
        public async Task<OrderUpdate> SendAsyncUpdateOrder(string url, string requestContent, bool isLoginMethod = false)
        {
            OrderUpdate orderUpdateRefresh = new OrderUpdate();
            try
            {
                HttpResponseMessage httpResponseMessage = await SendAsync(url, requestContent, isLoginMethod);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string content = await httpResponseMessage.Content.ReadAsStringAsync();
                    orderUpdateRefresh = JsonConvert.DeserializeObject<OrderUpdate>(content);
                    //orderUpdate = JsonConvert.DeserializeObject<OrderUpdate>(orderContent);
                    //check if empty result
                    if (orderUpdateRefresh != null && orderUpdateRefresh.Order != null)
                    {
                        //CurrentOrder = orderUpdateRefresh.Order;
                        if (orderUpdateRefresh.Answers != null && orderUpdateRefresh.Answers.Count > 0)
                        {
                            orderUpdateRefresh.Answers.Clear();
                            return orderUpdateRefresh;
                        }
                    }
                    else
                        throw new Exception("Order customer not changed");
                }
                else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleTokenExpired();
                }
                else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.ExpectationFailed)
                {
                    string readErrorContent = await httpResponseMessage.Content.ReadAsStringAsync();
                    //string readErrorContent = responseOrderUpdate.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new JsonGenericDictionaryOrArrayConverter() } };

                    QuestionFaultExceptiom questionFaultExceptiom = JsonConvert.DeserializeObject<QuestionFaultExceptiom>(readErrorContent, settings);
                    //exceptionMessage = questionFaultExceptiom.Message;

                    orderUpdateRefresh.Answers.Add(new QuestionAnswer(questionFaultExceptiom.Code, questionFaultExceptiom.Message));
                    //orderUpdate.Answers.
                }
                else
                {
                    //TODO: Handle failure API's, add logs to server
                }
            }
            catch
            {
            }
            return orderUpdateRefresh;
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            HttpResponseMessage httpResponseMessage;
            try
            {
                httpResponseMessage = await ClientHTTP.GetAsync(GetCompleteURL(url), HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }

            return httpResponseMessage;
        }

        public async Task<HttpResponseMessage> PostAsync(string url, string requestConentString)
        {
            HttpResponseMessage responseMessage = null;
            try
            {
                StringContent requestContent = new StringContent(requestConentString);
                HttpContent content = new StringContent(requestConentString, Encoding.UTF8, mediaType);
                responseMessage = await ClientHTTP.PostAsync(GetCompleteURL(url), content);
            }
            catch
            {
                throw;
            }

            return responseMessage;
        }

        private async Task<HttpResponseMessage> SendAsync(string url, string requestConentString, bool isLoginMethod = false)
        {
            HttpResponseMessage responseMessage;
            try
            {
                var completeUrl = isLoginMethod ? GetLoginURL(url) : GetCompleteURL(url);
                var request = new HttpRequestMessage(HttpMethod.Put, completeUrl);
                request.Content = new StringContent(requestConentString, Encoding.UTF8, mediaType);

                responseMessage = await ClientHTTP.SendAsync(request);
                responseMessage.EnsureSuccessStatusCode();
            }
            catch
            {
                throw;
            }

            return responseMessage;
        }

        private string GetCompleteURL(string url)
        {
            return baseURL + url;
        }

        private string GetLoginURL(string url)
        {
            return baseURL.Replace("V1/", "") + url;
        }

        private void HandleTokenExpired()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.DisplayAlert("Token Expired", "", "OK");
                await Application.Current.MainPage.Navigation.PushModalAsync(new LoginPageNew());
            });
        }
    }
}


