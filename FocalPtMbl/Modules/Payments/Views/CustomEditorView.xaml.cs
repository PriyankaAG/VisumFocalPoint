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
	public partial class CustomEditorView : ContentView
	{
		public CustomEditorView ()
		{
			InitializeComponent ();
		}

        public static readonly BindableProperty PlaceHolderProperty = BindableProperty.Create(nameof(PlaceHolder), typeof(string), typeof(CustomEditorView), default(string), BindingMode.TwoWay);
        public string PlaceHolder
        {
            get
            {
                return (string)GetValue(PlaceHolderProperty);
            }

            set
            {
                SetValue(PlaceHolderProperty, value);
            }
        }

        public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(nameof(LabelText), typeof(string), typeof(CustomEditorView), default(string), BindingMode.TwoWay);
        public string LabelText
        {
            get
            {
                return (string)GetValue(LabelTextProperty);
            }

            set
            {
                SetValue(LabelTextProperty, value);
            }
        }

        public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(CustomEditorView), Keyboard.Default, BindingMode.TwoWay);
        public Keyboard Keyboard
        {
            get
            {
                return (Keyboard)GetValue(KeyboardProperty);
            }

            set
            {
                SetValue(KeyboardProperty, value);
            }
        }
    }
}