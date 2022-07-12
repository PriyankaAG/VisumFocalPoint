using System.Net.Http;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint
{
    public interface IAPICompnent
    {
        Task<T> GetAsync<T>(string url);

        Task<HttpResponseMessage> GetAsync(string url);

        Task<T> PostAsync<T>(string url, string requestContent);

        Task<T> PutAsync<T>(string url, string requestContent);

        Task<HttpResponseMessage> PostAsync(string url, string requestConentString);

        Task<T> SendAsync<T>(string url, string requestConentString, bool isLoginMethod = false);

        void AddStoreToHeader(string storeNo);
        void AddTerminalToHeader(string terminalNo);

        Task<OrderUpdate> SendAsyncUpdateOrder(string url, string requestContent, bool isLoginMethod = false);

        Task<OrderDtlUpdate> SendAsyncUpdateOrderDetails(string url, string requestContent);
    }
}
