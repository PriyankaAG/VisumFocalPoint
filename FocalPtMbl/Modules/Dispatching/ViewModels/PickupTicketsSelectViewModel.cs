using FocalPoint.Components.Interface;
using FocalPoint.Data;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
        public PickupTicket SelectedTicket { get; internal set; }
        public IPickupTicketEntityComponent PickupTicketEntityComponent { get; set; }
        #endregion

        #region Constructor
        public PickupTicketsSelectViewModel()
        {
            PickupTicketEntityComponent = new PickupTicketEntityComponent();
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
        internal void GetSearchedTicketInfo(object SearchText)
        {
            try
            {
                GetPickupTicketInfo();
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
        #endregion
    }
}
