using LandlordCommunicator.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LandlordCommunicator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();

           


           

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Home, Title="Home" },
                new HomeMenuItem {Id = MenuItemType.Contractors, Title="Contractors" },
                new HomeMenuItem {Id = MenuItemType.Items, Title="Inventory" },
                new HomeMenuItem {Id = MenuItemType.Locations, Title="Locations" },
                 new HomeMenuItem {Id = MenuItemType.Reporting, Title="Reports" },
                new HomeMenuItem {Id = MenuItemType.Tenants, Title="Tenants" },
                new HomeMenuItem {Id = MenuItemType.ToDo, Title="ToDo List" },
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
                

            };
        }
    }
}