using FocalPoint.Data.API;
using FocalPoint.Utils;
using FocalPtMbl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace FocalPoint.Modules.FrontCounter.ViewModels
{
	public class OrderImageViewModel : BindableObject
	{
		public OrderImageDetail OrderImage { get; set; }
		
		private Xamarin.Forms.ImageSource _image;
		public ImageSource Image
		{
			get
			{
				if (_image == null)
				{
					try
					{
						byte[] bytes = Convert.FromBase64String(ImageBase64);						
						_image = Xamarin.Forms.ImageSource.FromStream(() => new MemoryStream(bytes));
					}
					catch (Exception e)
					{
						
					}
				}
				return _image;
			}
		}
		public string Text { get; set; }


		private string imageBase64;
		public string ImageBase64
		{
			get { return imageBase64; }
			set
			{
				imageBase64 = value;
				OnPropertyChanged("ImageBase64");
				OnPropertyChanged("Image");

			}
		}

		public OrderImageViewModel(OrderImageDetail image, bool uncompress = false)
		{
			if (uncompress)
			{
				image.Image = Ultils.UnCompressImage(image.Image);
			}

			OrderImage = image;
			ImageBase64 = OrderImage.Image;
			Text = image.OrderImageName;
		}
	}
}
