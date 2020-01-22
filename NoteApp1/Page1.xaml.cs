using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoteApp1
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Page1 : ContentPage
	{
        public string pageText = null;
        public bool isCancelled = true;
        public string lastEdit = null;
        //public int symCount = 0;

		public Page1 ()
		{
			InitializeComponent ();
            pageText = null;
            isCancelled = true;
            symCount.Text = "| " + (String.IsNullOrEmpty(Editor.Text) ? "0 символов" : Editor.Text.Count().ToString() + " " + (Editor.Text.Count() % 10 == 1 ? "символ" :
            Editor.Text.Count() % 10 < 5 && Editor.Text.Count() % 10 != 0 ? "символa" : "символов"));
        }

        public Page1(string text, string dateText)
        {
            InitializeComponent();
            pageText = text;
            Editor.Text = text;
            lastEditLabel.Text = dateText;
            symCount.Text = "| " + (String.IsNullOrEmpty(Editor.Text) ? "0 символов" : Editor.Text.Count().ToString() + " " + 
                (!(Editor.Text.Count() % 100 > 10 && Editor.Text.Count() % 100 < 15) ? (Editor.Text.Count() % 10 == 1 ? "символ" : 
                Editor.Text.Count() % 10 < 5 && Editor.Text.Count() % 10 != 0 ? "символa" : "символов") : "символов"));
        }

        private void SaveChanges(object sender, EventArgs e)
        {
            //fullText = Editor.Text;
            Dictionary<int, string> Months = new Dictionary<int, string> { { 1, "Января" }, { 2, "Февраля" }, { 3, "Марта" } }; // Лень...
            pageText = Editor.Text;
            isCancelled = false;
            var date = DateTime.Now;
            lastEdit = date.Day + " " + Months[date.Month] + " " +
                (date.Hour < 10 ? "0" : "") + date.Hour.ToString() +
                ":" + (date.Minute < 10 ? "0" : "") + date.Minute.ToString(); 
            Navigation.PopAsync();

        }

        private void Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            symCount.Text = "| " + (String.IsNullOrEmpty(Editor.Text) ? "0 символов" : Editor.Text.Count().ToString() + " " +
                (!(Editor.Text.Count() % 100 > 10 && Editor.Text.Count() % 100 < 15) ? (Editor.Text.Count() % 10 == 1 ? "символ" :
                Editor.Text.Count() % 10 < 5 && Editor.Text.Count() % 10 != 0 ? "символa" : "символов") : "символов"));
        }
    }
}