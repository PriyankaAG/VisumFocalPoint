using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPoint.MainMenu.Services
{
    public interface IDeviceInfo
    {
        string DeviceId { get; }
        string ModelNumber { get; }
        string Manufacturer { get; }
    }
}
