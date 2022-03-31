using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FocalPoint
{
    public interface ISettingsComponent
        : IComponent
    {
        Task<string> GetSecure(string key);

        Task SetSecure(string key, string value);

        void RemoveSecure(string key);

        void ClearSecure();

        void TrackError(Exception exception, IDictionary<string, string> properties = null, [CallerMemberName] string caller = null);
    }
}
