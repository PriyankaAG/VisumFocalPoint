using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FocalPoint.Modules.Payments.Types;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.MainMenu.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentHistoryList : ContentView
    {
        public PaymentHistoryList()
        {
            InitializeComponent();
            BindingContext = new PaymentHistoryDetail();
        }

    }
}