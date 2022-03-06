using FocalPtMbl.MainMenu.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.Dispatching.ViewModels
{
    public class TruckPageViewModel : ThemeBaseViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public DispatchesPageViewModel DispatchVm { get; set; }


        ///// <summary>
        ///// 
        ///// </summary>
        public Truck Truck { get; set; }


        ///// <summary>
        ///// 
        ///// </summary>
       public bool ShowDescription
        {
            get
            {
                return this.Truck != null;
            }
        }


        ///// <summary>
        ///// The current dispatches for the date selected
        ///// </summary>
        ObservableCollection<DispatchRowViewModel> _dispatches = null;
        public ObservableCollection<DispatchRowViewModel> Dispatches
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


        ///// <summary>
        ///// 
        ///// </summary>
        public TruckPageViewModel(DispatchesPageViewModel dvm, Truck truck)
        {
            this.DispatchVm = dvm;
            this.Truck = truck;
            this.Dispatches = new ObservableCollection<DispatchRowViewModel>();
        }
    }
}