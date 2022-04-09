using FocalPoint.Components.Interface;
using FocalPoint.Data;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.Dispatching.ViewModels
{
    public class PickupTicketsSelectViewModel : ThemeBaseViewModel
    {
        #region Properties
        ObservableCollection<PickupTicket> recent;
        public ObservableCollection<PickupTicket> Recent
        {
            get => recent;
            private set
            {
                this.recent = value;
                OnPropertyChanged(nameof(Recent));
            }
        }
        public List<PickupTicket> Tickets { get; set; }
        public PickupTicket SelectedTicket { get; internal set; }
        public IPickupTicketEntityComponent PickupTicketEntityComponent { get; set; }

        #endregion

        #region Constructor
        public PickupTicketsSelectViewModel()
        {
            PickupTicketEntityComponent = new PickupTicketEntityComponent();
            GetPickupTicketInfo();
        }
        #endregion

        #region Methods
        internal void GetPickupTicketInfo()
        {
            Indicator = true;
            try
            {
                PickupTicketEntityComponent.GetPickupTickets().ContinueWith(task =>
                {
                    var pickupTicketList = task.Result;
                    Tickets = task.Result;
                    if (recent == null)
                    {
                        Recent = new ObservableCollection<PickupTicket>(pickupTicketList);
                    }
                    else
                    {
                        Recent.Clear();
                        foreach (var ticket in pickupTicketList)
                        {
                            Recent.Add(ticket);
                        }
                    }
                    Indicator = false;
                });
            }
            catch (Exception ex)
            {

            }
        }
        internal void GetSearchedTicketInfo(string SearchText)
        {
            try
            {
                if (Tickets == null) return;
                if (string.IsNullOrEmpty(SearchText))
                {
                    Tickets.ForEach(x => Recent.Add(x));
                    return;
                }
                var filteredRecords = Tickets.Where(x => x.PuTNo.ToString().Contains(SearchText)).ToList();

                Recent.Clear();
                filteredRecords.ForEach(x => Recent.Add(x));
            }
            catch (Exception ex)
            {

            }
        }
        internal async Task<PickupTicket> GetDetailedTicketInfo(PickupTicket SelectedTicket)
        {
            bool isTrue = true;
            Indicator = true;
            try
            {
                var detailedTicket = await PickupTicketEntityComponent.GetPickupTicket(SelectedTicket.PuTNo.ToString());
                return detailedTicket;
            }
            catch (Exception ex)
            {
                return new PickupTicket();
            }
            finally
            {
                Indicator = false;
            }
        }
        internal PickupTicket GetTicketInfo(object item)
        {
            if (item is PickupTicket pTicket)
                return pTicket;
            return new PickupTicket();
        }
        #endregion
    }
}
