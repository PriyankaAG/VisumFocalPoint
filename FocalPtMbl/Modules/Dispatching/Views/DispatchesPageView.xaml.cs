using FocalPoint.Modules.Dispatching.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Dispatching.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DispatchesPageView : ContentPage
    {
        DispatchesPageViewModel _vm = new DispatchesPageViewModel(null);
        DispatchRowViewModel rowVm;
        public DispatchesPageView(DispatchRowViewModel item)
        {
            try
            {
                InitializeComponent();
                rowVm = item;
                this.BindingContext = item;
                ToolbarItems.Add(new ToolbarItem
                {
                    IconImageSource = "phone.png",
                    Command = PhoneCommand
                });
            }
            catch(Exception ex)
            {

            }
        }

        public Command PhoneCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (string.IsNullOrEmpty(rowVm?.Dispatch?.OriginPhone)) return;
                    var ok = await this.DisplayAlert("FocalPoint", string.Format("Call {0}?", rowVm.Dispatch.OriginPhone), "Yes", "Cancel");
                    if (ok)
                    {
                        await Utils.Ultils.OpenPhoneDialer(rowVm.Dispatch.OriginPhone);
                    }
                });
            }
        }
    }
}