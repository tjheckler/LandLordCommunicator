using LandlordCommunicator.Models;
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
	public partial class AllTenants : ContentPage
	{
		public ObservableCollection<Tenant> Tenants { get; set; }
		AllTenantsViewModel viewModel;
        public AllTenants()
        {
            InitializeComponent();

            BindingContext = viewModel = new AllTenantsViewModel();

           
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var tenant = args.SelectedItem as Tenant;
            if (tenant == null) return;

        }

        async void EditItem_Clicked(object sender, EventArgs e)
        {
            var tenant = TenantsListView.SelectedItem as Tenant;
            if (tenant == null)
            {
                await DisplayAlert(null, "You Must Select a Tenant to Edit", "OK");
            }
            else
            {
                await Navigation.PushModalAsync(new TenantEdit(new SelectedTenantViewModel(tenant)));

                // Manually deselect item.
                TenantsListView.SelectedItem = null;
            }
        }

        async void ViewItem_Clicked(object sender, EventArgs e)
        {
            var tenant = TenantsListView.SelectedItem as Tenant;
            if (tenant == null)
            {
                await DisplayAlert(null, "You Must Select a Tenant to View", "OK");
            }
            else
            {
                await Navigation.PushModalAsync(new TenantView(new SelectedTenantViewModel(tenant)));

                // Manually deselect item.
                TenantsListView.SelectedItem = null;
            }
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewTenantPage()));
        }


        async void Delete_Clicked(object sender, EventArgs e)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tenant.db3");
            var database = new SQLiteAsyncConnection(path);
            //Figure this out
            var tenant = TenantsListView.SelectedItem;
            if (tenant == null)
            {
                await DisplayAlert(null, "You must select a Tenant to delete", "OK");

            }
            else
            {
                if (await database.Table<Tenant>().CountAsync() == 1)
                {
                    await database.DropTableAsync<Tenant>();
                    await database.CreateTableAsync<Tenant>();
                }
                else
                {
                    await database.DeleteAsync(tenant);
                }
                await DisplayAlert(null, "Tenant Deleted", "OK");
                await Navigation.PushModalAsync(new NavigationPage(new AllTenants()));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            Tenants = new ObservableCollection<Tenant>();


            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tenant.db3");

            var database = new SQLiteConnection(path);

            database.CreateTable<Tenant>();

            var tenants = database.Table<Tenant>().OrderBy(x => x.Id).ToList();

            TenantsListView.ItemsSource = tenants;
        }
    }
}