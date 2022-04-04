using FocalPoint.Data;
using FocalPoint.Data.API;
using FocalPoint.Models;
using FocalPoint.Utils;
using FocalPtMbl.MainMenu.ViewModels;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.FrontCounter.ViewModels
{
    public class SelectedImageGetDetailsViewModel : ThemeBaseViewModel
    {
        public SelectedImageGetDetailsViewModel(Order selectedOrder, string fileName, Stream stream) : base("Selected Image Information")
        {
            SelectedOrder = selectedOrder;
            OriginalImageName  = fileName;
            ImageName = Path.GetFileName(fileName);
            imageStream = stream;
            ViewOrderEntityComponent = new ViewOrderEntityComponent();
        }

        public IViewOrderEntityComponent ViewOrderEntityComponent { get; set; }

        public Order SelectedOrder { get; internal set; }
        public Stream imageStream { get; internal set; }

        private OrderDtl _selectedDtlItem;
        public OrderDtl SelectedDtlItem
        {
            get => _selectedDtlItem;
            set
            {
                if (SetProperty(ref _selectedDtlItem, value))
                {
                    Description = _selectedDtlItem.OrderDtlDscr;
                }
            }
        }

        private string _selectedGroup;
        public string SelectedGroup
        {
            get => _selectedGroup;
            set => SetProperty(ref _selectedGroup, value);

        }
        public string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string OriginalImageName { get; set; }

        private string _imageName;
        public string ImageName
        {
            get => _imageName;
            set => SetProperty(ref _imageName, value);
        }

        private ICommand _captureImageCommand = null;

        public ICommand CaptureImageCommand => _captureImageCommand = _captureImageCommand ?? new Command(async () => await SaveCaptureImage());

        public async Task<bool> SaveCaptureImage()
        {
            string base64Image = Utils.Utils.ConvertToBase64(imageStream);
            string compressImage = Utils.Utils.CompressImage(base64Image);
            OrderImageInputDTO orderImageInputDTO = new OrderImageInputDTO();
            orderImageInputDTO.FPImage = new OrderImageDetail();
            orderImageInputDTO.FPImage.OrderNo = SelectedOrder.OrderNo;
            orderImageInputDTO.FPImage.OrderDtlNo = SelectedDtlItem?.OrderDtlNo ?? null;
            orderImageInputDTO.FPImage.OrderImageName = Path.GetFileNameWithoutExtension(ImageName);
            orderImageInputDTO.FPImage.OrderImageExt = Path.GetExtension(OriginalImageName);
            orderImageInputDTO.FPImage.OrderImageGroup = ImageGroupNames.GetGroupNumber(SelectedGroup);
            orderImageInputDTO.FPImage.OrderImageDscr = Description;
            orderImageInputDTO.FPImage.Image = compressImage;
            orderImageInputDTO.FPImage.OrderImageEmpID = DataManager.Settings.UserName;
            bool result = await ViewOrderEntityComponent.SaveOrderImage(orderImageInputDTO);
            MessagingCenter.Send(this, "LoadImages");
            return result;
        }
    }
}
