using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;

namespace NoteApp1
{
    public partial class MainPage : ContentPage
    {
        public int counter = 0;
        public MainPage()
        {       
            InitializeComponent();        
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var newPage1 = new Page1();
            newPage1.Disappearing += (object a, EventArgs b) => {
                Frame frame = new Frame();
                Label label = new Label();
                label.Text = newPage1.pageText.Split('\n')[0];
                label.BackgroundColor = Color.Bisque;
                frame.BackgroundColor = Color.Aqua;
                var c = new TapGestureRecognizer();
                c.Tapped += (tapSender, tapEventArg) =>
                {
                    Page1 anotherPage1 = new Page1(newPage1.pageText);
                    Navigation.PushAsync(anotherPage1);
                    anotherPage1.Disappearing += (object anotherA, EventArgs anotherB) =>
                    {
                        label.Text = anotherPage1.pageText.Split('\n')[0];
                        newPage1.pageText = anotherPage1.pageText;
                    };
                };
                frame.Content = label;
                frame.GestureRecognizers.Add(c);
                //myStackLayout.Children.Add(label);
                var t = new SwipeGestureRecognizer();
                if (Left.Height > Right.Height)
                {
                    t.Direction = SwipeDirection.Right;
                    t.Swiped += async (swipeSender, swipeEventArg) =>
                    {
                        if (await DisplayAlert("Confirm the deleting", "Are you sure?", "Yes!", "No"))
                        {
                            Right.Children.Remove(frame);
                        }
                    };
                    frame.GestureRecognizers.Add(t);
                    Right.Children.Add(frame);

                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    string fileName = Path.Combine(path, counter.ToString() + ".txt");

                    File.WriteAllText(fileName, label.Text);
                }
                else
                {
                    t.Direction = SwipeDirection.Left;
                    t.Swiped += async (swipeSender, swipeEventArg) =>
                    {
                        if (await DisplayAlert("Confirm the deleting", "Are you sure?", "Yes!", "No"))
                        {
                            Left.Children.Remove(frame);
                        }
                    };
                    frame.GestureRecognizers.Add(t);
                    Left.Children.Add(frame);
                }
            };

            Navigation.PushAsync(newPage1);
        }
    }
}
