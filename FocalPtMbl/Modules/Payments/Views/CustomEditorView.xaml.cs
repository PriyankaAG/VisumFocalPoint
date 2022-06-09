using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.Payments.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomEditorView : ContentView
	{
        public event EventHandler<TextChangedEventArgs> TextChanged;
        public event EventHandler<FocusEventArgs> Unfocused;
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

        public static readonly BindableProperty EditorTextProperty = BindableProperty.Create(nameof(EditorText), typeof(string), typeof(CustomEditorView), default(string), BindingMode.TwoWay);
        public string EditorText
        {
            get
            {
                return (string)GetValue(EditorTextProperty);
            }

            set
            {
                SetValue(EditorTextProperty, value);
            }
        }

        public static readonly BindableProperty MaxLenthProperty = BindableProperty.Create(nameof(MaxLenth), typeof(int), typeof(CustomEditorView), default(int), BindingMode.TwoWay);
        public int MaxLenth
        {
            get
            {
                return (int)GetValue(MaxLenthProperty);
            }

            set
            {
                SetValue(MaxLenthProperty, value);
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

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CustomEditorView), null, BindingMode.TwoWay);
        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }

            set
            {
                SetValue(CommandProperty, value);
            }
        }

        private void CustomEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged?.Invoke(sender, e);
        }

        private void CustomEntry_Unfocused(object sender, FocusEventArgs e)
        {
            Unfocused?.Invoke(sender, e);
        }
    }
}