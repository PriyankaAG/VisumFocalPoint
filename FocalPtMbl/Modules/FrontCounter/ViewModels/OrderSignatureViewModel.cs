using FocalPoint.Utils;
using FocalPtMbl.MainMenu.ViewModels;
using System.IO;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;

namespace FocalPoint.Modules.FrontCounter.ViewModels
{
    public class OrderSignatureViewModel: ThemeBaseViewModel
    {

        #region Construction & Initialisation

        public OrderSignatureViewModel(Order orderDetails, bool isWaiver, string title) : base("Signature Capture")
        {
            ViewOrderEntityComponent = new ViewOrderEntityComponent();
            SelectedOrder = orderDetails;
            IsWaiver = isWaiver;
            SignatureFor = title;
        }

        #endregion Construction & Initialisation

        #region Properties

        public IViewOrderEntityComponent ViewOrderEntityComponent { get; set; }

        public Order SelectedOrder { get; set; }
        public bool IsWaiver { get; set; }
        public string SignatureFor { get; set; }
        public string Message { get; set; }

        #endregion Properties

        #region Commands

        #endregion Commands

        #region Methods

        public void SaveSignature(Stream bitmap)
        {
            string base64Image = Ultils.ConvertToBase64(bitmap);
            string compressImage = Ultils.CompressImage(base64Image);
            MessagingCenter.Send(this, (IsWaiver ? "WaiverSignature" : "Signature"), compressImage);            
        }

        #endregion Methods
    }
}
