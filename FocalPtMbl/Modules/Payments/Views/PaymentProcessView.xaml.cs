using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FocalPoint.Modules.Payments.ViewModels;
using FocalPoint.Utils;
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
            var selectyedKind = ((PaymentPageViewModel)BindingContext)?.SelectedPaymentType.PaymentKind;
            if (selectyedKind != null && selectyedKind != "CA" && selectyedKind != "CK")
            {
                return;
            }
            //Unsubscribe
            TotalReceived.TextChanged -= TotalReceived_TextChanged;
            ChangeDue.TextChanged -= ChangeDue_TextChanged;
            //Update text
            var newText = e.NewTextValue;
            if (!string.IsNullOrEmpty(newText) && !newText.IsFirstCharacterNumber())
                newText = newText.Substring(1);
            TotalReceived.EditorText = decimal.TryParse(newText, out decimal value) ? value.ToString("c") : "";
            ChangeDue.EditorText = 0.0.ToString("c");
            //Subscribe
            TotalReceived.TextChanged += TotalReceived_TextChanged;
            ChangeDue.TextChanged += ChangeDue_TextChanged;
        }

        private void TotalReceived_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == null)
                return;
            //Unsubscribe
            Payment.TextChanged -= Payment_TextChanged;
            ChangeDue.TextChanged -= ChangeDue_TextChanged;
            //Update text
            var newText = e.NewTextValue;
            if (!string.IsNullOrEmpty(newText) && !newText.IsFirstCharacterNumber())
                newText = newText.Substring(1);

            Payment.EditorText = 0.0.ToString("c");
            ChangeDue.EditorText = decimal.TryParse(newText, out decimal value) ? value.ToString("c") : "";
            //Subscribe
            Payment.TextChanged += Payment_TextChanged;
            ChangeDue.TextChanged += ChangeDue_TextChanged;
        }

        private void ChangeDue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == null)
                return;
            //Unsubscribe
            Payment.TextChanged -= Payment_TextChanged;
            //Update text
            var newText = e.NewTextValue;
            if (!string.IsNullOrEmpty(newText) && !newText.IsFirstCharacterNumber())
                newText = newText.Substring(1);
            var newValue = string.IsNullOrEmpty(newText) ? 0.ToString() : newText;
            Payment.EditorText = decimal.TryParse(TotalReceived.EditorText?.Substring(1), out decimal total) && decimal.TryParse(newValue, out decimal due)
                ? (total - due).ToString("c")
                : "";
            //Subscribe
            Payment.TextChanged += Payment_TextChanged;
        }

        private void Payment_Unfocused(object sender, FocusEventArgs e)
        {
            Payment.TextChanged -= Payment_TextChanged;
            var newText = Payment.EditorText;
            if (!string.IsNullOrEmpty(newText) && !newText.IsFirstCharacterNumber())
                newText = newText.Substring(1);
            Payment.EditorText = decimal.TryParse(newText, out decimal value) ? value.ToString("c") : "";
            Payment.TextChanged += Payment_TextChanged;
        }

        private void TotalReceived_Unfocused(object sender, FocusEventArgs e)
        {
            TotalReceived.TextChanged -= TotalReceived_TextChanged;
            var newText = TotalReceived.EditorText;
            if (!string.IsNullOrEmpty(newText) && !newText.IsFirstCharacterNumber())
                newText = newText.Substring(1);

            TotalReceived.EditorText = decimal.TryParse(newText, out decimal value) ? value.ToString("c") : "";
            TotalReceived.TextChanged += TotalReceived_TextChanged;
        }

        private void ChangeDue_Unfocused(object sender, FocusEventArgs e)
        {
            ChangeDue.TextChanged -= ChangeDue_TextChanged;
            var newText = ChangeDue.EditorText;
            if (!string.IsNullOrEmpty(newText) && !newText.IsFirstCharacterNumber())
                newText = newText.Substring(1);

            ChangeDue.EditorText = decimal.TryParse(newText, out decimal value) ? value.ToString("c") : "";
            ChangeDue.TextChanged += ChangeDue_TextChanged;
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