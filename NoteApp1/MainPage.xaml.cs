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
                var pan = new PanGestureRecognizer();

                double totalX = 0.0;
                if (Right.Height < Left.Height)
                {
                    pan.PanUpdated += async (panSender, panArgs) =>
                    {
                        switch (panArgs.StatusType)
                        {
                            case GestureStatus.Canceled:
                            case GestureStatus.Started:
                                frame.TranslationX = 0;
                                break;
                            case GestureStatus.Running:
                                if (panArgs.TotalX > 0)
                                {
                                    frame.TranslationX = panArgs.TotalX;
                                    totalX = panArgs.TotalX;
                                }
                                break;
                            case GestureStatus.Completed:
                                if (totalX > 60)
                                {
                                    if (await DisplayAlert("Confirm the deleting", "Are you sure?", "Yes!", "No"))
                                    {
                                        Right.Children.Remove(panSender as Frame);
                                    }
                                    totalX = 0;
                                }
                                frame.TranslationX = 0;
                                break;
                        }
                    };
                    frame.GestureRecognizers.Add(pan);
                    Right.Children.Add(frame);
                }
                else
                {
                    pan.PanUpdated += async (panSender, panArgs) =>
                    {
                        switch (panArgs.StatusType)
                        {
                            case GestureStatus.Canceled:
                            case GestureStatus.Started:
                                frame.TranslationX = 0;
                                break;
                            case GestureStatus.Running:
                                if (panArgs.TotalX < 0)
                                {
                                    frame.TranslationX = panArgs.TotalX;
                                    totalX = panArgs.TotalX;
                                }
                                break;
                            case GestureStatus.Completed:
                                if (totalX < -60)
                                {
                                    if (await DisplayAlert("Confirm the deleting", "Are you sure?", "Yes!", "No"))
                                    {
                                        Left.Children.Remove(panSender as Frame);
                                    }
                                    totalX = 0;
                                }
                                frame.TranslationX = 0;
                                break;
                        }
                    };
                    frame.GestureRecognizers.Add(pan);
                    Left.Children.Add(frame);
                }
            };

            Navigation.PushAsync(newPage1);
        }
    }
}
