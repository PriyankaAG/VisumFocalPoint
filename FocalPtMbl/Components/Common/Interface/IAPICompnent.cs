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
        
    }
}
