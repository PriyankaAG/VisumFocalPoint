using FocalPtMbl.MainMenu.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.FrontCounter.ViewModels
{
    public class RentalResultsViewModel : ThemeBaseViewModel
    {
        ObservableCollection<RentalAvailability> recent;
        public ObservableCollection<RentalAvailability> Recent
        {
            get => this.recent;
            private set
            {
                this.recent = value;
                OnPropertyChanged(nameof(Recent));
            }
        }
        public RentalResultsViewModel(ObservableCollection<RentalAvailability> searchedAvailabilities)
        {
            Recent = searchedAvailabilities;
        }
    }
}
