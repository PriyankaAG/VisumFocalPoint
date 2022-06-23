using FocalPoint.Components.EntityComponents;
using FocalPoint.Components.Interface;
using FocalPoint.Data;
using FocalPtMbl.MainMenu.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.FrontCounter.ViewModels
{
    public class FrontCounterFilterResult
    {
        public bool IsNewDateSet { get; set; }
        public int StoreNo { get; set; }
        public DateTime SelectedDate { get; set; }
    }

    public class OrderDashboardOverviewDetail : OrderDashboardOverview
    {
        public OrderDashboardOverviewDetail(OrderDashboardOverview orderDashboardOverview)
        {
            this.DscrID = orderDashboardOverview.DscrID;
            this.Dscr = orderDashboardOverview.Dscr;
            this.OrderCnt = orderDashboardOverview.OrderCnt;
            this.RentalAmt = orderDashboardOverview.RentalAmt;
            this.MerchAmt = orderDashboardOverview.MerchAmt;
            this.LaborAmt = orderDashboardOverview.LaborAmt;
            this.GrossAmt = orderDashboardOverview.GrossAmt;
            this.AvgTranAmt = orderDashboardOverview.AvgTranAmt;
        }

        public bool IsTotalRow => this.Dscr.Contains("ALL Counter");

        public bool IsGrandTotalRow => this.Dscr.Contains("Grand Total");

        public bool IsCreditRow => this.Dscr.Contains("Credits");
    }

    public class OverviewData : NotificationObject
    {
        public string Header { get; set; }


        private ObservableCollection<OrderDashboardOverviewDetail> _orderDashboardOverviews;
        public ObservableCollection<OrderDashboardOverviewDetail> OrderDashboardOverviews
        {

            get => _orderDashboardOverviews;
            set
            {
                _orderDashboardOverviews = value;
                OnPropertyChanged(nameof(OrderDashboardOverviews));
            }
        }
    }

    public class FrontCounterViewModel : ThemeBaseViewModel
    {
        public IFrontCounterEntityComponent FrontCounterEntityComponent { get; set; }
        public FrontCounterViewModel() : base("Dashboard")
        {
            FrontCounterEntityComponent = new FrontCounterEntityComponent();
            FrontCounterFilterResult = new FrontCounterFilterResult();
            FrontCounterFilterResult.SelectedDate = DateTime.UtcNow;
            FrontCounterFilterResult.StoreNo = DataManager.Settings.HomeStore;
            FrontCounterFilterResult.IsNewDateSet = true;

            IsCounterButtonSelected = true;

            RentalCounterDetail = new OverviewData();
            RentalCounterDetail.Header = "Rental Counter";
            RentalCounterDetail.OrderDashboardOverviews = new ObservableCollection<OrderDashboardOverviewDetail>();

            WorkOrderDetail = new OverviewData();
            WorkOrderDetail.Header = "Work Orders";
            WorkOrderDetail.OrderDashboardOverviews = new ObservableCollection<OrderDashboardOverviewDetail>();

            GrandTotalDetail = new OverviewData();
            GrandTotalDetail.Header = "Grand Total (Revenue For The Day)";
            GrandTotalDetail.OrderDashboardOverviews = new ObservableCollection<OrderDashboardOverviewDetail>();

            ButtonSelectedCommand = new Command<bool>((bool isCounterSelected) => ChangeButtonStyle(isCounterSelected));
        }

        public void ChangeButtonStyle(bool isCounterSelected)
        {
            IsCounterButtonSelected = isCounterSelected;
        }

        private bool _isCounterButtonSelected;
        public bool IsCounterButtonSelected
        {
            get => _isCounterButtonSelected;
            set
            {
                _isCounterButtonSelected = value;
                OnPropertyChanged(nameof(IsCounterButtonSelected));
            }
        }

        private OrderDashboard _orderDashboardDetail;
        public OrderDashboard OrderDashboardDetail
        {
            get => _orderDashboardDetail;
            set
            {
                _orderDashboardDetail = value;
                OnPropertyChanged(nameof(OrderDashboardDetail));
            }
        }

        private OverviewData _rentalCounterDetail;
        public OverviewData RentalCounterDetail
        {
            get => _rentalCounterDetail;
            set
            {
                _rentalCounterDetail = value;
                OnPropertyChanged(nameof(RentalCounterDetail));
            }
        }

        private OverviewData _workOrderDetail;
        public OverviewData WorkOrderDetail
        {
            get => _workOrderDetail;
            set
            {
                _workOrderDetail = value;
                OnPropertyChanged(nameof(WorkOrderDetail));
            }
        }

        private OverviewData _grandTotalDetail;
        public OverviewData GrandTotalDetail
        {
            get => _grandTotalDetail;
            set
            {
                _grandTotalDetail = value;
                OnPropertyChanged(nameof(GrandTotalDetail));
            }
        }

        private FrontCounterFilterResult _frontCounterFilterResult;
        public FrontCounterFilterResult FrontCounterFilterResult
        {
            get => _frontCounterFilterResult;
            set
            {
                _frontCounterFilterResult = value;
                OnPropertyChanged(nameof(FrontCounterFilterResult));
            }
        }

        public ICommand ButtonSelectedCommand { get; }

        public async Task GetDashboardDetail()
        {
            try
            {
                Indicator = true;
                OrderDashboardDetail = await FrontCounterEntityComponent.GetDashboardDetails(FrontCounterFilterResult.StoreNo, FrontCounterFilterResult.SelectedDate);
                RentalCounterDetail.OrderDashboardOverviews = new ObservableCollection<OrderDashboardOverviewDetail>();
                WorkOrderDetail.OrderDashboardOverviews = new ObservableCollection<OrderDashboardOverviewDetail>();
                GrandTotalDetail.OrderDashboardOverviews = new ObservableCollection<OrderDashboardOverviewDetail>();
                if (OrderDashboardDetail != null)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        RentalCounterDetail.OrderDashboardOverviews.Add(new OrderDashboardOverviewDetail(OrderDashboardDetail.Overviews[i]));
                    }

                    for (int i = 5; i < 7; i++)
                    {
                        WorkOrderDetail.OrderDashboardOverviews.Add(new OrderDashboardOverviewDetail(OrderDashboardDetail.Overviews[i]));
                    }

                    OrderDashboardOverviewDetail grantalTotalOverview = (new OrderDashboardOverviewDetail(OrderDashboardDetail.Overviews[7]));
                    GrandTotalDetail.OrderDashboardOverviews.Add(grantalTotalOverview);
                    OnPropertyChanged(nameof(RentalCounterDetail));
                }
            }
            catch (Exception e)
            {
                //TODO: Log Error
            }
            finally
            {
                Indicator = false;
            }
        }
    }
}
