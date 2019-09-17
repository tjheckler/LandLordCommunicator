using LandlordCommunicator.Models;
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
	public partial class NewExpensePage : ContentPage
	{
		 //declare location 
         public Locations Location { get; set; }
        public Expense Expense { get; set; }
        public Contractor Contractor = new Contractor();
        public List<Expense> Expenses { get; set; }
        public Invoice Invoice { get; set; }
        public ObservableCollection<Contractor> Contractors { get; set; }
        public ObservableCollection<Locations> Locations { get; set; }
        public ObservableCollection<Invoice> Invoices { get; set; }
        //path to table if exists or needs created
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Location.db3");
        string path2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Contractor.db3");
        string path3 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Invoice.db3");
        string path4 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Expense.db3");
        //loads new page
        public NewExpensePage()
        {

            InitializeComponent();

            BindingContext = this;
            var database = new SQLiteConnection(path);
            var database2 = new SQLiteConnection(path2);
            var database3 = new SQLiteConnection(path3);
            var database4 = new SQLiteConnection(path4);

            Contractors = new ObservableCollection<Contractor>();
            Locations = new ObservableCollection<Locations>();
            Invoices = new ObservableCollection<Invoice>();

            var locations = database.Table<Locations>().OrderBy(x => x.Id).ToList();
            var contractors = database2.Table<Contractor>().OrderBy(x => x.Id).ToList();
            var invoices = database3.Table<Invoice>().OrderBy(x => x.Id).ToList();

            //ContractorPicker
            foreach (Contractor contractor in contractors)
            {
                Contractors.Add(contractor);
            }
            
                Contractor vacant = new Contractor();
                {
                    vacant.CompanyName = "None ";
                    vacant.ContractorType = "Contractor";
                    vacant.Description = "Empty";
                    vacant.PhoneNumber = "N/A";
                    vacant.Id = 100001;
                }
                Contractors.Add(vacant);
            
            ContractorPicker.ItemsSource = Contractors;

            //LocationPicker
            foreach (Locations location in locations)
            {
                Locations.Add(location);
            }
            Locations vacant2 = new Locations();
            {
                vacant2.Image = "/Assets/Icon4-72.png";
                vacant2.Name = "None";
                vacant2.RentAmount = 0;
                vacant2.Tenant = "N/A";
                vacant2.Id = 100002;
                vacant2.Address = "N/A";
                vacant2.City = "N/A";
                vacant2.Latitude = 0;
                vacant2.Longitude = 0;
                vacant2.Zipcode = "N/a";
                vacant2.Suite = "N/A"
;            }
            Locations.Add(vacant2);
            LocationPicker.ItemsSource = Locations;

            //InvoicePicker
            foreach (Invoice invoice in invoices)
            {
                Invoices.Add(invoice);
            }
            Invoice vacant3 = new Invoice();
            {
                vacant3.Description = "None ";
                vacant3.Image = "/Assets/Icon5-72.png";
                vacant3.Id = 100003;
            }
            Invoices.Add(vacant3);
            InvoicePicker.ItemsSource = Invoices;

        }
        Contractor selectedItem;
        public Contractor SelectedItem
        {
            get { return selectedItem; }
            set
            {
                try
                {
                    selectedItem = value;
                    OnPropertyChanged("SelectedItem");
                    if (selectedItem == null)
                        return;
                }
                catch(Exception ex)
                {
                    DisplayAlert("WHOA!","Well, looks like that is not available at this time", "OK");
                }
            }
        }
        Locations selectedLocation;
        public Locations SelectedLocation
        {
            get { return selectedLocation; }
            set
            {
                try
                {
                    selectedLocation = value;
                    OnPropertyChanged("SelectedLocation");
                    if (selectedLocation == null)
                        return;
                }
                catch (Exception ex)
                {
                    DisplayAlert("WHOA!", "Well, looks like that is not available at this time", "OK");
                }
            }
        }
        Invoice selectedInvoice;
        public Invoice SelectedInvoice
        {
            get { return selectedInvoice; }
            set
            {
                try
                {
                    selectedInvoice = value;
                    OnPropertyChanged("SelectedIvoice");
                    if (selectedInvoice == null)
                        return;
                }
                catch (Exception ex)
                {
                    DisplayAlert("WHOA!", "Well, looks like that is not available at this time", "OK");
                }
            }
        }
        //Add and validate new Location
        async void Save_Clicked(object sender, EventArgs e)
          {
            var database4 = new SQLiteConnection(path4);
            
            database4.CreateTable<Expense>(CreateFlags.ImplicitPK);
            var maxPk = database4.Table<Expense>().OrderByDescending(L => L.Id).FirstOrDefault();
            //load from page
           

            // create database connection
          

           
            //If Empty, Do not Process
            if (EntryDescription.Text == null ||  
                Convert.ToDecimal(EntryExpenseAmount.Text) <= 0 )
            {

                await DisplayAlert("Empty Field!", "All Fields Must Be Complete", "OK");
            }
            else
            {
                
              Expense Expense = new Expense
                {

                    Id = (maxPk == null ? 1 : maxPk.Id + 1),
                    Description = EntryDescription.Text,
                    Location = LocationPicker.Items[LocationPicker.SelectedIndex],
                    Invoice = InvoicePicker.Items[InvoicePicker.SelectedIndex],
                    ExpenseAmount = Convert.ToDecimal(EntryExpenseAmount.Text),
                    Contractor = ContractorPicker.Items[ContractorPicker.SelectedIndex],
                    Payment = IsPaid.Text
                    
                };
                //If not empty, Process
                database4.Insert(Expense);
                await DisplayAlert("Success", "Successfully Added ", "OK");
                //go back to list
                await Navigation.PopToRootAsync();
            }
            
        }

            async void Cancel_Clicked(object sender, EventArgs e)
            {
                //go back to list
                 await Navigation.PopModalAsync();
            
            }

        private void Paid_Toggled(object sender, ToggledEventArgs e)
        {
            YesOrNo.Text = String.Format("{0}", e.Value);
        }
    }
	}
