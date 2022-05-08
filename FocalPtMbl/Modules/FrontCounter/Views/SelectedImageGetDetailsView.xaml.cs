using FocalPoint.Modules.FrontCounter.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectedImageGetDetailsView : ContentPage
    {
        SelectedImageGetDetailsViewModel SelectedImageGetDetailsViewModel { get; set; }
        public SelectedImageGetDetailsView(Order selectedOrder, string fileName, Stream stream)
        {
            InitializeComponent();
            SelectedImageGetDetailsViewModel = new SelectedImageGetDetailsViewModel(selectedOrder, fileName, stream);
            this.BindingContext = SelectedImageGetDetailsViewModel;
        }

        private void TextEdit_ClearIconClicked(object sender, System.ComponentModel.HandledEventArgs e)
        {

        }

        private async void SimpleButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void SimpleButton_Clicked_1(object sender, EventArgs e)
        {
            bool success = await SelectedImageGetDetailsViewModel.SaveCaptureImage();
            if (success)
            {
                await DisplayAlert("FocalPoint Mobile", "Image saved successfully.", "OK");
            }
            else
            {
                await DisplayAlert("FocalPoint Mobile", "Failed to Save Image", "OK");
            }
            await Navigation.PopAsync();
        }
    }
}