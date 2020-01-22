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

		public Page1 ()
		{
			InitializeComponent ();
            pageText = null;
            isCancelled = true;
            
        }

        public Page1(string text)
        {
            InitializeComponent();
            pageText = text;
            Editor.Text = text;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            //fullText = Editor.Text;
            pageText = Editor.Text;
            isCancelled = false;
            Navigation.PopAsync();

        }
    }
}