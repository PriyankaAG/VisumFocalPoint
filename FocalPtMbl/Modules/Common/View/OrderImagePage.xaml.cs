using FocalPoint.Modules.FrontCounter.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Common.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OrderImagePage : ContentPage
	{
		OrderImageViewModel _imageVM = null;

		public OrderImagePage(OrderImageViewModel imageVM)
		{
			InitializeComponent();

			_imageVM = imageVM;

			this.Title = _imageVM.OrderImage.OrderImageName;
		}


		async protected override void OnAppearing()
		{
			base.OnAppearing();

			try
			{
				//TODO: 
				//var compressed = await ApiClient.OrderImage(_imageVM.DlOrderImage.ImageID.Value);

				//var svc = DependencyService.Get<ICompressSvc>();
				//var bytes = Convert.FromBase64String(svc.Uncompress(compressed));

				//DisplayImage.Source = Xamarin.Forms.ImageSource.FromStream(() => new MemoryStream(bytes));


				//App.Platform.Dismiss();
			}
			catch (Exception e)
			{
				var msg = e.Message;
				//await FPDisplayAlert.DisplayAlert("FocalPoint Connect", "There was an error loading the image.");
			}
			finally
			{
			}
		}
	}

	public class ImageDisplay
	{
		public Image Image { get; set; }
	}
}