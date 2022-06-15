using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Payments.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HybridWebViewPage : ContentPage
    {
        public HybridWebViewPage()
        {
            InitializeComponent();
            hybridWebView.RegisterAction(data => DisplayDataFromJavascript(data));
            hybridWebView.Source = "https://visumbanyan.fpsdns.com:8080/cardconnect/manual.html";
            hybridWebView.Navigated += HybridWebView_Navigated;
            //    hybridWebView.Source = new HtmlWebViewSource()
            //    {
            //        Html =
            //$@"<html>" +
            //"<head>" +
            //    "<script type=\"text/javascript\">" +
            //        "function invokexamarinforms(){" +
            //        "    try{" +
            //        "        var inputvalue = document.getElementById(\"textInputElement\").value;" +
            //        "        invokeCSharpAction(inputvalue + '. This is from Javascript in the WebView!');" +
            //        "    }" +
            //        "    catch(err){" +
            //        "        alert(err);" +
            //        "    }" +
            //        "}" +
            //    "</script>" +
            //"</head>" +

            //"<body>" +
            //    "<div>" +
            //        "<input type=\"text\" id=\"textInputElement\" placeholder=\"type something here...\">" + 
            //        "<button type=\"button\" onclick=\"invokexamarinforms()\">Send to Xamarin.Forms</button" 
            //        +"</div>" 
            //        +"</body>" 
            //        +"</html>"
            //    };
        }

        private Task DisplayDataFromJavascript(string data)
        {
            return DisplayAlert("Alert", "Hello " + data, "OK");
        }

        private void HybridWebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            hybridWebView.EvaluateJavaScriptAsync("document.getElementById('firstname').setAttribute('value', 'Nitin')");
            hybridWebView.EvaluateJavaScriptAsync("document.getElementById('lastname').setAttribute('value', 'Patil')");
            hybridWebView.EvaluateJavaScriptAsync("document.getElementById('address').setAttribute('value', 'Street 6')");
            hybridWebView.EvaluateJavaScriptAsync("document.getElementById('zip').setAttribute('value', '411015')");
            hybridWebView.EvaluateJavaScriptAsync("document.getElementById('tokenframe').setAttribute('src', 'https://boltgw.cardconnect.com:6443/itoke/ajax-tokenizer.html?invalidinputevent=true&tokenizewheninactive=false')");
        }
    }
}