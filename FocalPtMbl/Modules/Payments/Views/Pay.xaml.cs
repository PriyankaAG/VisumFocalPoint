using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Payments.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Pay : ContentView
	{
		public Pay ()
		{
			InitializeComponent ();
		}
		private void Entry_TextChanged(object sender, TextChangedEventArgs e)
		{
			//entryPropertyValue?.TextChanged.Invoke(entry, e);
		}
	}
}