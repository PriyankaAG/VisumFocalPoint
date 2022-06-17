using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.MainMenu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageEditoCustomControl : ContentView
    {
        public event EventHandler<TextChangedEventArgs> TextChanged;
        public ImageEditoCustomControl()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ImageProperty = BindableProperty.Create(nameof(Image), typeof(string), typeof(ImageEditoCustomControl), 
            default(string), Xamarin.Forms.BindingMode.OneWay);
        public string Image
        {
            get
            {
                return (string)GetValue(ImageProperty);
            }

            set
            {
                SetValue(ImageProperty, value);
            }
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ImageEditoCustomControl), default(string), BindingMode.TwoWay);
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }

            set
            {
                SetValue(TextProperty, value);
            }
        }

        public static readonly BindableProperty PlaceHolderProperty = BindableProperty.Create(nameof(PlaceHolder), typeof(string), typeof(ImageEditoCustomControl), default(string), BindingMode.TwoWay);
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

        public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(nameof(IsPassword), typeof(string), typeof(ImageEditoCustomControl), default(string), BindingMode.TwoWay);
        public string IsPassword
        {
            get
            {
                return (string)GetValue(IsPasswordProperty);
            }

            set
            {
                SetValue(IsPasswordProperty, value);
            }
        }

        public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(ImageEditoCustomControl), Keyboard.Default, BindingMode.TwoWay);
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

        public static readonly BindableProperty CompletedProperty = BindableProperty.Create(nameof(Completed), typeof(ICommand), typeof(ImageEditoCustomControl), null, BindingMode.TwoWay);
        public ICommand Completed
        {
            get
            {
                return (ICommand)GetValue(CompletedProperty);
            }

            set
            {
                SetValue(CompletedProperty, value);
            }
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ImageEditoCustomControl), null, BindingMode.TwoWay);
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

        public static readonly BindableProperty UnfocusedProperty = BindableProperty.Create(nameof(Unfocused), typeof(ICommand), typeof(ImageEditoCustomControl), null, BindingMode.TwoWay);
        public new ICommand Unfocused
        {
            get
            {
                return (ICommand)GetValue(UnfocusedProperty);
            }

            set
            {
                SetValue(UnfocusedProperty, value);
            }
        }
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Text = e.NewTextValue;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            /*if (propertyName == ImageProperty.PropertyName)
            {
                title.Text = Image;
            }
            else if (propertyName == TextProperty.PropertyName)
            {
                entry.Text = Text;
            }*/
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged?.Invoke(sender, e);
        }
    }
}
