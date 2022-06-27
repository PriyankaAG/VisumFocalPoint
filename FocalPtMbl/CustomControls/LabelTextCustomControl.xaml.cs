using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LabelTextCustomControl : ContentView
    {
        public LabelTextCustomControl()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(LabelTextCustomControl), default(string), BindingMode.TwoWay);
        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }

            set
            {
                SetValue(TitleProperty, value);
            }
        }

        public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(string), typeof(LabelTextCustomControl), default(string), BindingMode.TwoWay);
        public string Value
        {
            get
            {
                return (string)GetValue(ValueProperty);
            }

            set
            {
                SetValue(ValueProperty, value);
            }
        }

        static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColorValue), typeof(Color), typeof(LabelTextCustomControl), default(string), BindingMode.TwoWay);
        public Color TextColorValue
        {
            get
            {
                return (Color)GetValue(TextColorProperty);
            }

            set
            {
                SetValue(TextColorProperty, value);
            }
        }
    }
}