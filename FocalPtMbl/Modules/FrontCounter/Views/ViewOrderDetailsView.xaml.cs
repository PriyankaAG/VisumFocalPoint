using FocalPoint.Data.API;
using FocalPoint.Models;
using FocalPoint.Modules.Common.View;
using FocalPoint.Modules.FrontCounter.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Visum.Services.Mobile.Entities;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewOrderDetailsView
    {
        public ViewOrderDetailsView(Order selectedOrder)
        {
            this.Title = "View Orders";
            InitializeComponent();
            ((ViewOrderDetailsViewModel)this.BindingContext).SelectedOrder = selectedOrder;

            this.ImageListView.ItemSelected += ImageListView_ItemSelected;

            this.ToolbarItems.Add(new ToolbarItem()
            {
                IconImageSource = "more.png",
                Command = this.CogCommand,
            });
            Task.Run(async () =>
            {
                await LoadImages();
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Unsubscribe<OrderSignatureTermsViewModel, bool>(this, "TermsDeclined");
            MessagingCenter.Unsubscribe<OrderSignatureTermsViewModel, bool>(this, "TermsAccepted");
            MessagingCenter.Unsubscribe<OrderSignatureViewModel, string>(this, "WaiverSignature");
            MessagingCenter.Unsubscribe<OrderSignatureViewModel, string>(this, "Signature");
            MessagingCenter.Unsubscribe<SelectedImageGetDetailsViewModel>(this, "LoadImages");

            MessagingCenter.Subscribe<OrderSignatureTermsViewModel, bool>(this, "TermsDeclined", async(sender, args) =>
            {
                SignatureTermsView signatureTermsView = (SignatureTermsView)Navigation.NavigationStack.FirstOrDefault(p => p is SignatureTermsView);
                OrderSignatureView orderSignatureView = (OrderSignatureView)Navigation.NavigationStack.FirstOrDefault(p => p is OrderSignatureView);
                if (signatureTermsView != null || orderSignatureView != null)
                    await Navigation.PopAsync();
                ViewOrderDetailsViewModel viewOrderDetailsViewModel = ((ViewOrderDetailsViewModel)this.BindingContext);
                if (args)
                {
                    await DisplayAlert("FocalPoint Mobile", "Damage Waiver Rejected", "OK");
                }
                else
                {
                   await DisplayAlert("FocalPoint Mobile", "Terms Rejected", "OK");
                }

            });

            MessagingCenter.Subscribe<OrderSignatureTermsViewModel, bool>(this, "TermsAccepted", async(sender, args) =>
            {
                OrderSignatureView orderSignatureView = (OrderSignatureView)Navigation.NavigationStack.FirstOrDefault(p => p is OrderSignatureView);
                SignatureTermsView signatureTermsView = (SignatureTermsView)Navigation.NavigationStack.FirstOrDefault(p => p is SignatureTermsView);
                if (orderSignatureView != null || signatureTermsView != null)
                    await Navigation.PopAsync();
                ViewOrderDetailsViewModel viewOrderDetailsViewModel = (ViewOrderDetailsViewModel)BindingContext;
                viewOrderDetailsViewModel.OpenSignaturePage(Navigation, args);
            });

            MessagingCenter.Subscribe<OrderSignatureViewModel, string>(this, "WaiverSignature", async (sender, capturedImage) =>
            {
                OrderSignatureView orderSignatureView = (OrderSignatureView)Navigation.NavigationStack.FirstOrDefault(p => p is OrderSignatureView);
                SignatureTermsView signatureTermsView = (SignatureTermsView)Navigation.NavigationStack.FirstOrDefault(p => p is SignatureTermsView);
                if (orderSignatureView != null || signatureTermsView != null)
                    await Navigation.PopAsync();
                ViewOrderDetailsViewModel viewOrderDetailsViewModel = (ViewOrderDetailsViewModel)BindingContext;
                viewOrderDetailsViewModel.WaiverCapturedImage = capturedImage;
                viewOrderDetailsViewModel.IsNeedToRedirectTermsOrSignature(Navigation);
            });

            MessagingCenter.Subscribe<OrderSignatureViewModel, string>(this, "Signature", async(sender, capturedImage) =>
            {
                SignatureTermsView signatureTermsView = (SignatureTermsView)Navigation.NavigationStack.FirstOrDefault(p => p is SignatureTermsView);
                OrderSignatureView orderSignatureView = (OrderSignatureView)Navigation.NavigationStack.FirstOrDefault(p => p is OrderSignatureView);
                if (signatureTermsView != null || orderSignatureView != null)
                    await Navigation.PopAsync();
                ViewOrderDetailsViewModel viewOrderDetailsViewModel = (ViewOrderDetailsViewModel)BindingContext;
                viewOrderDetailsViewModel.SignatureImage = capturedImage;
                bool success = await viewOrderDetailsViewModel.SaveSignature();
                if (success)
                {
                   await DisplayAlert("FocalPoint Mobile", "Signature added successfully", "OK");
                }
                else
                {
                    await DisplayAlert("FocalPoint Mobile", "Failed to add Signature", "OK");
                }
            });

            MessagingCenter.Subscribe<SelectedImageGetDetailsViewModel>(this, "LoadImages", async(sender) =>
            {
                await LoadImages();
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped_4(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped_5(object sender, EventArgs e)
        {

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
                            OrderCogOptions.Email,
                            OrderCogOptions.CaptureImage,
                            OrderCogOptions.AttachImage);

                    if (option == OrderCogOptions.Cancel)
                        return;

                    if (option == OrderCogOptions.CaptureSignature)
                        await Signature();
                    else if (option == OrderCogOptions.CaptureImage)
                        await CaptureImage();
                    else if (option == OrderCogOptions.AttachImage)
                        await AttachImage();
                    else if (option == OrderCogOptions.Email)
                        await SendEmail();
                });
            }
        }

        private async Task Signature()
        {
            ViewOrderDetailsViewModel viewOrderDetailsViewModel = ((ViewOrderDetailsViewModel)this.BindingContext);
            await viewOrderDetailsViewModel.SignatureCommand(this.Navigation);
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
                        //string emailContent = await DisplayPromptAsync("Email Details", "What's the content of the message");
                        //if (emailContent != null)
                        //{
                        IGeneralComponent generalComponent = new GeneralComponent();
                        EmailDocumentInputDTO emailDocumentInputDTO = new EmailDocumentInputDTO();
                        emailDocumentInputDTO.DocKind = (int)DocKinds.Order;
                        emailDocumentInputDTO.RecordID = ((ViewOrderDetailsViewModel)this.BindingContext).SelectedOrder.OrderNo;
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
                        //}
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

        private async Task CaptureImage()
        {
            try
            {
                if (MediaPicker.IsCaptureSupported)
                {
                    var photoResult = await MediaPicker.CapturePhotoAsync();

                    var stream = await photoResult.OpenReadAsync();
                    if (stream != null)
                    {
                        await Navigation.PushAsync(new SelectedImageGetDetailsView(((ViewOrderDetailsViewModel)this.BindingContext).SelectedOrder, photoResult.FileName, stream));
                    }
                    //Check old app to add details before you save or cancel

                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task AttachImage()
        {
            try
            {
                if (MediaPicker.IsCaptureSupported)
                {
                    var photoResult = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                    {
                        Title = "Pick A photo"
                    });

                    var stream = await photoResult.OpenReadAsync();
                    if (stream != null)
                    {
                        await Navigation.PushAsync(new SelectedImageGetDetailsView(((ViewOrderDetailsViewModel)this.BindingContext).SelectedOrder, photoResult.FileName, stream));
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        async private void ImageListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (this.ImageListView.SelectedItem == null)
                return;

            await Navigation.PushAsync(new OrderImagePage(this.ImageListView.SelectedItem as OrderImageViewModel));

            ImageListView.SelectedItem = null;
        }

        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        async Task LoadImages()
        {
            ViewOrderDetailsViewModel viewOrderDetailsViewModel = ((ViewOrderDetailsViewModel)this.BindingContext);
            var images= await viewOrderDetailsViewModel.GetOrderImagesAsync();

            viewOrderDetailsViewModel.Images.Clear();
            viewOrderDetailsViewModel.ImageList.Clear();
            var displayOrder = ImageGroupNames.GetDisplayOrder();

            foreach (var heading in displayOrder)
            {
                try
                {
                    ImageList curGroup = new ImageList();
                    curGroup.Heading = heading;
                    foreach (var img in images.Where(x => x.OrderImageGroup == ImageGroupNames.GetGroupNumber(heading)))
                    {
                        var imgVM = new OrderImageViewModel(img, true);
                        viewOrderDetailsViewModel.Images.Add(imgVM);
                        curGroup.Images.Add(imgVM);
                    }

                    if (curGroup.Images.Any())
                    {
                        viewOrderDetailsViewModel.ImageList.Add(curGroup);
                    }
                }
                catch (Exception e)
                {
                }
            }
        }

        private void OnRefresh(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var list = (ListView)sender;
                ViewOrderDetailsViewModel viewOrderDetailsViewModel = ((ViewOrderDetailsViewModel)this.BindingContext);

                if (viewOrderDetailsViewModel.Images == null)
                    viewOrderDetailsViewModel.Images = new List<OrderImageViewModel>();

                await LoadImages();

                list.IsRefreshing = false;
            });
        }
    }

    public class OrderCogOptions
    {
        public static string Cancel = "Cancel";
        public static string Email = "Email Copy to Customer";
        public static string CaptureSignature = "Capture Signature";
        public static string CaptureImage = "Capture Image";
        public static string AttachImage = "Attach Image";
    }

    public class ImageList : ObservableCollection<OrderImageViewModel>
    {
        public string Heading { get; set; }
        public ObservableCollection<OrderImageViewModel> Images => this;
    }
}