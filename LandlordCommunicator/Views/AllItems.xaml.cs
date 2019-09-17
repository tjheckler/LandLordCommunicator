using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using LandlordCommunicator.Models;
using LandlordCommunicator.Views;
using LandlordCommunicator.ViewModels;
using System.Collections.ObjectModel;
using System.IO;
using SQLite;

namespace LandlordCommunicator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllItems : ContentPage
    {
        public ObservableCollection<Item> Items { get; set; }
        ItemsViewModel viewModel;

        public AllItems()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();

           
        }

       public void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null) return;
            
        }
        
        async void ViewItem_Clicked(object sender, EventArgs e)
        {
            var item = ItemsListView.SelectedItem as Item;
            if (item == null)
            {
                await DisplayAlert(null, "You Must Select a Item to View", "OK");
            }
            else
            {
                await Navigation.PushModalAsync(new ItemView(new ItemDetailViewModel(item)));

                // Manually deselect item.
                ItemsListView.SelectedItem = null;
            }
        }
        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }


        async void Delete_Clicked(object sender, EventArgs e)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Item.db3");
            var database = new SQLiteAsyncConnection(path);
            //Figure this out
            var item = ItemsListView.SelectedItem;
            if (item == null)
            {
                await DisplayAlert(null, "You Must Select an Item to Delete", "OK");

            }
            else
            {
                if (await database.Table<Item>().CountAsync() == 1)
                {
                    await database.DropTableAsync<Item>();
                    await database.CreateTableAsync<Item>();
                }
                else
                {
                    await database.DeleteAsync(item);
                }
                await DisplayAlert(null, "Item Deleted", "OK");
                await Navigation.PushModalAsync(new NavigationPage(new AllItems()));
            }
        }

       

        async void Edit_Clicked(object sender, EventArgs e)
        {
            var item = ItemsListView.SelectedItem as Item;
            if (item == null)
            {
                await DisplayAlert(null, "You Must Select a Item to Edit", "OK");
            }
            else
            {
                await Navigation.PushModalAsync(new ItemEdit(new ItemDetailViewModel(item)));

                // Manually deselect item.
                ItemsListView.SelectedItem = null;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Items = new ObservableCollection<Item>();


            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Item.db3");

            var database = new SQLiteConnection(path);

            database.CreateTable<Item>();

            var items = database.Table<Item>().OrderBy(x => x.Id).ToList();



            ItemsListView.ItemsSource = items;

        }
    }
}