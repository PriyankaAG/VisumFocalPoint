using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FocalPtMbl.MainMenu.ViewModels.Services
{
    public interface INavigationService
    {
        Task<Page> PushPage(object viewModel);
        Task<Page> PushPageFromMenu(Type pageToPush, string pageTitle);
        void PushChildPage(Page page);

        Task Push(Object viewModel);
    }
}
