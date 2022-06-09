using System.Net.Http;
using System.Threading.Tasks;

namespace FocalPoint
{
    public interface IAPICompnent
    {
        Task<T> GetAsync<T>(string url);

        Task<HttpResponseMessage> GetAsync(string url);

        Task<T> PostAsync<T>(string url, string requestContent);

        Task<HttpResponseMessage> PostAsync(string url, string requestConentString);

        Task<T> SendAsync<T>(string url, string requestConentString, bool isLoginMethod);

        void AddStoreToHeader(string storeNo);
        void AddTerminalToHeader(string terminalNo);
    }
}
