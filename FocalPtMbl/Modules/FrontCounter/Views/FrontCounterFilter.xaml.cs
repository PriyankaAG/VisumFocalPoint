using FocalPoint.Modules.FrontCounter.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FrontCounterFilter : ContentPage
    {

        public TaskCompletionSource<FrontCounterFilterResult> Result = new TaskCompletionSource<FrontCounterFilterResult>();

        public FrontCounterFilter(FrontCounterFilterResult frontCounterFilterResult)
        {
            InitializeComponent();
            datePicker.Date = frontCounterFilterResult.SelectedDate;
        }

        void OnCancelClicked(object sender, EventArgs args)
        {
            this.Navigation.PopAsync();

            this.Result.SetResult(new FrontCounterFilterResult()
            {
                IsNewDateSet = false,
                SelectedDate = datePicker.Date
            });
        }

        async void OnOKClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
            this.Result.SetResult(new FrontCounterFilterResult()
            {
                IsNewDateSet = true,
                SelectedDate = datePicker.Date
            });
        }
    }
}