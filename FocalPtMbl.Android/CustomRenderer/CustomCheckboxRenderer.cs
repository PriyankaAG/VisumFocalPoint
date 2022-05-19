using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FocalPtMbl.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.CheckBox), typeof(CustomCheckboxRenderer))]
namespace FocalPtMbl.Droid.CustomRenderer
{
    class CustomCheckboxRenderer : ViewRenderer<Xamarin.Forms.CheckBox, Android.Widget.CheckBox>
    {
        private Android.Widget.CheckBox checkBox;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.CheckBox> e)
        {
            base.OnElementChanged(e);
            var model = e.NewElement;
            checkBox = new Android.Widget.CheckBox(Context);
            checkBox.SetButtonDrawable(Resource.Drawable.custom_checkbox);
            checkBox.Tag = this;
            SetNativeControl(checkBox);
        }
    }
}