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
        static object locker = new object();

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

        public async Task<T> PutAsync<T>(string url, string requestContent)
        {
            T typedRequestContent = default;
            try
            {
                HttpResponseMessage httpResponseMessage = await PutAsync(url, requestContent);
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
                    if (string.IsNullOrEmpty(content))
                    {
                        //success
                    }
                    else
                    {
                        orderUpdateRefresh = JsonConvert.DeserializeObject<OrderUpdate>(content);
                        //check if empty result
                        if (orderUpdateRefresh != null && orderUpdateRefresh.Order != null)
                        {
                            if (orderUpdateRefresh.Answers != null && orderUpdateRefresh.Answers.Count > 0)
                            {
                                orderUpdateRefresh.Answers.Clear();
                                return orderUpdateRefresh;
                            }
                        }
                    }
                }
                else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleTokenExpired();
                }
                else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.ExpectationFailed)
                {
                    string readErrorContent = await httpResponseMessage.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new JsonGenericDictionaryOrArrayConverter() } };
                    QuestionFaultExceptiom questionFaultExceptiom = JsonConvert.DeserializeObject<QuestionFaultExceptiom>(readErrorContent, settings);
                    orderUpdateRefresh.Answers.Add(new QuestionAnswer(questionFaultExceptiom.Code, questionFaultExceptiom.Message));
                }
                else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotAcceptable)
                {
                    string readErrorContent = await httpResponseMessage.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new JsonGenericDictionaryOrArrayConverter() } };
                    string errorMessage= JsonConvert.DeserializeObject<string>(readErrorContent, settings);
                    orderUpdateRefresh.NotAcceptableErrorMessage = errorMessage;
                }
            }
            catch
            {
            }
            return orderUpdateRefresh;
        }

        public async Task<OrderUpdate> SendAsyncUpdateOrderDetails(string url, string requestContent)
        {
            OrderUpdate orderDetailUpdateRefresh = new OrderUpdate();
            try
            {
                HttpResponseMessage httpResponseMessage = await SendAsync(url, requestContent, false);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string content = await httpResponseMessage.Content.ReadAsStringAsync();
                    orderDetailUpdateRefresh = JsonConvert.DeserializeObject<OrderUpdate>(content);
                    if (orderDetailUpdateRefresh != null)
                    {
                        if (orderDetailUpdateRefresh.Answers != null && orderDetailUpdateRefresh.Answers.Count > 0)
                        {
                            orderDetailUpdateRefresh.Answers.Clear();
                            return orderDetailUpdateRefresh;
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
                    var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new JsonGenericDictionaryOrArrayConverter() } };

                    QuestionFaultExceptiom questionFaultExceptiom = JsonConvert.DeserializeObject<QuestionFaultExceptiom>(readErrorContent, settings);

                    orderDetailUpdateRefresh.Answers.Add(new QuestionAnswer(questionFaultExceptiom.Code, questionFaultExceptiom.Message));
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
            return orderDetailUpdateRefresh;
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

        public async Task<HttpResponseMessage> PutAsync(string url, string requestConentString)
        {
            HttpResponseMessage responseMessage = null;
            try
            {
                StringContent requestContent = new StringContent(requestConentString);
                HttpContent content = new StringContent(requestConentString, Encoding.UTF8, mediaType);
                responseMessage = await ClientHTTP.PutAsync(GetCompleteURL(url), content);
            }
            catch
            {
                throw;
            }

            return responseMessage;
        }

        public async Task<HttpResponseMessage> SendAsync(string url, string requestConentString, bool isLoginMethod = false)
        {
            HttpResponseMessage responseMessage;
            try
            {
                var completeUrl = isLoginMethod ? GetLoginURL(url) : GetCompleteURL(url);
                var request = new HttpRequestMessage(HttpMethod.Put, completeUrl);
                request.Content = new StringContent(requestConentString, Encoding.UTF8, mediaType);

                responseMessage = await ClientHTTP.SendAsync(request);
            }
            catch (Exception ex)
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

        public void HandleTokenExpired()
        {
            lock (locker)
            {
                if (DataManager.IsTokenExpired == false)
                {
                    DataManager.IsTokenExpired = true;
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        DataManager.ClearSettings();
                        await Application.Current.MainPage.DisplayAlert("Token Expired", "", "OK");
                        await Application.Current.MainPage.Navigation.PushModalAsync(new LoginPageNew());
                    });
                }
            }
        }
    }
}


