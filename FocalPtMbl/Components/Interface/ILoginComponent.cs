using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Components.Interface
{
    public interface ILoginComponent
    {
        Task<Guid> UserLogin(string url, string requestString);
        Task<bool> Logoff();
        Task<List<Company>> GetLoginStores();
    }
}
