using LandlordCommunicator.Models;
using LandlordCommunicator.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LandlordCommunicator.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemView : ContentPage
	{
		public Item Item { get; set; }

		//path to table if exists or needs created
		string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Item.db3");

		ItemDetailViewModel viewModel;

		public ItemView(ItemDetailViewModel viewModel)
		{
			InitializeComponent();

			BindingContext = this.viewModel = viewModel;
		}
        
        public ItemView()
		{
			InitializeComponent();

			BindingContext = viewModel = new ItemDetailViewModel();

            
           
        }



		//update location
		async void Save_Clicked(object sender, EventArgs e)
		{

			await Navigation.PushModalAsync(new NavigationPage(new AllItems()));
		}

       
    }
}