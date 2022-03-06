using FocalPtMbl.MainMenu.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FocalPtMbl
{
    public class Utilities
    {

    }
    public class TitleViewExtensions
    {
        public static BindableProperty IsShadowVisibleProperty =
            BindableProperty.CreateAttached("IsShadowVisible", typeof(bool), typeof(Page), false);
        public static bool GetIsShadowVisible(Page view)
        {
            return (bool)view.GetValue(IsShadowVisibleProperty);
        }

        public static void SetIsShadowVisible(Page view, bool value)
        {
            view.SetValue(IsShadowVisibleProperty, value);
        }
    }
    public class CollectionViewHeaderTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (!(item is PageItem pageItem))
                return null;
            if (pageItem.Header)
            {
                return HeaderTemplate;
            }
            else
            {
                return ItemTemplate;
            }
        }
        public DataTemplate HeaderTemplate { get; set; }
        public DataTemplate ItemTemplate { get; set; }
    }
}
