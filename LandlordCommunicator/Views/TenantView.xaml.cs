using LandlordCommunicator.Models;
using LandlordCommunicator.ViewModels;
using SQLite;
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
	public partial class TenantView : ContentPage
	{
		public Tenant Tenant { get; set; }

		//path to table if exists or needs created
		string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tenant.db3");

		SelectedTenantViewModel viewModel;

		public TenantView(SelectedTenantViewModel viewModel)
		{
			InitializeComponent();

			BindingContext = this.viewModel = viewModel;
		}

		public TenantView()
		{
			InitializeComponent();

			BindingContext = viewModel = new SelectedTenantViewModel();


		}

       

        //update tenant
        async void Save_Clicked(object sender, EventArgs e)
		{
		   
			await Navigation.PushModalAsync(new NavigationPage(new AllTenants()));
		}


	}
}