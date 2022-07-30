using FocalPoint.Data.API;
using FocalPoint.Modules.FrontCounter.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.FrontCounter.ViewModels
{
    public class ViewOrderDetailsViewModel : CommonViewModel
    {
        public ViewOrderDetailsViewModel()
        {
            ViewOrderEntityComponent = new ViewOrderEntityComponent();
            Images = new List<OrderImageViewModel>();
            ImageList = new ObservableCollection<ImageList>();
        }        

        public Order SelectedOrder { get; internal set; }
        public List<OrderImageViewModel> Images { get; set; }

        private ObservableCollection<ImageList> _imageList;
        public ObservableCollection<ImageList> ImageList 
        {
            get => _imageList;
            set => SetProperty(ref _imageList, value);
        }

        public IViewOrderEntityComponent ViewOrderEntityComponent { get; set; }

        public string CustomerPhoneTypeText
        {
            get
            {
                if (this.SelectedOrder.Customer.CustomerPhoneType == "F")
                    return "Fax:  ";

                if (this.SelectedOrder.Customer.CustomerPhoneType == "C")
                    return "Cell:  ";

                if (this.SelectedOrder.Customer.CustomerPhoneType == "H")
                    return "Home:  ";

                if (this.SelectedOrder.Customer.CustomerPhoneType == "W")
                    return "Work:  ";

                return string.Empty;
            }
        }

        public string CustomerPhoneType2Text
        {
            get
            {
                if (this.SelectedOrder.Customer.CustomerPhoneType2 == "F")
                    return "Fax:  ";

                if (this.SelectedOrder.Customer.CustomerPhoneType2 == "C")
                    return "Cell:  ";

                if (this.SelectedOrder.Customer.CustomerPhoneType2 == "H")
                    return "Home:  ";

                if (this.SelectedOrder.Customer.CustomerPhoneType2 == "W")
                    return "Work:  ";

                return string.Empty;
            }
        }

        public string CustomerTypeText
        {
            get
            {
                if (this.SelectedOrder.Customer.CustomerType == "C")
                    return "Cash";

                if (this.SelectedOrder.Customer.CustomerType == "R")
                    return "Charge";

                if (this.SelectedOrder.Customer.CustomerType == "M")
                    return "Miscellaneous";

                if (this.SelectedOrder.Customer.CustomerType == "O")
                    return "Open";

                return string.Empty;
            }
        }

        //interface bool CanDrawSignature()
        //{
        //}
        internal async Task<ObservableCollection<OrderImageDetail>> GetOrderImagesAsync()
        {
            var ordImages = new ObservableCollection<OrderImageDetail>();
            var orderImages = await ViewOrderEntityComponent.GetOrderImages(SelectedOrder.OrderNo.ToString());
            if (orderImages != null)
                foreach (var item in orderImages)
                {
                    ordImages.Add(item);
                }
            return ordImages;
        }
    }

    public class ImagesTemp
    {
        public ImageSource Image { get; set; }
        public string Text { get; set; }
    }
}
