using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPtMbl.MainMenu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupItemView : AbsoluteLayout
    {
        public event EventHandler TappedControlShortcut;
        public GroupItemView()
        {
            InitializeComponent();
        }
        void GroupItemViewTapped(object sender, EventArgs e) => TappedControlShortcut.Invoke(sender, e);

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            base.LayoutChildren(x, y, width, height);
        }
    }
}