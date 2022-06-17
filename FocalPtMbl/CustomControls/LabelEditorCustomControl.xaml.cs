using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LabelEditorCustomControl : ContentView
    {
        public event EventHandler<TextChangedEventArgs> TextChanged;
        public event EventHandler<FocusEventArgs> TextUnfocused;
        public LabelEditorCustomControl ()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty PlaceHolderProperty = BindableProperty.Create(nameof(PlaceHolder), typeof(string), typeof(LabelEditorCustomControl), default(string), BindingMode.TwoWay);
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

        public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(nameof(LabelText), typeof(string), typeof(LabelEditorCustomControl), default(string), BindingMode.TwoWay);
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

        public static readonly BindableProperty EditorTextProperty = BindableProperty.Create(nameof(EditorText), typeof(string), typeof(LabelEditorCustomControl), default(string), BindingMode.TwoWay);
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

        public static readonly BindableProperty MaxLenthProperty = BindableProperty.Create(nameof(MaxLenth), typeof(int), typeof(LabelEditorCustomControl), default(int), BindingMode.TwoWay);
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

        public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(LabelEditorCustomControl), Keyboard.Default, BindingMode.TwoWay);
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

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(LabelEditorCustomControl), null, BindingMode.TwoWay);
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

        public static readonly BindableProperty IsEnabledProperty = BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(LabelEditorCustomControl), true, BindingMode.TwoWay);
        public bool IsEnabled
        {
            get
            {
                return (bool)GetValue(IsEnabledProperty);
            }

            set
            {
                SetValue(IsEnabledProperty, value);
            }
        }

        private void CustomEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged?.Invoke(sender, e);
        }

        private void CustomEntry_Unfocused(object sender, FocusEventArgs e)
        {
            TextUnfocused?.Invoke(sender, e);
        }
    }
}