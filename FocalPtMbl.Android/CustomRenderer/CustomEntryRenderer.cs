using Android.Content;
using Android.Views;
using FocalPoint.CustomRenderers;
using FocalPtMbl.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace FocalPtMbl.Droid.CustomRenderer
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }
        }
    }

}