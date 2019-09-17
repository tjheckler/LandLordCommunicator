using LandlordCommunicator.Models;
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
	public partial class NewContractorPage : ContentPage
	{
        //declare location 
        public Contractor Contractor { get; set; }

        //path to table if exists or needs created
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Contractor.db3");

        //loads new page
        public NewContractorPage()
        {

            InitializeComponent();

            BindingContext = this;

        }


        //Add and validate new Location
        async void Save_Clicked(object sender, EventArgs e)
        {
            var database = new SQLiteConnection(path);

            database.CreateTable<Contractor>(CreateFlags.ImplicitPK);
            var maxPk = database.Table<Contractor>().OrderByDescending(L => L.Id).FirstOrDefault();
           

            //If Empty, Do not Process
            if (EntryCompanyName.Text == null || EntryContractorType.Text == null || EntryDescription.Text == null
                || EntryPhoneNumber.Text == null )

            {

                await DisplayAlert(null, "One or More Fields Empty", "OK");
            }
            else
            {
               Contractor = new Contractor()
                {
                   
                    Id = (maxPk == null ? 1 : maxPk.Id + 1),
                    CompanyName = EntryCompanyName.Text,
                    ContractorType = EntryContractorType.Text,
                    PhoneNumber = EntryPhoneNumber.Text,
                    Description = EntryDescription.Text


                };

                //If not empty, Process
                database.Insert(Contractor);
                await DisplayAlert(null, "Successfully Added Contractor", "OK");
                //go back to list
                await Navigation.PopToRootAsync();
            }

        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            //go back to list
            await Navigation.PopModalAsync();

        }

      
        }
}