using LandlordCommunicator.Models;
using LandlordCommunicator.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
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
	public partial class ExpenseEdit : ContentPage
	{
        public Expense Expense { get; set; }
        public Contractor Contractor = new Contractor();
        public Invoice Invoice = new Invoice();
        public ObservableCollection<Contractor> Contractors { get; set; }
        public ObservableCollection<Locations> Locations { get; set; }
        public ObservableCollection<Invoice> Invoices { get; set; }
        //path to table if exists or needs created
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Expense.db3");
        string path2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Contractor.db3");
        string path3 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Location.db3");
        string path4 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Invoice.db3");
        SelectedExpenseViewModel viewModel;

        public ExpenseEdit(SelectedExpenseViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
            var database2 = new SQLiteConnection(path2);
            var database3 = new SQLiteConnection(path3);
            var database4 = new SQLiteConnection(path4);
            Contractors = new ObservableCollection<Contractor>();
            Locations = new ObservableCollection<Locations>();
            Invoices = new ObservableCollection<Invoice>();

            var contractors = database2.Table<Contractor>().OrderBy(x => x.Id).ToList();
            var locations = database3.Table<Locations>().OrderBy(x => x.Id).ToList();
            var invoices = database4.Table<Invoice>().OrderBy(x => x.Id).ToList();

            foreach (Contractor contractor in contractors)
            {
                Contractors.Add(contractor);
            }
            Contractor vacant = new Contractor();
            {
                vacant.CompanyName = "None";
                vacant.ContractorType = "None";
                vacant.Description = "Empty";
                vacant.PhoneNumber = "N/A";
                vacant.Id = 100000;
            }
            Contractors.Add(vacant);
            ContractorPicker.ItemsSource = Contractors;

            foreach (Locations location in locations)
            {
                Locations.Add(location);
            }
            Locations vacant3 = new Locations();
            {
                vacant3.Image = "/Assets/Icon4-72.png";
                vacant3.Address = "None";
                vacant3.City = "Empty";
                vacant3.Latitude = 0;
                vacant3.Longitude = 0;
                vacant3.Name = "None";
                vacant3.RentAmount = 0;
                vacant3.Suite = "N/A";
                vacant3.Tenant = "None";
                vacant3.Zipcode = "N/A";
                vacant3.Id = 100001;
            }
            Locations.Add(vacant3);
            LocationPicker.ItemsSource = Locations;

            foreach (Invoice invoice in invoices)
            {
                Invoices.Add(invoice);
            }
            Invoice vacant4 = new Invoice();
            {
                vacant4.Description = "None";
                vacant4.Image = "/Assets/Icon5-72.png";
                vacant4.Id = 100002;
            }
            Invoices.Add(vacant4);
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
                catch (Exception ex)
                {
                    DisplayAlert("WHOA!", "Well, looks like that is not available at this time", "OK");
                }
            }
        }
        Location selectedLocation;
        public Location SelectedLocation
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
                    OnPropertyChanged("SelectedInvoice");
                    if (selectedInvoice == null)
                        return;
                }
                catch (Exception ex)
                {
                    DisplayAlert("WHOA!", "Well, looks like that is not available at this time", "OK");
                }
            }
        }
        //update location
        async void Save_Clicked(object sender, EventArgs e)
        {
            var database = new SQLiteAsyncConnection(path);
            //TODO: Fix If tenant does not need to be changed exception problem
            if (ContractorPicker.SelectedItem == null|| LocationPicker.SelectedItem == null || InvoicePicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Must Re-Select Contractor, Location, Invoice, or Mark None in Edit Mode", "OK");
            }
            else
            {

                if (DESCRIPTION.Text != null  && Convert.ToDecimal(EXPENSEAMOUNT.Text).ToString() != null)
                {
                    Expense expense = new Expense()
                    {

                        
                        Id = Convert.ToInt32(viewModel.Expense.Id),
                        Description= DESCRIPTION.Text,
                        ExpenseAmount = Convert.ToDecimal(EXPENSEAMOUNT.Text),
                        Location = LocationPicker.Items[LocationPicker.SelectedIndex],
                        Contractor = ContractorPicker.Items[ContractorPicker.SelectedIndex],
                        Invoice = InvoicePicker.Items[InvoicePicker.SelectedIndex],
                        Payment = YesOrNo.Text
                    };
                    await database.UpdateAsync(expense);
                    await DisplayAlert("YAY!", "Expense Updated Successfully", "OK");
                    await Navigation.PushModalAsync(new NavigationPage(new AllExpenses()));
                }
                
            }
        }

        private void Paid_Toggled(object sender, ToggledEventArgs e)
        {
            YesOrNo.Text = String.Format("{0}", e.Value);
        }
    }
}
