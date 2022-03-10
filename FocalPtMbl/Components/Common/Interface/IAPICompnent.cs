using System.Net.Http;
using System.Threading.Tasks;

namespace FocalPoint
{
    public interface IAPICompnent
    {
        Task<HttpResponseMessage> PostAsyc(string url, string requestConentString);
        Task<HttpResponseMessage> GetAsync(string url);
    }
}
