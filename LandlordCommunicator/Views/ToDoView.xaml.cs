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
	public partial class ToDoView : ContentPage
	{
        public ToDo ToDo { get; set; }

        //path to table if exists or needs created
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ToDo.db3");

        ToDoDetailViewModel viewModel;

        public ToDoView(ToDoDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public ToDoView()
        {
            InitializeComponent();

            BindingContext = viewModel = new ToDoDetailViewModel();


        }

       
        //update location
        async void Save_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushModalAsync(new NavigationPage(new AllToDo()));
        }
    }
}