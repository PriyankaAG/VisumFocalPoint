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
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using Google.Android.Material.Navigation;
using FocalPtMbl.Droid.CustomRenderer;
using Google.Android.Material.Tabs;
using AndroidX.AppCompat.Widget;
using FocalPoint.Modules.Dispatching.Views;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(ExtendedTabbedPageRenderer))]
namespace FocalPtMbl.Droid.CustomRenderer
{
    public class ExtendedTabbedPageRenderer : TabbedPageRenderer
    {
       
        public ExtendedTabbedPageRenderer(Context ctx) : base(ctx)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.TabbedPage> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && e.NewElement is ScheduleDispatchingPageView)
            {
                TabLayout tablayout = (TabLayout)ViewGroup.GetChildAt(1);
                tablayout.ScrollBarStyle = ScrollbarStyles.OutsideOverlay;
                tablayout.TabMode = TabLayout.ModeScrollable;
                tablayout.SetSelectedTabIndicatorColor(Android.Graphics.Color.Blue);
                tablayout.SetSelectedTabIndicatorHeight(10);

            }
            else if (e.NewElement != null && e.NewElement is PickupTicketPage)
            {
                TabLayout tablayout = (TabLayout)ViewGroup.GetChildAt(1);
                tablayout.ScrollBarStyle = ScrollbarStyles.OutsideOverlay;
                tablayout.SetSelectedTabIndicatorColor(Android.Graphics.Color.Blue);
                tablayout.SetSelectedTabIndicatorHeight(10);
            }
            if (e.OldElement != null)
            {
                
            }
        }
    }
}