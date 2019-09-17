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
	public partial class AllContractors : ContentPage
	{
        public ObservableCollection<Contractor> Contractors { get; set; }

        public AllContractors ()
		{
			InitializeComponent ();

            BindingContext = this;

           
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var contractor = args.SelectedItem as Contractor;
            if (contractor == null) return;

        }

        async void EditItem_Clicked(object sender, EventArgs e)
        {
            var contractor = ContractorsListView.SelectedItem as Contractor;
            if (contractor == null)
            {
                await DisplayAlert(null, "You Must Select a Contractor to Edit", "OK");
            }
            else
            {
                await Navigation.PushModalAsync(new ContractorEdit(new SelectedContractorViewModel(contractor)));

                // Manually deselect item.
                ContractorsListView.SelectedItem = null;
            }
        }

        async void ViewItem_Clicked(object sender, EventArgs e)
        {
            var contractor= ContractorsListView.SelectedItem as Contractor;
            if (contractor == null)
            {
                await DisplayAlert(null, "You Must Select a Contractor to View", "OK");
            }
            else
            {
                await Navigation.PushModalAsync(new ContractorView(new SelectedContractorViewModel(contractor)));

                // Manually deselect item.
                ContractorsListView.SelectedItem = null;
            }
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewContractorPage()));
        }


        async void Delete_Clicked(object sender, EventArgs e)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Contractor.db3");
            var database = new SQLiteAsyncConnection(path);
            //Figure this out
            var contractor = ContractorsListView.SelectedItem;
            if (contractor == null)
            {
                await DisplayAlert(null, "You must select a Contractor to delete", "OK");

            }
            else
            {
                if (await database.Table<Contractor>().CountAsync() == 1)
                {
                    await database.DropTableAsync<Contractor>();
                    await database.CreateTableAsync<Contractor>();
                }
                else
                {
                    await database.DeleteAsync(contractor);
                }
                await DisplayAlert(null, "Contractor Deleted", "OK");
                await Navigation.PushModalAsync(new NavigationPage(new AllContractors()));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Contractors = new ObservableCollection<Contractor>();


            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Contractor.db3");

            var database = new SQLiteConnection(path);

            database.CreateTable<Contractor>();

            var contractors = database.Table<Contractor>().OrderBy(x => x.Id).ToList();

            ContractorsListView.ItemsSource = contractors;

        }
    }
}