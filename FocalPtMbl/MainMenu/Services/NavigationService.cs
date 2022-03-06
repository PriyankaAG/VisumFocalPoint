using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Linq;
using FocalPtMbl.MainMenu.Models;
using FocalPtMbl.MainMenu.ViewModels;
using FocalPtMbl.MainMenu.Views;
using FocalPtMbl.MainMenu.ViewModels.Services;
using System.Threading.Tasks;

namespace FocalPtMbl.MainMenu.Services
{
    public class NavigationService : INavigationService
    {
        NavigationPage navigator;
        bool ctrlPagePushed;
        bool isPagePushed;

        public Dictionary<Type, Func<Page>> PageBinders { get; }

        public NavigationService()
        {
            PageBinders = new Dictionary<Type, Func<Page>>();
            this.ctrlPagePushed = false;
            this.isPagePushed = false;
        }

        public void SetNavigator(NavigationPage navPage)
        {
            this.navigator = navPage;
        }

        public async Task Push(object viewModel)
        {
            Type vmType = viewModel.GetType();
            if (PageBinders.TryGetValue(vmType, out Func<Page> pageBuilder))
            {
                Page page = pageBuilder();
                if (!this.ctrlPagePushed)
                {
                    page.BindingContext = viewModel;
                    this.ctrlPagePushed = true;
                    await PushAsync(page);
                }
            }
        }

        void Page_Disappearing(object sender, EventArgs e)
        {
            Page page = sender as Page;
            if (page != null)
            {
                page.Disappearing -= Page_Disappearing;
                if (page is ControlPage)
                {
                    this.ctrlPagePushed = false;
                }
                else
                {
                    this.isPagePushed = false;
                    GC.Collect();
                }
            }
        }

        public async Task<Page> PushPage(object viewModel)
        {
            Page page = null;
            PageItem item = viewModel as PageItem;

            if (!this.isPagePushed &&
                item != null &&
                item.Module != null &&
                item.Module.IsSubclassOf(typeof(Page))
            )
            {
               // ErrorDialogPage errorDialogPage = this.navigator.CurrentPage as ErrorDialogPage;
                try
                {
                    var module = item.Module;
                    page = (Page)Activator.CreateInstance(module);
                    page.Title = item.PageTitle;

                    if (page != null)
                    {
                        this.isPagePushed = true;
                        await PushAsync(page);
                    }

                }
                catch (Exception e)
                {
#if DEBUG
                   throw e;
#else
                        throw e;
#endif
                    //                    if (errorDialogPage != null)
                    //                       errorDialogPage.ShowError(e);
                }
            }
            return await Task.FromResult(page);
        }

        public IEnumerable<Page> GetOpenedPages<T>() where T : Page
        {
            return this.navigator.Navigation.NavigationStack.Where((p) => p.GetType() == typeof(T));
        }



        async Task PushAsync(Page page)
        {
            page.Disappearing += Page_Disappearing;
            NavigationPage.SetBackButtonTitle(page, "Back");
            this.navigator.BackgroundColor = (Color)Application.Current.Resources["BackgroundThemeColor"];
            await this.navigator.PushAsync(page);
        }


    }
}
