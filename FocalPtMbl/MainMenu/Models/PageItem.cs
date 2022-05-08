using FocalPtMbl.MainMenu.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPtMbl.MainMenu.Models
{
    public class PageItem
    {
        string pageTitle = null;
        string icon = null;
        string ctrlPageTitle = null;
        bool showItemUnderline = true;
        Type module;
        List<PageItem> pageItems;

        IconOverlayText itemOverlayText = IconOverlayText.None;

        public string Icon
        {
            get => string.IsNullOrEmpty(this.icon) ? "default_icon" : this.icon;
            set
            {
                this.icon = value;
            }
        }
        public bool Header { get; set; }

        public string Title { get; set; }
        public string PageTitle
        {
            get => this.pageTitle ?? ControlsPageTitle;
            set { this.pageTitle = value; }
        }
        public string ControlsPageTitle
        {
            get => this.ctrlPageTitle ?? Title;
            set { this.ctrlPageTitle = value; }
        }
        public string Description { get; set; }
        public Type Module
        {
            get { return this.module; }
            set
            {
                this.module = value;
                if (value != null && value.GetInterface("IPageData") != null)
                {
                    this.pageItems = ((IPageData)Activator.CreateInstance(value)).PageItems;
                }
            }
        }
        public List<PageItem> PageItems { get { return this.pageItems; } }
        public bool ShowItemUnderline { get { return this.showItemUnderline; } set { this.showItemUnderline = value; } }
        public IconOverlayText  IconOverlayText { get { return this.itemOverlayText; } set { this.itemOverlayText = value; } }

        public bool ShowBadge { get { return this.itemOverlayText  != IconOverlayText.None; } }
        public string BadgeIcon
        {
            get
            {
                if (this.itemOverlayText == IconOverlayText.Updated)
                {
                    return "badge_updated";
                }
                else if (this.itemOverlayText == IconOverlayText.New)
                {
                    return "badge_new";
                }
                else return string.Empty;
            }
        }
    }
}
