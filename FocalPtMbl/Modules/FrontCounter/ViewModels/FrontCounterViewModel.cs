using FocalPoint.Components.EntityComponents;
using FocalPoint.Components.Interface;
using FocalPtMbl.MainMenu.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.FrontCounter.ViewModels
{

    public class OrderDashboardOverviewDeatail
    {
        public int DscrID { get; set; }
        public string Dscr { get; set; }
        public int OrderCnt { get; set; }
        public decimal RentalAmt { get; set; }
        public decimal MerchAmt { get; set; }
        public decimal LaborAmt { get; set; }
        public decimal GrossAmt { get; set; }
        public decimal AvgTranAmt { get; set; }
    }
    public class FrontCounterViewModel: ThemeBaseViewModel
    {
        public IFrontCounterEntityComponent FrontCounterEntityComponent { get; set; }
        public FrontCounterViewModel() : base("Dashboard")
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

        //private OrderDashboard _OrderDashboardDetail;
        //public OrderDashboard OrderDashboardDetail
        //{
        //    get => OrderDashboardDetail;
        //    set
        //    {
        //        _OrderDashboardDetail = value;
        //        OnPropertyChanged(nameof(OrderDashboardDetail));
        //    }
        //}

        private ObservableCollection<OrderDashboardOverviewDeatail> _rentalCounterList;
        public ObservableCollection<OrderDashboardOverviewDeatail> RentalCounterList
        {
            get => _rentalCounterList;
            set
            {
                _rentalCounterList = value;
                OnPropertyChanged(nameof(RentalCounterList));
            }
        }

        //private ObservableCollection<OrderDashboardOverview> _workOrderList;
        //public ObservableCollection<OrderDashboardOverview> WorkOrderList
        //{
        //    get => _workOrderList;
        //    set
        //    {
        //        _workOrderList = value;
        //        OnPropertyChanged(nameof(WorkOrderList));
        //    }
        //}

        //private ObservableCollection<OrderDashboardOverview> _grandTotalList;
        //public ObservableCollection<OrderDashboardOverview> GrandTotalList
        //{
        //    get => _grandTotalList;
        //    set
        //    {
        //        _grandTotalList = value;
        //        OnPropertyChanged(nameof(GrandTotalList));
        //    }
        //}

        //public OrderDashboard GetDetails()
        //{
        //    RentalCounterList = new ObservableCollection<OrderDashboardOverview>();

        //    RentalCounterList.Add(new OrderDashboardOverview()
        //    {
        //        Dscr = "Quick Sales",
        //        AvgTranAmt = 2,
        //        DscrID = 2,
        //        GrossAmt = 2,
        //        LaborAmt = 1,
        //        MerchAmt = 1,
        //        OrderCnt = 1,
        //        RentalAmt = 1
        //    });

        //    RentalCounterList.Add(new OrderDashboardOverview()
        //    {
        //        Dscr = "Quick Sales",
        //        AvgTranAmt = 2,
        //        DscrID = 2,
        //        GrossAmt = 2,
        //        LaborAmt = 1,
        //        MerchAmt = 1,
        //        OrderCnt = 1,
        //        RentalAmt = 1
        //    });
        //    return OrderDashboardDetail;
        //}

        //public OrderDashboard GetDashboardDetail()
        //{
        //    try
        //    {
        //        Indicator = true;
        //        OrderDashboard orderDashboardDetail = new OrderDashboard();
        //        orderDashboardDetail.Overviews = new System.Collections.Generic.List<OrderDashboardOverview>();
        //        orderDashboardDetail.Overviews.Add(new OrderDashboardOverview()
        //        {
        //            Dscr = "Closed Orders",
        //            AvgTranAmt = 1,
        //            DscrID = 1,
        //            GrossAmt = 1,
        //            LaborAmt = 1,
        //            MerchAmt = 1,
        //            OrderCnt = 1,
        //            RentalAmt = 1
        //        });
        //        orderDashboardDetail.Overviews.Add(new OrderDashboardOverview()
        //        {
        //            Dscr = "Quick Sales",
        //            AvgTranAmt = 2,
        //            DscrID = 2,
        //            GrossAmt = 2,
        //            LaborAmt = 1,
        //            MerchAmt = 1,
        //            OrderCnt = 1,
        //            RentalAmt = 1
        //        });

        //        //OrderDashboardDetail = await FrontCounterEntityComponent.GetDashboardDetails(SelectedDate);
        //    }
        //    catch (Exception e)
        //    {
        //        //TODO: Log Error
        //    }
        //    finally
        //    {
        //        Indicator = false;
        //    }
        //    return OrderDashboardDetail;
        //}
    }
}
