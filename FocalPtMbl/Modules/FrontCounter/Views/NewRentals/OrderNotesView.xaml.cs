using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FocalPoint.Modules.FrontCounter.Views.NewRentals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderNotesView : ContentPage
    {
        public Tuple<string, string> Notes { get; set; }
        public OrderNotesView(Tuple<string, string> notes)
        {
            InitializeComponent();
            Title = "Notes";
            Notes = notes;

            EditorInternalNotes.Text = Notes.Item1;
            EditorPrintNotes.Text = Notes.Item2;
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Tuple<string, string> result = new Tuple<string, string>(EditorInternalNotes.Text, EditorPrintNotes.Text);
            MessagingCenter.Send(this, "NotesAdded", result);
            this.Navigation.PopAsync();
        }
    }
}