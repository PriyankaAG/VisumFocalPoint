using FocalPoint.Utils;
using FocalPtMbl.MainMenu.ViewModels;
using System.IO;
using Xamarin.Forms;

namespace FocalPoint.Modules.ViewModels
{
    public class SignatureViewModel: ThemeBaseViewModel
    {

        #region Construction & Initialisation

        public SignatureViewModel(bool isWaiver, string title) : base("Signature Capture")
        {
            IsWaiver = isWaiver;
            SignatureFor = title;
        }


        #endregion Construction & Initialisation

        #region Properties

        public bool IsWaiver { get; set; }
        public string SignatureFor { get; set; }
        public string Message { get; set; }

        #endregion Properties

        #region Commands

        #endregion Commands

        #region Methods

        public void SaveSignature(Stream bitmap)
        {
            string base64Image = Utils.Utils.ConvertToBase64(bitmap);
            string compressImage = Utils.Utils.CompressImage(base64Image);
            MessagingCenter.Send(this, (IsWaiver ? "WaiverSignature" : "Signature"), compressImage);            
        }

        #endregion Methods
    }
}
