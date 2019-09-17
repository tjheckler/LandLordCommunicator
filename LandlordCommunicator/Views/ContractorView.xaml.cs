using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LandlordCommunicator.Models;
using LandlordCommunicator.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LandlordCommunicator.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContractorView : ContentPage
	{
        public Contractor Contractor { get; set; }

        //path to table if exists or needs created
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Contractor.db3");

        SelectedContractorViewModel viewModel;

        public ContractorView(SelectedContractorViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public ContractorView()
        {
            InitializeComponent();

            BindingContext = viewModel = new SelectedContractorViewModel();


        }



        //update tenant
        async void Save_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushModalAsync(new NavigationPage(new AllContractors()));
        }

    }
}