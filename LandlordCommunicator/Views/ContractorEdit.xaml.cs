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
	public partial class ContractorEdit : ContentPage
	{
        public Contractor Contractor { get; set; }

        //path to table if exists or needs created
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Contractor.db3");

        SelectedContractorViewModel viewModel;
        public ContractorEdit(SelectedContractorViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public ContractorEdit()
        {
            InitializeComponent();

            BindingContext = viewModel = new SelectedContractorViewModel();


        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            var database = new SQLiteAsyncConnection(path);

           
                Contractor contractor = new Contractor()
                {
                    
                    Id = Convert.ToInt32(ID.Text),
                    CompanyName = COMPANYNAME.Text,
                    Description = DESCRIPTION.Text,
                    ContractorType = CONTRACTORTYPE.Text,
                    PhoneNumber = PHONENUMBER.Text,
                    
                };

                await database.UpdateAsync(contractor);
                await DisplayAlert(null, "Contractor Updated Successfully", "OK");
                await Navigation.PopModalAsync();
          
           
        }


       


    }
}