using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using FocalPoint.CustomControls;
using FocalPtMbl.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomDropDown), typeof(CustomDropdownRenderer))]
namespace FocalPtMbl.Droid.CustomRenderer
{
    public class CustomDropdownRenderer : ViewRenderer<CustomDropDown, Spinner>
    {
        Spinner spinner;
        public CustomDropdownRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<CustomDropDown> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                spinner = new Spinner(Context);
                SetNativeControl(spinner);
            }

            if (e.OldElement != null)
            {
                Control.ItemSelected -= OnItemSelected;
            }
            if (e.NewElement != null)
            {
                var view = e.NewElement;

                if (view.ItemsSource == null || view.ItemsSource.Length <= 0)
                    return;

                ArrayAdapter adapter = new ArrayAdapter(Context, Android.Resource.Layout.SimpleListItem1, view.ItemsSource);
                Control.Adapter = adapter;
                Control.ForceHasOverlappingRendering(true);
                Control.DropDownVerticalOffset = 150;

                if (view.SelectedIndex != -1)
                {
                    Control.SetSelection(view.SelectedIndex);
                }

                Control.ItemSelected += OnItemSelected;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var view = Element;
            if (e.PropertyName == CustomDropDown.ItemsSourceProperty.PropertyName)
            {
                if (view.ItemsSource == null || view.ItemsSource.Length <= 0)
                    return;

                ArrayAdapter adapter = new ArrayAdapter(Context, Android.Resource.Layout.SimpleListItem1, view.ItemsSource);
                Control.Adapter = adapter;
            }
            if (e.PropertyName == CustomDropDown.SelectedIndexProperty.PropertyName)
            {
                Control.SetSelection(view.SelectedIndex);
                if (view.SelectedIndex == 0 && view.IsFirstRowPlaceholder)
                {
                    view.EntryTextColor = "#60000000";
                }
                else
                { 
                    view.EntryTextColor = "#000000";
                }
            }
            base.OnElementPropertyChanged(sender, e);
        }

        private void OnItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var view = Element;
            if (view != null)
            {
                view.SelectedIndex = e.Position;
                view.OnItemSelected(e.Position);
            }
        }
    }
}