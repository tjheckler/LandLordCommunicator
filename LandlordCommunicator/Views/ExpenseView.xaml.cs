using LandlordCommunicator.Models;
using LandlordCommunicator.ViewModels;
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
	public partial class ExpenseView : ContentPage
	{
        public Expense Expense { get; set; }

        //path to table if exists or needs created
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Expense.db3");

        SelectedExpenseViewModel viewModel;

        public ExpenseView(SelectedExpenseViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public ExpenseView()
        {
            InitializeComponent();

            BindingContext = viewModel = new SelectedExpenseViewModel();


        }



        //update tenant
        async void Save_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushModalAsync(new NavigationPage(new AllExpenses()));
        }

    }
}