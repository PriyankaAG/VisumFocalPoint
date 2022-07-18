using FocalPoint.Modules.FrontCounter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectSerialOnlyView : ContentPage
    {
        public TaskCompletionSource<List<string>> Result = new TaskCompletionSource<List<string>>();
        public SelectSerialOnlyView(AvailabilityMerch selectedItem)
        {
            this.Title = "Select Serials";
            InitializeComponent();
            ((SelectSerialsOnlyViewModel)this.BindingContext).SelectedItem = selectedItem;
            Device.BeginInvokeOnMainThread(async () =>
            {
                await ((SelectSerialsOnlyViewModel)this.BindingContext).GetSerials(selectedItem);
            });
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void Continue_Clicked(object sender, EventArgs e)
        {
            SelectSerialsOnlyViewModel SelectSerialsOnlyViewModel = (SelectSerialsOnlyViewModel)this.BindingContext;
            if (SelectSerialsOnlyViewModel.SelectedSerials?.Count() > 0)
            {
                Result.SetResult(SelectSerialsOnlyViewModel.SelectedSerials.Select(r => r.MerchSerSerial).ToList());
                Navigation.PopAsync();
            }
            else
            {
                DisplayAlert("Error", "Select Serial Numbers", "ok", "cancel");
            }
        }

        private void Serial_Tap(object sender, DevExpress.XamarinForms.CollectionView.CollectionViewGestureEventArgs e)
        {
            var item = e.Item as MerchandiseSerial;
            if (item != null)
                ((SelectSerialsOnlyViewModel)this.BindingContext).AddToSelectedSerial(item);
        }
    }
}