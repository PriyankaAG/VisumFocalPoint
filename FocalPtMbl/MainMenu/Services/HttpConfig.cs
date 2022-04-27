using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPoint.MainMenu.Services
{
    public class HttpConfig
    {
        // Items that make the configuration unique.
        readonly string token;
        readonly string baseUrl;
        readonly string store;
        readonly string terminal;
        readonly string uniqueKey;
        readonly string user;

        // BaseUrl, Timeout, Headers, Auth info, etc
        //int Timeout = 1000;
        //string headerextra = string.Empty;
        //string headerextra2 = string.Empty;
        //string headerextra3 = string.Empty;

        public HttpConfig(string baseUrl, string store, string terminal, string token, string user)
        {
            this.baseUrl = baseUrl;
            this.store = store;
            this.terminal = terminal;
            this.token = token;
            this.user = user;

            this.uniqueKey = $"{this.baseUrl}\\{this.store}\\{this.terminal}\\{this.token}\\{this.user}";
        }

        /// <summary>
        /// Get the token for the HTTP Client header (session)
        /// </summary>
        public string Token  => this.token;

        /// <summary>
        /// Gets the unique key used to identify an HTTP Client in the HttpClientCacheSErvice.
        /// </summary>
        public string UniqueKey  => this.uniqueKey;

        public string BaseUrl => baseUrl;

        public string Store => store;

        public string Terminal => terminal;
     
        public string User => user;
    }
}
