using FocalPoint.Modules.FrontCounter.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FocalPoint.Modules.FrontCounter.Views.NewRentals;
using FocalPoint.Modules.Dispatching.Views;
using FocalPoint.Modules.ServiceDepartment.Views;
using FocalPoint.Data;
using FocalPtMbl.MainMenu.ViewModels.Services;
using Visum.Services.Mobile.Entities;
using FocalPoint.MainMenu.Views;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FrontCounter : ContentView
    {
        FrontCounterViewModel frontCounterDashboardViewModel;
        public FrontCounter()
        {
            InitializeComponent();
            if (DataManager.Settings != null && string.IsNullOrEmpty(DataManager.Settings.UserName))
                return;
            frontCounterDashboardViewModel = new FrontCounterViewModel();
            BindingContext = frontCounterDashboardViewModel;
            ReloadData();
        }
        public void ReloadData()
        {
            if (frontCounterDashboardViewModel == null) return;

            if (Application.Current.MainPage != null)
                (Application.Current.MainPage as MainMenuFlyout).IsMainDashboardPage = true;

            Device.BeginInvokeOnMainThread(async () =>
            {
                if (frontCounterDashboardViewModel.IsFrontCounterAccess)
                {
                    await frontCounterDashboardViewModel.GetDashboardDetail();
                }
                else
                {
                    await frontCounterDashboardViewModel.GetDashboardHomeDetail();
                }
            });
        }

        private void Counter_Clicked(object sender, System.EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (BindingContext is FrontCounterViewModel frontCounterViewModel)
                {
                    frontCounterViewModel.ChangeButtonStyle(true);
                    grdUtilization.IsVisible = false;
                    RentalCounterDetailView.IsVisible = true;
                    WorkOrderDetailView.IsVisible = true;
                    GrandTotalDetailView.IsVisible = true;
                }
            });
        }

        private void Utilization_Clicked(object sender, System.EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (BindingContext is FrontCounterViewModel frontCounterViewModel)
                {
                    frontCounterViewModel.ChangeButtonStyle(false);
                    grdUtilization.IsVisible = true;
                    RentalCounterDetailView.IsVisible = false;
                    WorkOrderDetailView.IsVisible = false;
                    GrandTotalDetailView.IsVisible = false;
                }
            });
        }

        private void FilterIconTapped(object sender, System.EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await OpenFilterPopup();
                }
                catch (Exception ex)
                {
                    //TODO: Log Error
                }
            });

            async Task OpenFilterPopup()
            {
                FrontCounterViewModel frontCounterViewModel = (BindingContext as FrontCounterViewModel);
                var filterPage = new FrontCounterFilter(frontCounterViewModel.FrontCounterFilterResult);
                await this.Navigation.PushModalAsync(filterPage);
                frontCounterViewModel.FrontCounterFilterResult = await filterPage.Result.Task;
                if (frontCounterViewModel.FrontCounterFilterResult.IsNewDateSet)
                {
                    await frontCounterViewModel.GetDashboardDetail();
                }
            }
        }

        private async void QuickRental_Clicked(object sender, EventArgs e)
        {
            try
            {
                //NewQuickRentalMainPage newQuickRentalMainPage = new NewQuickRentalMainPage();
                //await this.Navigation.PushAsync(newQuickRentalMainPage);

                var NavSer = DependencyService.Resolve<INavigationService>();
                await NavSer.PushPageFromMenu(typeof(NewQuickRentalMainPage), "Quick Rental");
                (Application.Current.MainPage as MainMenuFlyout).IsDashboardAboutToLoad = true;
                (Application.Current.MainPage as MainMenuFlyout).IsMainDashboardPage = false;
            }
            catch (Exception ex)
            {
                //TODO: log error
            }
        }

        private async void Dispatching_Clicked(object sender, EventArgs e)
        {
            try
            {
                ScheduleDispatchingPageView scheduleDispatchingPageView = new ScheduleDispatchingPageView();

                var NavSer = DependencyService.Resolve<INavigationService>();
                NavSer.PushChildPage(scheduleDispatchingPageView);
                (Application.Current.MainPage as MainMenuFlyout).IsDashboardAboutToLoad = true;
                (Application.Current.MainPage as MainMenuFlyout).IsMainDashboardPage = false;

                //await this.Navigation.PushAsync(scheduleDispatchingPageView);
            }
            catch (Exception ex)
            {
                //TODO: log error
            }
        }

        private async void ServiceDept_Clicked(object sender, EventArgs e)
        {
            try
            {
                WorkOrderFormView workOrderFormView = new WorkOrderFormView();

                var NavSer = DependencyService.Resolve<INavigationService>();
                NavSer.PushChildPage(workOrderFormView);
                (Application.Current.MainPage as MainMenuFlyout).IsDashboardAboutToLoad = true;
                (Application.Current.MainPage as MainMenuFlyout).IsMainDashboardPage = false;

                //await this.Navigation.PushAsync(workOrderFormView);
            }
            catch (Exception ex)
            {
                //TODO: log error
            }
        }

        private async void Return_Clicked(object sender, EventArgs e)
        {
            try
            {
                ReturnsView returnsView = new ReturnsView();

                var NavSer = DependencyService.Resolve<INavigationService>();
                NavSer.PushChildPage(returnsView);
                (Application.Current.MainPage as MainMenuFlyout).IsDashboardAboutToLoad = true;
                (Application.Current.MainPage as MainMenuFlyout).IsMainDashboardPage = false;

                //await this.Navigation.PushAsync(returnsView);
            }
            catch (Exception ex)
            {
                //TODO: log error
            }
        }

    }
}