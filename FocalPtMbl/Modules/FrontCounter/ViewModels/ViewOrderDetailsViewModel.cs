using FocalPoint.Components.EntityComponents;
using FocalPoint.Data;
using FocalPoint.Data.API;
using FocalPoint.Modules.FrontCounter.Views;
using FocalPoint.Utils;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.FrontCounter.ViewModels
{
    public class ViewOrderDetailsViewModel : ThemeBaseViewModel
    {
        HttpClient clientHttp;
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        public ViewOrderDetailsViewModel()
        {
            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            clientHttp = httpClientCache.GetHttpClientAsync();
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

        private SignatureMessageOutputDTO _signatureMessageOutputDTO;
        public SignatureMessageOutputDTO SignatureMessageOutputDTO
        {
            get => _signatureMessageOutputDTO;
            set => SetProperty(ref _signatureMessageOutputDTO, value);
        }

        private string _waiverCapturedImage;
        public string WaiverCapturedImage
        {
            get => _waiverCapturedImage;
            set => SetProperty(ref _waiverCapturedImage, value);
        }

        public string _signatureImage;
        public string SignatureImage
        {
            get => _signatureImage;
            set => SetProperty(ref _signatureImage, value);
        }

        public IViewOrderEntityComponent ViewOrderEntityComponent { get; set; }

        internal bool EmailExists()
        {
            //throw new NotImplementedException();
            return false;
        }

        public void OpenSignatureTermsView(INavigation navigation)
        {
            var orderSignatureTermsViewModel = new OrderSignatureTermsViewModel(false, SelectedOrder.OrderNo, "Terms & Conditions", SignatureMessageOutputDTO.Terms);
            var orderSignatureTermsView = new SignatureTermsView();
            orderSignatureTermsView.BindingContext = orderSignatureTermsViewModel;
            navigation.PushAsync(orderSignatureTermsView);
        }

        public void OpenSignatureWaiverPage(INavigation navigation)
        {
            var orderSignatureTermsViewModel = new OrderSignatureTermsViewModel(true, SelectedOrder.OrderNo, SignatureMessageOutputDTO.WaiverDscr, SignatureMessageOutputDTO.Waiver);
            var orderSignatureTermsView = new SignatureTermsView();
            orderSignatureTermsView.BindingContext = orderSignatureTermsViewModel;
            navigation.PushAsync(orderSignatureTermsView);
        }

        public void OpenSignaturePage(INavigation navigation, bool isWaiver)
        {
            OrderSignatureViewModel orderSignatureViewModel = new OrderSignatureViewModel(SelectedOrder, isWaiver, "Sign above for Terms & Conditions");
            var orderSignatureView = new OrderSignatureView();
            orderSignatureView.BindingContext = orderSignatureViewModel;
            navigation.PushAsync(orderSignatureView);
        }

        public void IsNeedToRedirectTermsOrSignature(INavigation navigation)
        {
            if (!string.IsNullOrWhiteSpace(SignatureMessageOutputDTO?.Terms))
            {
                OpenSignatureTermsView(navigation);
            }
            else
            {
                OpenSignaturePage(navigation, false);
            }
        }

        public async Task SignatureCommand(INavigation navigation)
        {
            SignatureMessageInputDTO singnatureMessageInputDTO = new SignatureMessageInputDTO();
            singnatureMessageInputDTO.DocKind = (int)DocKinds.Order;
            singnatureMessageInputDTO.RecordID = SelectedOrder.OrderNo;
            singnatureMessageInputDTO.Stat = "E";//OrderEdit, used when editing an existing Order
            SignatureMessageOutputDTO = await ViewOrderEntityComponent.GetSignatureMessageDTO(singnatureMessageInputDTO);
            if (SignatureMessageOutputDTO != null)
            {
                if (!string.IsNullOrWhiteSpace(SignatureMessageOutputDTO.Waiver))
                {
                    OpenSignatureWaiverPage(navigation);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(SignatureMessageOutputDTO.Terms))
                    {
                        OpenSignatureTermsView(navigation);
                    }
                    else
                    {
                        OpenSignaturePage(navigation, false);
                    }
                }
            }
            else
            { 
                OpenSignaturePage(navigation, false); 
            }
        }

        public async Task<bool> SaveSignature()
        {
            SignatureInputDTO signatureInputDTO = new SignatureInputDTO();
            signatureInputDTO.DocKind = (int)DocKinds.Order;
            signatureInputDTO.RecordID = SelectedOrder.OrderNo;
            signatureInputDTO.Stat = "E";//OrderEdit, used when editing an existing Order
            signatureInputDTO.Format = 4;//Base64 String of Image
            signatureInputDTO.Signature = SignatureImage;
            signatureInputDTO.Waiver = WaiverCapturedImage;
            return await ViewOrderEntityComponent.SaveSignature(signatureInputDTO);
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
