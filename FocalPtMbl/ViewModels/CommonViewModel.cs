using FocalPoint.Data.API;
using FocalPoint.Modules.FrontCounter.Views;
using FocalPoint.Modules.ViewModels;
using FocalPtMbl.MainMenu.ViewModels;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FocalPoint
{
    public class CommonViewModel : ThemeBaseViewModel
    {
        public CommonViewModel(string title = "") : base(title)
        {
            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            clientHttp = httpClientCache.GetHttpClientAsync();
            GeneralComponent = new GeneralComponent();
        }

        HttpClient clientHttp;
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }

        public DocKinds DocKind { get; set; }
        public int RecordID { get; set; }
        public string Stat { get; set; }
        public IGeneralComponent GeneralComponent { get; set; }

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

        internal bool EmailExists()
        {
            //throw new NotImplementedException();
            return false;
        }

        public void SetEntityDetails(DocKinds docKind, int recordID, string stat)
        {
            DocKind = docKind;
            RecordID = recordID;
            Stat = stat;
        }

        public void OpenSignatureTermsView(INavigation navigation)
        {
            var orderSignatureTermsViewModel = new SignatureTermsViewModel(false, "Terms & Conditions", SignatureMessageOutputDTO.Terms);
            var orderSignatureTermsView = new SignatureTermsView();
            orderSignatureTermsView.BindingContext = orderSignatureTermsViewModel;
            navigation.PushAsync(orderSignatureTermsView);
        }

        public void OpenSignatureWaiverPage(INavigation navigation)
        {
            var orderSignatureTermsViewModel = new SignatureTermsViewModel(true, SignatureMessageOutputDTO.WaiverDscr, SignatureMessageOutputDTO.Waiver);
            var orderSignatureTermsView = new SignatureTermsView();
            orderSignatureTermsView.BindingContext = orderSignatureTermsViewModel;
            navigation.PushAsync(orderSignatureTermsView);
        }

        public void OpenSignaturePage(INavigation navigation, bool isWaiver)
        {
            SignatureViewModel orderSignatureViewModel = new SignatureViewModel(isWaiver, "Sign above for Terms & Conditions");
            var orderSignatureView = new SignatureView();
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
            singnatureMessageInputDTO.DocKind = (int)DocKind;
            singnatureMessageInputDTO.RecordID = RecordID;
            singnatureMessageInputDTO.Stat = Stat;
            SignatureMessageOutputDTO = await GeneralComponent.GetSignatureMessageDTO(singnatureMessageInputDTO);
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

        public virtual async Task<bool> SaveSignature()
        {
            SignatureInputDTO signatureInputDTO = new SignatureInputDTO();
            signatureInputDTO.DocKind = (int)DocKind;
            signatureInputDTO.RecordID = RecordID;
            signatureInputDTO.Stat = Stat;
            signatureInputDTO.Format = 4;//Base64 String of Image
            signatureInputDTO.Signature = SignatureImage;
            signatureInputDTO.Waiver = WaiverCapturedImage;
            return await GeneralComponent.SaveSignature(signatureInputDTO);
        }
    }
}
