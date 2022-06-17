using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FocalPoint.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LabelDropDownCustomControl : ContentView
    {
        public LabelDropDownCustomControl()
        {
            InitializeComponent();
        }
        public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(nameof(LabelText), typeof(string), typeof(string), default(string), BindingMode.TwoWay);
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

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            propertyName: nameof(ItemsSource),
            returnType: typeof(String[]),
            declaringType: typeof(LabelDropDownCustomControl),
            defaultBindingMode: BindingMode.TwoWay,
            defaultValue: null);

        public String[] ItemsSource
        {
            get { return (String[])GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(
            propertyName: nameof(SelectedIndex),
            returnType: typeof(int),
            declaringType: typeof(LabelDropDownCustomControl),
            defaultBindingMode: BindingMode.TwoWay,
            defaultValue: -1);

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public static readonly BindableProperty IsFirstRowPlaceholderProperty = BindableProperty.Create(
            propertyName: nameof(IsFirstRowPlaceholder),
            returnType: typeof(bool),
            declaringType: typeof(LabelDropDownCustomControl),
            defaultBindingMode: BindingMode.TwoWay,
            defaultValue: false);

        public bool IsFirstRowPlaceholder
        {
            get { return (bool)GetValue(IsFirstRowPlaceholderProperty); }
            set { SetValue(IsFirstRowPlaceholderProperty, value); }
        }

        public static readonly BindableProperty EntryTextColorProperty = BindableProperty.Create(
          propertyName: nameof(EntryTextColor),
          returnType: typeof(string),
          declaringType: typeof(LabelDropDownCustomControl),
            defaultBindingMode: BindingMode.TwoWay,
          defaultValue: "#000000");

        public string EntryTextColor
        {
            get { return (string)GetValue(EntryTextColorProperty); }
            set { SetValue(EntryTextColorProperty, value); }
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            propertyName: nameof(Text),
            returnType: typeof(string),
            declaringType: typeof(LabelDropDownCustomControl),
            defaultBindingMode: BindingMode.TwoWay,
            defaultValue: "");

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
            propertyName: nameof(SelectedItem),
            returnType: typeof(string),
            declaringType: typeof(LabelDropDownCustomControl),
            defaultBindingMode: BindingMode.TwoWay,
            defaultValue: "");

        public string SelectedItem
        {
            get { return (string)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly BindableProperty ImageProperty = BindableProperty.Create(
            propertyName: nameof(Image),
            returnType: typeof(string),
            declaringType: typeof(LabelDropDownCustomControl),
            defaultBindingMode: BindingMode.TwoWay,
            defaultValue: "comboboxdropsmall");

        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly BindableProperty HandleColorProperty = BindableProperty.Create(
            propertyName: nameof(HandleColor),
            returnType: typeof(string),
            declaringType: typeof(LabelDropDownCustomControl),
            defaultBindingMode: BindingMode.TwoWay,
            defaultValue: "#57b8ff");

        public string HandleColor
        {
            get { return (string)GetValue(HandleColorProperty); }
            set { SetValue(HandleColorProperty, value); }
        }

        public event EventHandler<ItemSelectedEventArgs> ItemSelected;

        public void OnItemSelected(int pos)
        {
        }

        private void myPicker_ItemSelected(object sender, ItemSelectedEventArgs e)
        {
            if (IsFirstRowPlaceholder && e.SelectedIndex == 0)
            {
                SelectedItem = null;
            }
            else
            {
                SelectedItem = ItemsSource[e.SelectedIndex];
            }
            Text = ItemsSource[e.SelectedIndex];//.Length > 10 ? ItemsSource[e.SelectedIndex].Substring(0, 10) : ItemsSource[e.SelectedIndex];
            OnPropertyChanged(nameof(Text));
            ItemSelected?.Invoke(this, new ItemSelectedEventArgs() { SelectedIndex = e.SelectedIndex, IsFirstRowPlaceholder = e.IsFirstRowPlaceholder });
        }
    }
}