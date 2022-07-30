using FocalPoint.Data;
using FocalPoint.Data.DataModel;
using FocalPoint.Modules.FrontCounter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FrontCounterFilter : ContentPage
    {
        List<Store> stores;
        public TaskCompletionSource<FrontCounterFilterResult> Result = new TaskCompletionSource<FrontCounterFilterResult>();

        public FrontCounterFilter(FrontCounterFilterResult frontCounterFilterResult)
        {
            InitializeComponent();
            datePicker.Date = frontCounterFilterResult.SelectedDate;
            stores = DataManager.LoadStores();
            storePicker.ItemsSource = stores;
            storePicker.SelectedItem = stores.FirstOrDefault(s => s.CmpNo == frontCounterFilterResult.StoreNo);
        }

        void OnCancelClicked(object sender, EventArgs args)
        {
            this.Navigation.PopModalAsync();
            Store store = (Store)storePicker.SelectedItem;
            if (store != null)
            {
                this.Result.SetResult(new FrontCounterFilterResult()
                {
                    IsNewDateSet = false,
                    StoreNo = store.CmpNo,
                    SelectedDate = datePicker.Date
                });
            }
        }

        async void OnOKClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
            Store store = (Store)storePicker.SelectedItem;
            if (store != null)
            {
                this.Result.SetResult(new FrontCounterFilterResult()
                {
                    IsNewDateSet = true,
                    StoreNo = store.CmpNo,
                    SelectedDate = datePicker.Date
                });
            }
        }
    }
}