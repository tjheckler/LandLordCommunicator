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
	public partial class AllToDo : ContentPage
	{
        public ObservableCollection<ToDo> ToDo { get; set; }
        ToDoViewModel viewModel;

        public AllToDo()
        {
            InitializeComponent();

            BindingContext = viewModel = new ToDoViewModel();

           
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var todo = args.SelectedItem as ToDo;
            if (todo == null) return;

        }

        async void ViewItem_Clicked(object sender, EventArgs e)
        {
            var todo = ToDoListView.SelectedItem as ToDo;
            if (todo == null)
            {
                await DisplayAlert(null, "You Must Select a ToDo Item to View", "OK");
            }
            else
            {
                await Navigation.PushModalAsync(new ToDoView(new ToDoDetailViewModel(todo)));

                // Manually deselect item.
                ToDoListView.SelectedItem = null;
            }
        }
        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewToDoPage()));
        }


        async void Delete_Clicked(object sender, EventArgs e)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ToDo.db3");
            var database = new SQLiteAsyncConnection(path);
            //Figure this out
            var todo = ToDoListView.SelectedItem;
            if (todo == null)
            {
                await DisplayAlert(null, "You Must Select a ToDo Item to Delete", "OK");

            }
            else
            {
                if (await database.Table<ToDo>().CountAsync() == 1)
                {
                    await database.DropTableAsync<ToDo>();
                    await database.CreateTableAsync<ToDo>();
                }
                else
                {
                    await database.DeleteAsync(todo);
                }
                await DisplayAlert(null, "ToDo Item Deleted", "OK");
                await Navigation.PushModalAsync(new NavigationPage(new AllToDo()));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ToDo = new ObservableCollection<ToDo>();


            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ToDo.db3");

            var database = new SQLiteConnection(path);

            database.CreateTable<ToDo>();

            var todo = database.Table<ToDo>().OrderBy(x => x.Id).ToList();



            ToDoListView.ItemsSource = todo;

        }

        async void Edit_Clicked(object sender, EventArgs e)
        {
            var todo = ToDoListView.SelectedItem as ToDo;
            if (todo == null)
            {
                await DisplayAlert(null, "You Must Select a ToDo Item to Edit", "OK");
            }
            else
            {
                await Navigation.PushModalAsync(new ToDoEdit(new ToDoDetailViewModel(todo)));

                // Manually deselect item.
                ToDoListView.SelectedItem = null;
            }
        }
    }
}