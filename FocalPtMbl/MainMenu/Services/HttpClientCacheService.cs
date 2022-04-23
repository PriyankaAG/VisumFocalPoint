using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace FocalPoint.MainMenu.Services
{
    /// <summary>
    /// Factory instance for making HttpClients and caching them.
    /// </summary>
    public class HttpClientCacheService : IHttpClientCacheService
    {
        /// <summary>
        /// Maximum number of clients to cache
        /// </summary>
        public static readonly int MAX_CLIENT = 5;

        /// <summary>
        /// Syncronizaton object for guarding the cache.
        /// </summary>
        private readonly object sync = new object();

        /// <summary>
        /// Dictionary of clients.
        /// </summary>
        private Dictionary<string, Tuple<HttpConfig, HttpClient>> mClients;

        /// <summary>
        /// List used as a LIFO for removing clients when the maximum number of clients is reached.
        /// </summary>
        private List<HttpConfig> mClientSequence;

        /// <summary>
        /// Constructor for HTTP Client Cache. NOTE.
        /// </summary>
        /// <remarks>
        /// Use dependency injection and register this class as a singlton
        /// </remarks>
        public HttpClientCacheService()
        {
            mClients = new Dictionary<string, Tuple<HttpConfig, HttpClient>>();
            mClientSequence = new List<HttpConfig>();
        }

        /// <summary>
        /// Gets or set the Base URL to use when making HTTP Clients
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or set the Store to use when making HTTP Clients
        /// </summary>
        public string Store { get; set; }

        /// <summary>
        /// Gets or set the Terminal to use when making HTTP Clients
        /// </summary>
        public string Terminal { get; set; }

        /// <summary>
        /// Gets or set the Token to use when making HTTP Clients
        /// </summary>
        public string Token { get; set; }
        public string User { get; set; }

        public void AddClient(string baseURL,  string store, string terminal, string token, string user, HttpClient client)
        {
           lock(this.sync)
            {
                var config = new HttpConfig(baseURL, store, terminal, token, user);
                mClients.Add(config.UniqueKey, new Tuple<HttpConfig, HttpClient>(config, client));
                this.mClientSequence.Add(config);
            }
        }

        /// <summary>
        /// Clears all clients from the cache associated with the token.
        /// </summary>
        /// <remarks>
        /// Use this method to clear out clients then the token is no longer valid.
        /// </remarks>
        /// <param name="token"></param>
        public void Clear(string token)
        {
            List<HttpConfig> configKeys = new List<HttpConfig>(5);
            foreach (var item in this.mClients)
            {
                if (item.Value.Item1.Token == token)
                    configKeys.Add(item.Value.Item1);
            }

            foreach (var config in configKeys)
            {
                this.mClients.Remove(config.UniqueKey);
                if (this.mClientSequence.Contains(config))
                    this.mClientSequence.Remove(config);
            }
        }

        /// <summary>
        /// Gets an HTTP Client associated with the current Base URL, Store, Terminal, and Token.
        /// </summary>
        /// <returns></returns>
        public HttpClient GetHttpClientAsync()
        {
            return GetHttpClientAsync(
                new HttpConfig(
                    this.BaseUrl,
                    this.Store,
                    this.Terminal,
                    this.Token,
                    this.User));
        }
        public HttpClient GetHttpClientAsync(HttpConfig config)
        {
            HttpClient httpClient;

            lock (this.sync)
            {
                if (mClients.ContainsKey(config.UniqueKey))
                {
                    httpClient = mClients[config.UniqueKey].Item2;
                }
                else
                {
                    if (mClientSequence.Count >= MAX_CLIENT)
                    {
                        // Remove the first item
                        var removingClientConfig = mClientSequence[0];
                        mClientSequence.RemoveAt(0);
                        mClients.Remove(removingClientConfig.UniqueKey);
                    }

                    httpClient = new HttpClient();
                    // var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes("c6760347-c341-47c6-9d90-8171615edc92"));
                    // clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                   
                    httpClient.DefaultRequestHeaders.Add("Token", config.Token);
                    httpClient.DefaultRequestHeaders.Add("StoreNo", config.Store);
                    httpClient.DefaultRequestHeaders.Add("TerminalNo", config.Terminal);
                    httpClient.DefaultRequestHeaders.Add("User", config.User);

                    mClients[config.UniqueKey] = new Tuple<HttpConfig, HttpClient>(config, httpClient);
                    mClientSequence.Add(config);
                }
            }

            return httpClient;
        }
    }
}