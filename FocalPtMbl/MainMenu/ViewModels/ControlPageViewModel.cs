using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using FocalPtMbl.MainMenu.ViewModels.Services;
using FocalPtMbl.MainMenu.Models;
using FocalPtMbl.MainMenu.Data; 


namespace FocalPtMbl.MainMenu.ViewModels
{
    public class ControlPageViewModel : ThemeBaseViewModel 
    {
        IPageData data;
        PageItem selectedItem;
        public List<PageItem> PageItems
        {
            get => data.PageItems;
        }
        public PageItem SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                if (selectedItem == null)
                    return;
                NavigationPageCommand.Execute(selectedItem);
            }
        }
        public ICommand NavigationPageCommand { get; }

        public ControlPageViewModel(INavigationService navigationService, IPageData data)
        {
            this.data = data;
            this.Title = data.Title;
            NavigationPageCommand = new DelegateCommand<PageItem>((p) => navigationService.PushPage(p));
        }

    }
}
