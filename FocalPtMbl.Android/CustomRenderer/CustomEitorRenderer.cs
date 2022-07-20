using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FocalPoint.CustomRenderer;
using FocalPtMbl.Droid.CustomRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEitorRenderer))]
namespace FocalPtMbl.Droid.CustomRenderer
{
    public class CustomEitorRenderer : EditorRenderer
    {
        public CustomEitorRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }
        }
    }
}