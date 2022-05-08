using FocalPoint_Mobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace FocalPoint_Mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}