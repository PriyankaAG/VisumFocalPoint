using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FocalPoint.Components.Common.Interface
{
    public interface IAPICompnent
    {
        Task<HttpResponseMessage> PostAsyc(string url, string requestConentString);
        Task<HttpResponseMessage> GetAsync(string url);
    }
}
