using FocalPtMbl.MainMenu.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FocalPoint
{
    public class CollectionViewHeaderTemplateSelector:DataTemplateSelector 
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
