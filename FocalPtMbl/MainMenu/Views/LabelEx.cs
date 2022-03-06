using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FocalPtMbl.MainMenu.Views
{
    public class LabelEx : Label
    {
        public const int label_size = 100;
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest() { Request = new Size(label_size, label_size), Minimum = new Size(0, 0) };
        }
    }
}
