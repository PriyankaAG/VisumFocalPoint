﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPtMbl.MainMenu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ControlPage : ContentPage
    {
        public ControlPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            this.collectionView.SelectedItem = null;
            base.OnAppearing();
        }
    }
}