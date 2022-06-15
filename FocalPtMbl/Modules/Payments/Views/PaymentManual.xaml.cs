using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Payments.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentManual : ContentPage
    {
        public PaymentManual()
        {
            InitializeComponent();
            CardConnectWebView.Navigated += (o, s) => {
                CardConnectWebView.EvaluateJavaScriptAsync("document.getElementById('firstname').setAttribute('value', 'Nitin')");
                CardConnectWebView.EvaluateJavaScriptAsync("document.getElementById('lastname').setAttribute('value', 'Patil')");
                CardConnectWebView.EvaluateJavaScriptAsync("document.getElementById('address').setAttribute('value', 'Street 6')");
                CardConnectWebView.EvaluateJavaScriptAsync("document.getElementById('zip').setAttribute('value', '411015')");
                CardConnectWebView.EvaluateJavaScriptAsync("document.getElementById('tokenframe').setAttribute('src', 'https://boltgw.cardconnect.com:6443/itoke/ajax-tokenizer.html?invalidinputevent=true&tokenizewheninactive=false')");
            };
            //_ = LoadHtml();
        }

        private async Task LoadHtml()
        {
            HttpClient client = new HttpClient();
            var html = await client.GetStringAsync((CardConnectWebView.Source as UrlWebViewSource).Url);

            var html1 = await CardConnectWebView.EvaluateJavaScriptAsync("document.documentElement.outerHTML");
            string decodedHtml = WebUtility.HtmlDecode(html1);
            //WebBrowser.Source = decodedHtml;
        }

        private void CardConnectWebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            var url = e.Url;
        }

        private void CardConnectWebView_Navigated(object sender, WebNavigatedEventArgs e)
        {

        }
    }
}