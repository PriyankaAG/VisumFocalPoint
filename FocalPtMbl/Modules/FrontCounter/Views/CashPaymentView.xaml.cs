using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CashPaymentView : ContentPage
    {
        public CashPaymentView()
        {
            this.Title = "Cash Payment";
            InitializeComponent();
        }

        private void TextEdit_TextChanged(object sender, EventArgs e)
        {

        }

        private void SimpleButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}