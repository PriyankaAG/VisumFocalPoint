using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace FocalPoint.MainMenu.Services
{
    public interface IHttpClientCacheService
    {
        string BaseUrl { get; set; }
        string Store { get; set; }
        string Terminal { get; set; }
        string Token { get; set; }
        string User { get; set; }

        void Clear(string token);
        HttpClient GetHttpClientAsync();
        HttpClient GetHttpClientAsync(HttpConfig config);
        void AddClient(string baseURL,  string store, string terminal, string token, string user, HttpClient client);
    }
}
