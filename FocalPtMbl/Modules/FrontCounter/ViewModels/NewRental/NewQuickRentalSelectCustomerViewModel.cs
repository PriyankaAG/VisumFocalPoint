using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FocalPoint.Components.EntityComponents;
using FocalPoint.Components.Interface;
using FocalPtMbl.MainMenu.ViewModels;
using MvvmHelpers.Commands;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Modules.FrontCounter.ViewModels.NewRental
{
    public class NewQuickRentalSelectCustomerViewModel : ThemeBaseViewModel
    {
        public INewQuickRentalEntityComponent NewQuickRentalEntityComponent { get; set; }
        public ICommand CompletedCommand { get; }
        public ICommand TextChangedCommand { get; }
        public ICommand UnfocusedCommand { get; }
        public string TextToSearch { get; set; }

        public List<Customer> ListOfCustomers { get; set; }
        public NewQuickRentalSelectCustomerViewModel() : base("Select Customer")
        {
            NewQuickRentalEntityComponent = new NewQuickRentalEntityComponent();
            CompletedCommand = new Command<string>((a) => OnCompleted(a));
            TextChangedCommand = new Command<string>((a) => OnTextChanged(a));
            UnfocusedCommand = new Command<string>((a) => Unfocused(a));


        }
        private void Unfocused(string ordType)
        {
            if (Indicator)
                return;

            if (TextToSearch != string.Empty)
            {
                SearchRecords(TextToSearch);
            }
        }
        private void OnTextChanged(string ordType)
        {
            if (Indicator)
                return;

            if (TextToSearch == string.Empty)
            {
                FetchAllRecord();
            }
        }
        private void OnCompleted(string ordType)
        {
            if (Indicator)
                return;

            if (TextToSearch != string.Empty)
            {
                SearchRecords(TextToSearch);
            }
        }
        public async Task GetCustomers()
        {
            FetchAllRecord();
        }

        private async void FetchAllRecord()
        {
            try
            {

                Indicator = true;
                if (ListOfCustomers != null && ListOfCustomers.Count > 0) ListOfCustomers.Clear();
                ListOfCustomers = await NewQuickRentalEntityComponent.GetCustomers("");
                OnPropertyChanged(nameof(ListOfCustomers));

            }
            catch (Exception e)
            {
                //TODO: Log Error
            }
            finally
            {
                Indicator = false;
            }
        }

        private async void SearchRecords(string textToSearch)
        {
            try
            {

                Indicator = true;
                if (ListOfCustomers != null && ListOfCustomers.Count > 0) ListOfCustomers.Clear();
                ListOfCustomers = await NewQuickRentalEntityComponent.GetCustomers(textToSearch);
                OnPropertyChanged(nameof(ListOfCustomers));

            }
            catch (Exception e)
            {
                //TODO: Log Error
            }
            finally
            {
                Indicator = false;
            }
        }
    }
}
