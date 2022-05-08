using FocalPoint.Data.API;
using FocalPoint.Modules.FrontCounter.ViewModels;
using FocalPoint.Modules.FrontCounter.Views;
using FocalPoint.Modules.ServiceDepartment.ViewModels;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.ServiceDepartment.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkOrderFormTabDetailsView 
    {
        public WorkOrderFormTabDetailsView(WorkOrder workOrder)
        {
            DevExpress.XamarinForms.Navigation.Initializer.Init();
            this.viewModel = new WorkOrderFormTabDetailsViewModel(workOrder);
            BindingContext = this.viewModel;
            this.Title = "Work Order: " + workOrder.WONo.ToString();
            InitializeComponent();

            this.ToolbarItems.Add(new ToolbarItem()
            {
                IconImageSource = "more.png",
                Command = this.CogCommand,
            });

        }
        readonly WorkOrderFormTabDetailsViewModel viewModel;

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            //if web request is pending try to reload token and wait if()
            ActivityIndicator activityIndicator = new ActivityIndicator { IsRunning = true };
            await Task.Delay(10000);
            //when task is complete turn indicator off
            activityIndicator.IsRunning = false;
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public Command CogCommand
        {
            get
            {
                return new Command(async () =>
                {
                    string option = await this.DisplayActionSheet("Options", OrderCogOptions.Cancel, null,
                            OrderCogOptions.CaptureSignature,
                            OrderCogOptions.Email);

                    if (option == OrderCogOptions.Cancel)
                        return;

                    if (option == OrderCogOptions.CaptureSignature)
                        await Signature();
                    else if (option == OrderCogOptions.Email)
                        await SendEmail();
                });
            }
        }

        private async Task Signature()
        {
            WorkOrderFormTabDetailsViewModel viewOrderDetailsViewModel = ((WorkOrderFormTabDetailsViewModel)this.BindingContext);
           // await viewOrderDetailsViewModel.SignatureCommand(this.Navigation);
        }

        private async Task SendEmail()
        {

            if (((ViewOrderDetailsViewModel)this.BindingContext).EmailExists())
            {
                string emailContent = await DisplayPromptAsync("Email Details", "What's the content of the message");
                //Send the email to the server
            }
            //Popup would you like to send message to email on file
            else
            {
                bool confirmation = await DisplayAlert("No Email Found", "No Email on file for customer. Would you like to send to a new address? ", "OK", "Cancel");
                if (confirmation)
                {
                    string customersEmail = await DisplayPromptAsync("Customer's Email", "What's the customers email address", keyboard: Keyboard.Email);
                    if (IsValidEmail(customersEmail))
                    {
                        string emailContent = await DisplayPromptAsync("Email Details", "What's the content of the message");
                        if (emailContent != null)
                        {
                            IGeneralComponent generalComponent = new GeneralComponent();
                            EmailDocumentInputDTO emailDocumentInputDTO = new EmailDocumentInputDTO();
                            emailDocumentInputDTO.DocKind = (int)DocKinds.Order;
                           // emailDocumentInputDTO.RecordID = ((WorkOrderFormTabDetailsViewModel)this.BindingContext).WOJobNo;
                            emailDocumentInputDTO.ToAddr = customersEmail;
                            bool response = await generalComponent.SendEmailDocument(emailDocumentInputDTO);
                            if (response)
                            {
                                await DisplayAlert("Success", "Document sent successfully", "OK");
                            }
                            else
                            {
                                await DisplayAlert("FocalPoint Mobile", "Failed to send an email", "OK");
                            }
                        }
                    }
                    else
                    {
                        await DisplayAlert("Email not in the right format", "The email entered was not the correct format. Please include the full domain ", "OK");
                    }
                }
                //Else no email found/new address Get new email address

                //GetBody
            }
        }
    }
}