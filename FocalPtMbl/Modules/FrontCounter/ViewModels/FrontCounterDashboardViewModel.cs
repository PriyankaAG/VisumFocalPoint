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

        private OrderDashboard _OrderDashboardDetail;
        public OrderDashboard OrderDashboardDetail
        {
            get => OrderDashboardDetail;
            set
            {
                _OrderDashboardDetail = value;
                OnPropertyChanged(nameof(OrderDashboardDetail));
            }
        }

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

        public OrderDashboard GetDashboardDetail()
        {
            try
            {
                Indicator = true;
                OrderDashboard orderDashboardDetail = new OrderDashboard();
                orderDashboardDetail.Overviews = new System.Collections.Generic.List<OrderDashboardOverview>();
                orderDashboardDetail.Overviews.Add(new OrderDashboardOverview()
                {
                    Dscr = "Closed Orders",
                    AvgTranAmt = 1,
                    DscrID = 1,
                    GrossAmt = 1,
                    LaborAmt = 1,
                    MerchAmt = 1,
                    OrderCnt = 1,
                    RentalAmt = 1
                });
                orderDashboardDetail.Overviews.Add(new OrderDashboardOverview()
                {
                    Dscr = "Quick Sales",
                    AvgTranAmt = 2,
                    DscrID = 2,
                    GrossAmt = 2,
                    LaborAmt = 1,
                    MerchAmt = 1,
                    OrderCnt = 1,
                    RentalAmt = 1
                });

                //OrderDashboardDetail = await FrontCounterEntityComponent.GetDashboardDetails(SelectedDate);
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
