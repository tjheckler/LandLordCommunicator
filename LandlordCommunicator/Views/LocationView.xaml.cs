using LandlordCommunicator.Models;
using LandlordCommunicator.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.ExternalMaps;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LandlordCommunicator.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LocationView : ContentPage
	{
		public Locations Location { get; set; }

		//path to table if exists or needs created
		string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Location.db3");

		SelectedLocationViewModel viewModel;

		public LocationView(SelectedLocationViewModel viewModel)
		{
			InitializeComponent();

			BindingContext = this.viewModel = viewModel;

            if (LATITUDE.Text == "0" || LONGITUDE.Text == "0")
            {
                NAVIGATE.IsVisible = false;
                NAVIGATE.IsEnabled = false;
            }
        }

		public LocationView()
		{
			InitializeComponent();

			BindingContext = viewModel = new SelectedLocationViewModel();

            if(LATITUDE.Text == "0" || LONGITUDE.Text == "0")
            {
                NAVIGATE.IsVisible = false;
                NAVIGATE.IsEnabled = false;
            }
            

		}

       

		//update location
		async void Save_Clicked(object sender, EventArgs e)
		{
		   
			await Navigation.PushModalAsync(new NavigationPage(new AllLocations()));
		}

       
    }
}
