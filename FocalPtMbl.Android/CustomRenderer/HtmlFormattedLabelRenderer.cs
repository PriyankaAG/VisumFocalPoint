using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using FocalPoint.CustomControls;
using FocalPtMbl.Droid.CustomRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HtmlFormattedLabel), typeof(HtmlFormattedLabelRenderer))]
namespace FocalPtMbl.Droid.CustomRenderer
{
    [Obsolete]
    public class HtmlFormattedLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            var view = (HtmlFormattedLabel)Element;
            if (view?.Text == null) return;

            Control.SetText(Html.FromHtml(view.Text.ToString(), FromHtmlOptions.OptionUseCssColors), TextView.BufferType.Spannable);
        }
    }
}