using FocalPtMbl.MainMenu.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FocalPoint.Modules.ViewModels
{
    public class SignatureTermsViewModel : ThemeBaseViewModel
    {
        #region Constructor & Intialization

        public SignatureTermsViewModel(bool isWaiver, string title, string message): base("Signature")
        {
            this.TermsTitle = title;
            IsWaiver = isWaiver;
            Message = message;
        }

        #endregion Constructor & Intialization

        #region Properties

        public bool IsWaiver { get; set; }
        public string TermsTitle { get; set; }
        public string Message { get; set; }

        #endregion Properties

        #region Commands

        private ICommand _declineCommand = null;

        public ICommand DeclineCommand => _declineCommand = _declineCommand ?? new Command(() => OnDeclineCommand());

        private ICommand _acceptCommand = null;

        public ICommand AcceptCommand => _acceptCommand = _acceptCommand ?? new Command(() => OnAcceptCommand());

        #endregion Commands

        #region Methods

        public void OnDeclineCommand()
        {
            MessagingCenter.Send(this, "TermsDeclined", IsWaiver);
        }

        public void OnAcceptCommand()
        {
            MessagingCenter.Send(this, "TermsAccepted", IsWaiver);
        }

        #endregion Methods


    }
}
