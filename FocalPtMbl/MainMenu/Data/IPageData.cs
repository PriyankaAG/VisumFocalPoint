using System;
using System.Collections.Generic;
using System.Text;
using FocalPtMbl.MainMenu.Models;

namespace FocalPtMbl.MainMenu.Data
{
    public interface IPageData
    {
        List<PageItem> PageItems { get; }
        String Title { get; }
    }
}
