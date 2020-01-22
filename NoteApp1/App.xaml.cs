using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace NoteApp1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            //foreach (Frame frame in main.Resources)
            //{
            //    newName = leftCounter++.ToString() + "Left.txt";
            //    string newFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), newName);
            //    File.WriteAllText(newFile, ((Label)frame.Content).Text);
            //}
            //foreach (Frame frame in Right.Children)
            //{
            //    newName = leftCounter++.ToString() + "Right.txt";
            //    string newFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), newName);
            //    File.WriteAllText(newFile, ((Label)frame.Content).Text);
            //}
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
