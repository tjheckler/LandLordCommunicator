using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LandlordCommunicator.Views;
using LandlordCommunicator.Services;
using System.IO;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace LandlordCommunicator
{
    public partial class App : Application
    {
       

       
        public App()
        {
            InitializeComponent();


            MainPage = new MainPage();
           Page AllItems = new AllItems();
            Page AllLocations = new AllLocations();
            Page AllTenants = new AllTenants();
            Page AllToDo = new AllToDo();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

       
    }
}
