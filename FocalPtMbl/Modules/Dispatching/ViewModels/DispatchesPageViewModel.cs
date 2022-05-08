using FocalPtMbl.MainMenu.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.Dispatching.ViewModels
{
    public class DispatchesPageViewModel : ThemeBaseViewModel
    {
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

        internal List<Truck> GetTrucks()
        {
            //  throw new NotImplementedException();
            return new List<Truck>();
        }

        internal void Search()
        {
            try
            {

                var dispatches = new List<Dispatches>();// = await ApiClient.Dispatches(this.SearchDate);




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
            catch (Exception e)
            {
                var msg = e.Message;
            }
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
            //this.Title = "Dispatching";
            //this.SearchDate = DateTime.Now.Date;
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
