using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FocalPoint.Modules.Payments.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Payments.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentProcessView : ContentView
    {
        public PaymentProcessView()
        {
            InitializeComponent();
        }

        private void Payment_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((PaymentPageViewModel)BindingContext != null 
                && ((PaymentPageViewModel)BindingContext).SelectedPaymentType.PaymentKind != "CA" 
                && ((PaymentPageViewModel)BindingContext).SelectedPaymentType.PaymentKind != "CK")
            {
                return;
            }
            TotalReceived.TextChanged -= TotalReceived_TextChanged;
            ChangeDue.TextChanged -= ChangeDue_TextChanged;
            TotalReceived.EditorText = decimal.TryParse(e.NewTextValue?.Trim('$'), out decimal value) ? value.ToString("c") : "";
            ChangeDue.EditorText = 0.0.ToString("c");
            TotalReceived.TextChanged += TotalReceived_TextChanged;
            ChangeDue.TextChanged += ChangeDue_TextChanged;
        }

        private void TotalReceived_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == null) return;
            Payment.TextChanged -= Payment_TextChanged;
            ChangeDue.TextChanged -= ChangeDue_TextChanged;
            Payment.EditorText = 0.0.ToString("c");
            ChangeDue.EditorText = decimal.TryParse(e.NewTextValue?.Trim('$'), out decimal value) ? value.ToString("c") : "";
            Payment.TextChanged += Payment_TextChanged;
            ChangeDue.TextChanged += ChangeDue_TextChanged;
        }

        private void ChangeDue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == null) return;
            Payment.TextChanged -= Payment_TextChanged;
            var newValue = string.IsNullOrEmpty(e.NewTextValue) ? 0.ToString() : e.NewTextValue.Trim('$');
            Payment.EditorText = decimal.TryParse(TotalReceived.EditorText?.Trim('$'), out decimal total) && decimal.TryParse(newValue, out decimal due)
                ? (total - due).ToString("c")
                : "";
            Payment.TextChanged += Payment_TextChanged;
        }

        private void Payment_Unfocused(object sender, FocusEventArgs e)
        {
            Payment.EditorText = decimal.TryParse(Payment.EditorText.Trim('$'), out decimal value) ? value.ToString("c") : "";
        }

        private void TotalReceived_Unfocused(object sender, FocusEventArgs e)
        {
            TotalReceived.EditorText = decimal.TryParse(TotalReceived.EditorText.Trim('$'), out decimal value) ? value.ToString("c") : "";
        }

        private void ChangeDue_Unfocused(object sender, FocusEventArgs e)
        {
            ChangeDue.EditorText = decimal.TryParse(ChangeDue.EditorText.Trim('$'), out decimal value) ? value.ToString("c") : "";
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            var imageSource = ((ImageButton)sender).Source.ToString();
            if (imageSource.Contains("arrow_down.png"))
            {
                PaymentGrid.IsVisible = false;
                ((ImageButton)sender).Source = "arrow_up.png";
            }
            else
            {
                PaymentGrid.IsVisible = true;
                ((ImageButton)sender).Source = "arrow_down.png";
            }
        }
    }
}