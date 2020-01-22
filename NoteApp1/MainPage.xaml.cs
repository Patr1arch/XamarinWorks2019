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
        public int leftCounter = 0;
        public int rightCounter = 0;
        public MainPage()
        {       
            InitializeComponent();
            string newName = leftCounter.ToString() + "Left.txt";
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), newName);
            while (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath);
                CreateFrame(content, true);
                newName = (++leftCounter).ToString() + "Left.txt";
                filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), newName);
            }

            newName = rightCounter.ToString() + "Right.txt";
            filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), newName);
            while (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath);
                CreateFrame(content, false);
                newName = (++rightCounter).ToString() + "Right.txt";
                filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), newName);
            }
        }

        private void CreateFrame(string text, bool isLeft)
        {
            Frame frame = new Frame();
            Label label = new Label();
            label.Text = text;
            label.LineBreakMode = LineBreakMode.TailTruncation;
            frame.HeightRequest = 75;
            frame.BorderColor = Color.Gray;
            var c = new TapGestureRecognizer();
            c.Tapped += (tapSender, tapEventArg) =>
            {
                Page1 anotherPage1 = new Page1(label.Text);
                anotherPage1.Disappearing += (object anotherA, EventArgs anotherB) =>
                {
                    label.Text = anotherPage1.pageText;
                };
                Navigation.PushAsync(anotherPage1);
            };
            frame.Content = label;
            frame.GestureRecognizers.Add(c);

            var pan = new PanGestureRecognizer();

            double totalX = 0.0;
            if (!isLeft)
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
                                    File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                            --rightCounter + "Right.txt"));
                                    Right.Children.Remove(panSender as Frame);
                                    //if (Math.Abs(Left.Height - Right.Height) > Math.Abs(Left.Height - Right.Height + Left.Children.Count == 0 ?
                                    //    0 : Left.Children.Last().Height) && Left.Children.Count != 0) {
                                    if (Left.Children.Count - Right.Children.Count > 1)
                                    {
                                        var transferFrame = Left.Children.Last();
                                        Left.Children.Remove(transferFrame);
                                        transferFrame.GestureRecognizers.RemoveAt(transferFrame.GestureRecognizers.Count - 1);
                                        transferFrame.GestureRecognizers.Add(pan);
                                        File.Move(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                            --leftCounter + "Left.txt"),
                                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                        rightCounter++ + "Right.txt"));
                                        Right.Children.Add(transferFrame);
                                    }

                                    //}
                                }
                                totalX = 0;
                            }
                            frame.TranslationX = 0;
                            break;
                    }
                };
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
                                    File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                          --leftCounter + "Left.txt"));
                                    Left.Children.Remove(panSender as Frame);
                                    //if (Math.Abs(Right.Height - Left.Height) > Math.Abs(Right.Height - Left.Height + Right.Children.Count == 0 ?
                                    //    0 : Right.Children.Last().Height) && Right.Children.Count != 0)
                                    //{
                                    if (Right.Children.Count - Left.Children.Count > 1)
                                    {
                                        var transferFrame = Right.Children.Last();
                                        Right.Children.Remove(transferFrame);
                                        transferFrame.GestureRecognizers.RemoveAt(transferFrame.GestureRecognizers.Count - 1);
                                        transferFrame.GestureRecognizers.Add(pan);

                                        File.Move(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                            --rightCounter + "Right.txt"),
                                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                        leftCounter++ + "Left.txt"));
                                        Left.Children.Add(transferFrame);
                                    }
                                    //}
                                }
                                totalX = 0;
                            }
                            frame.TranslationX = 0;
                            break;
                    }
                };
            }

            frame.GestureRecognizers.Add(pan);

            if (isLeft) Left.Children.Add(frame);
            else Right.Children.Add(frame);

        }


        private void Button_Clicked(object sender, EventArgs e)
        {
            var newPage1 = new Page1();
            newPage1.Disappearing += (object a, EventArgs b) => {
                if (newPage1.isCancelled) return;
                Frame frame = new Frame();
                Label label = new Label();
                label.Text = newPage1.pageText;
                label.LineBreakMode = LineBreakMode.TailTruncation;
                frame.HeightRequest = 75;
                frame.BorderColor = Color.Gray;
                var c = new TapGestureRecognizer();
                c.Tapped += (tapSender, tapEventArg) =>
                {
                    Page1 anotherPage1 = new Page1(label.Text);
                    anotherPage1.Disappearing += (object anotherA, EventArgs anotherB) =>
                    {
                        label.Text = anotherPage1.pageText;
                    };
                    Navigation.PushAsync(anotherPage1);
                };
                frame.Content = label;
                frame.GestureRecognizers.Add(c);

                var pan = new PanGestureRecognizer();

                string newName;
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
                                        File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                                --rightCounter + "Right.txt"));
                                        Right.Children.Remove(panSender as Frame);
                                        //if (Math.Abs(Left.Height - Right.Height) > Math.Abs(Left.Height - Right.Height + Left.Children.Count == 0 ?
                                        //    0 : Left.Children.Last().Height) && Left.Children.Count != 0) {
                                        if (Left.Children.Count - Right.Children.Count > 1)
                                        {
                                            var transferFrame = Left.Children.Last();
                                            Left.Children.Remove(transferFrame);
                                            transferFrame.GestureRecognizers.RemoveAt(transferFrame.GestureRecognizers.Count - 1);
                                            transferFrame.GestureRecognizers.Add(pan);
                                            File.Move(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                                --leftCounter + "Left.txt"),
                                            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                            rightCounter++ + "Right.txt"));
                                            Right.Children.Add(transferFrame);
                                        }

                                        //}
                                    }
                                    totalX = 0;
                                }
                                frame.TranslationX = 0;
                                break;
                        }
                    };
                    newName = rightCounter++.ToString() + "Right.txt";
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
                                        File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                              --leftCounter + "Left.txt"));
                                        Left.Children.Remove(panSender as Frame);
                                        //if (Math.Abs(Right.Height - Left.Height) > Math.Abs(Right.Height - Left.Height + Right.Children.Count == 0 ?
                                        //    0 : Right.Children.Last().Height) && Right.Children.Count != 0)
                                        //{
                                        if (Right.Children.Count - Left.Children.Count > 1)
                                        {
                                            var transferFrame = Right.Children.Last();
                                            Right.Children.Remove(transferFrame);
                                            transferFrame.GestureRecognizers.RemoveAt(transferFrame.GestureRecognizers.Count - 1);
                                            transferFrame.GestureRecognizers.Add(pan);

                                            File.Move(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                                --rightCounter + "Right.txt"),
                                            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                            leftCounter++ + "Left.txt"));
                                            Left.Children.Add(transferFrame);
                                        }
                                        //}
                                    }
                                    totalX = 0;
                                }
                                frame.TranslationX = 0;
                                break;
                        }
                    };
                    newName = leftCounter++.ToString() + "Left.txt";
                    frame.GestureRecognizers.Add(pan);
                    Left.Children.Add(frame);
                }
                string newFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), newName);
                File.WriteAllText(newFile, ((Label)frame.Content).Text);
            };
           
            Navigation.PushAsync(newPage1);
        }
    }
}
