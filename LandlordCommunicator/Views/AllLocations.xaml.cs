using LandlordCommunicator.Models;
using LandlordCommunicator.Services;
using LandlordCommunicator.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LandlordCommunicator.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AllLocations : ContentPage
	{
		public ObservableCollection<Locations> Locations { get; set; }
       
		AllLocationsViewModel viewModel;

		public AllLocations()
		{
			InitializeComponent();

			BindingContext = viewModel = new AllLocationsViewModel();

			
		}

		 void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
		{
			var location = args.SelectedItem as Locations;
			if (location == null) return;
            
		}

        async void ViewItem_Clicked(object sender, EventArgs e)
        {
            var location = LocationsListView.SelectedItem as Locations;
            if (location == null)
            {
                await DisplayAlert(null, "You Must Select a Location to View", "OK");
            }
            else
            {
                await Navigation.PushModalAsync(new LocationView(new SelectedLocationViewModel(location)));

                // Manually deselect item.
                LocationsListView.SelectedItem = null;
            }
        }
        async void AddItem_Clicked(object sender, EventArgs e)
		{
           
            
			await Navigation.PushModalAsync(new NavigationPage(new NewLocationPage()));
		}

		
		async void Delete_Clicked(object sender, EventArgs e)
		{
			var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Location.db3");
			var database = new SQLiteAsyncConnection(path);
			
			var location = LocationsListView.SelectedItem;
			if (location == null)
			{
				await DisplayAlert(null, "You must select a location to delete", "OK");
				
			}
			else
			{
				if (await database.Table<Locations>().CountAsync() == 1)
				{
					await database.DropTableAsync<Locations>();
					await database.CreateTableAsync<Locations>();
				}
				else
				{
					await database.DeleteAsync(location);
				}
				await DisplayAlert(null, "Location Deleted", "OK");
				await Navigation.PushModalAsync(new NavigationPage(new AllLocations()));
			}
		}

       

        async void Edit_Clicked(object sender, EventArgs e)
        {
            var location = LocationsListView.SelectedItem as Locations;
            if (location == null)
            {
                await DisplayAlert(null, "You Must Select a Location to Edit", "OK");
            }
            else
            {
                await Navigation.PushModalAsync(new LocationEdit(new SelectedLocationViewModel(location)));

                // Manually deselect item.
                LocationsListView.SelectedItem = null;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Locations = new ObservableCollection<Locations>();

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Location.db3");

            var database = new SQLiteConnection(path);

            database.CreateTable<Locations>();

            var locations = database.Table<Locations>().OrderBy(x => x.Id).ToList();



            LocationsListView.ItemsSource = locations;

        }
    }
}