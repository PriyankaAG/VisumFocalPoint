using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Components.Interface
{
    public interface IDispatchingEntityComponent
    {
        Task<List<Truck>> GetTrucks();
        Task<List<Dispatches>> GetDispatches(DateTime searchDate);
    }
}
