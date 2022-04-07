using FocalPoint.Components.EntityComponents;
using FocalPoint.Components.Interface;
using FocalPtMbl.MainMenu.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.Dispatching.ViewModels
{
    public class DispatchesPageViewModel : ThemeBaseViewModel
    {
        public IDispatchingEntityComponent DispatchingEntityComponent { get; set; }
        List<Truck> trucks;
        public List<Truck> Trucks
        {
            get => trucks;
            private set
            {
                this.trucks = value;
                OnPropertyChanged(nameof(trucks));
            }
        }

        DateTime _searchDate;
        public DateTime SearchDate
        {
            get
            {
                return _searchDate;
            }
            set
            {
                if (_searchDate == value)
                    return;

                _searchDate = value;
                OnPropertyChanged("SearchDate");
            }
        }

        internal async Task<List<Truck>> GetTrucks()
        {
            try
            {
                var trucksList = await DispatchingEntityComponent.GetTrucks();

                if (trucks == null)
                {
                    Trucks = new List<Truck>(trucksList);
                }
                else
                {
                    Trucks.Clear();
                    foreach (var truck in trucksList)
                    {
                        Trucks.Add(truck);
                    }
                }
                //TO test ActivityIndicator
                await Task.Delay(500);
                return Trucks;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal async Task Search()
        {
            _ = Task.Run(async () =>
             {
                 try
                 {
                     Indicator = true;
                     await DispatchingEntityComponent.GetDispatches(this.SearchDate).ContinueWith(task =>
                     {
                         lock (this)
                         {
                             var dispatches = task.Result;

                             this.AllDispatches.Clear();

                             foreach (var t in this.TruckViewModels)
                                 t.Dispatches.Clear();

                             if (dispatches != null)
                             {
                                 foreach (var d in dispatches)
                                 {
                                     var tvm = this.TruckViewModels.SingleOrDefault(x => x.Truck.TruckNo == d.DispatchTruckID);

                                     if (tvm != null)
                                         tvm.Dispatches.Add(new DispatchRowViewModel(d));

                                     this.AllDispatches.Add(new DispatchRowViewModel(d));
                                 }
                             }
                         }

                     });

                 }
                 catch (Exception e)
                 {
                     var msg = e.Message;
                 }
                 finally
                 {
                     Indicator = false;
                 }
             });
        }

        ObservableCollection<DispatchRowViewModel> _dispatches = null;
        public ObservableCollection<DispatchRowViewModel> AllDispatches
        {
            get
            {
                return _dispatches;
            }
            set
            {
                _dispatches = value;
                OnPropertyChanged("Dispatches");
            }
        }
        List<TruckPageViewModel> _truckViewModels = null;
        public List<TruckPageViewModel> TruckViewModels
        {
            get
            {
                return _truckViewModels;
            }
            set
            {
                _truckViewModels = value;
                OnPropertyChanged("TruckViewModels");
            }
        }
        public DispatchesPageViewModel(List<Truck> trucks)
        {
            this.SearchDate = DateTime.Now.Date;
            DispatchingEntityComponent = new DispatchingEntityComponent();

            this.AllDispatches = new ObservableCollection<DispatchRowViewModel>();
            this.TruckViewModels = new List<TruckPageViewModel>();

            if (trucks != null)
            {
                foreach (var t in trucks)
                    this.TruckViewModels.Add(new TruckPageViewModel(this, t));
            }
        }
    }
}
