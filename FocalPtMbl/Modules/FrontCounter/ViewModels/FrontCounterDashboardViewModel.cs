using FocalPoint.Components.EntityComponents;
using FocalPoint.Components.Interface;
using FocalPtMbl.MainMenu.ViewModels;
using System;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.FrontCounter.ViewModels
{
    public class FrontCounterDashboardViewModel : ThemeBaseViewModel
    {
        public IFrontCounterEntityComponent FrontCounterEntityComponent { get; set; }

        public OrderDashboard OrderDashboardDetail { get; set; }

        public FrontCounterDashboardViewModel() : base("Dashboard")
        {
            FrontCounterEntityComponent = new FrontCounterEntityComponent();
            SelectedDate = DateTime.UtcNow;
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        public async Task<OrderDashboard> GetDashboardDetail()
        {
            try
            {
                Indicator = true;
                OrderDashboardDetail = await FrontCounterEntityComponent.GetDashboardDetails(SelectedDate);
            }
            catch (Exception e)
            {
                //TODO: Log Error
            }
            finally
            {
                Indicator = false;
            }
            return OrderDashboardDetail;
        }
    }
}
