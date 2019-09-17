using LandlordCommunicator.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LandlordCommunicator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            MenuPages.Add((int)MenuItemType.Home, (NavigationPage)Detail);
           

        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Home:
                        MenuPages.Add(id, new NavigationPage(new HomePage()));
                        break;

                    case (int)MenuItemType.Tenants:
                        MenuPages.Add(id, new NavigationPage(new AllTenants()));
                        break;

                    case (int)MenuItemType.Locations:
                        MenuPages.Add(id, new NavigationPage(new AllLocations()));
                        break;

                    case (int)MenuItemType.Items:
                        MenuPages.Add(id, new NavigationPage(new AllItems()));
                        break;

                    case (int)MenuItemType.ToDo:
                        MenuPages.Add(id, new NavigationPage(new AllToDo()));
                        break;

                    case (int)MenuItemType.Reporting:
                        MenuPages.Add(id, new NavigationPage(new Reporting()));
                        break;

                    case (int)MenuItemType.Contractors:
                        MenuPages.Add(id, new NavigationPage(new AllContractors()));
                        break;

                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}