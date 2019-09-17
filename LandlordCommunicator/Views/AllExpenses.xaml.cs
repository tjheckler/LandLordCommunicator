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
	public partial class AllExpenses : ContentPage
	{
        public ObservableCollection<Expense> Expenses { get; set; }
        AllExpensesViewModel viewModel;
        public AllExpenses()
        {
            InitializeComponent();

            BindingContext = viewModel = new AllExpensesViewModel();

            
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var expense = args.SelectedItem as Expense;
            if (expense == null) return;

        }

        async void EditItem_Clicked(object sender, EventArgs e)
        {
            var expense = ExpensesListView.SelectedItem as Expense;
            if (expense == null)
            {
                await DisplayAlert(null, "You Must Select an Expense to Edit", "OK");
            }
            else
            {
                await Navigation.PushModalAsync(new ExpenseEdit(new SelectedExpenseViewModel(expense)));

                // Manually deselect item.
                ExpensesListView.SelectedItem = null;
            }
        }

        async void ViewItem_Clicked(object sender, EventArgs e)
        {
            var expense = ExpensesListView.SelectedItem as Expense;
            if (expense == null)
            {
                await DisplayAlert(null, "You Must Select an Expense to View", "OK");
            }
            else
            {
                await Navigation.PushModalAsync(new ExpenseView(new SelectedExpenseViewModel(expense)));

                // Manually deselect item.
                ExpensesListView.SelectedItem = null;
            }
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewExpensePage()));
        }


        async void Delete_Clicked(object sender, EventArgs e)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Expense.db3");
            var database = new SQLiteAsyncConnection(path);
            //Figure this out
            var expense = ExpensesListView.SelectedItem;
            if (expense == null)
            {
                await DisplayAlert(null, "You must select an Expense to delete", "OK");

            }
            else
            {
                if (await database.Table<Expense>().CountAsync() == 1)
                {
                    await database.DropTableAsync<Expense>();
                    await database.CreateTableAsync<Expense>();
                }
                else
                {
                    await database.DeleteAsync(expense);
                }
                await DisplayAlert(null, "Expense Deleted", "OK");
                await Navigation.PushModalAsync(new NavigationPage(new AllExpenses()));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Expenses = new ObservableCollection<Expense>();


            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Expense.db3");

            var database = new SQLiteConnection(path);

            database.CreateTable<Expense>();

            var expenses = database.Table<Expense>().OrderBy(x => x.Id).ToList();

            ExpensesListView.ItemsSource = expenses;

        }
    }
}