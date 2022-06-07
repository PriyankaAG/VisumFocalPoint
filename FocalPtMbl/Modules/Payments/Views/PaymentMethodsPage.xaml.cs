using System.Threading.Tasks;
using FocalPoint.Modules.Payments.ViewModels;
using Visum.Services.Mobile.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Payments.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentMethodsPage : ContentPage
    {
        PaymentPageViewModel viewModel;
        public PaymentMethodsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel = (PaymentPageViewModel)BindingContext;
            SetPayments();
        }
        
        private void SetPayments()
        {
            if (viewModel != null && viewModel.PaymentTypes != null && viewModel.PaymentTypes.Count > 1)
                AddPaymentKinds(viewModel.PaymentTypes);
        }

        private void AddPaymentKinds(System.Collections.Generic.List<PaymentType> paymentTypes)
        {
            int rowCount = paymentTypes.Count <= 3 ? 1 : 1 + paymentTypes.Count / 3;
            paymentKinds.Children.Clear();
            paymentKinds.RowDefinitions.Clear();
            paymentKinds.ColumnDefinitions.Clear();
            for (int i = 0; i < rowCount; i++)
            {
                paymentKinds.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            for (int j = 0; j < 3; j++)
            {
                paymentKinds.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            }

            int RN = 0;
            for (int num = 0; num < paymentTypes.Count; num++)
            {
                var paymentIcon = paymentTypes[num].PaymentTIcon;
                var imgBtn = new ImageButton
                {
                    Source = viewModel.GetPaymentImage(paymentIcon),
                    BackgroundColor = Color.FromHex("#f5f6fa"),
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    CommandParameter = paymentTypes[num],
                    HeightRequest = 70
                };
                imgBtn.Clicked += ImgBtn_Clicked;

                var lblText = new Label
                {
                    Text = paymentTypes[num].PaymentDscr,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center
                };
                var stackLayout = new StackLayout { Orientation = StackOrientation.Vertical };
                stackLayout.Children.Add(imgBtn);
                stackLayout.Children.Add(lblText);
                if (num % 3 == 0)
                {
                    paymentKinds.Children.Add(stackLayout, 0, RN);
                }
                else if (num % 3 == 1)
                {
                    paymentKinds.Children.Add(stackLayout, 1, RN);
                }
                else
                {
                    paymentKinds.Children.Add(stackLayout, 2, RN);
                    RN++;
                }
            }
        }

        private async void ImgBtn_Clicked(object sender, System.EventArgs e)
        {
            PaymentType paymentType = ((ImageButton)sender).CommandParameter as PaymentType;
            ((ImageButton)sender).IsEnabled = false;
            viewModel.SelectedPaymentType = paymentType;
            PaymentKindPage paymentKindPage = new PaymentKindPage
            {
                BindingContext = viewModel
            };
            await Navigation.PushAsync(paymentKindPage);
            ((ImageButton)sender).IsEnabled = true;
        }

        private void CancelButton_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}