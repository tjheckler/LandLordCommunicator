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
	public partial class AllInvoices : ContentPage
	{
        public ObservableCollection<Invoice> Invoices { get; set; }
        
        public AllInvoices()
        {
            InitializeComponent();

            BindingContext = this;

           
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var invoice = args.SelectedItem as Invoice;
            if (invoice == null) return;

        }

        async void EditItem_Clicked(object sender, EventArgs e)
        {
            var invoice = InvoiceListView.SelectedItem as Invoice;
            if (invoice == null)
            {
                await DisplayAlert(null, "You Must Select an Invoice to Edit", "OK");
            }
            else
            {
                await Navigation.PushModalAsync(new InvoiceEdit(new SelectedInvoiceViewModel(invoice)));

                // Manually deselect item.
                InvoiceListView.SelectedItem = null;
            }
        }

        async void ViewItem_Clicked(object sender, EventArgs e)
        {
            var invoice = InvoiceListView.SelectedItem as Invoice;
            if (invoice == null)
            {
                await DisplayAlert(null, "You Must Select an Invoice to View", "OK");
            }
            else
            {
                await Navigation.PushModalAsync(new InvoiceView(new SelectedInvoiceViewModel(invoice)));

                // Manually deselect item.
                InvoiceListView.SelectedItem = null;
            }
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewInvoicePage()));
        }


        async void Delete_Clicked(object sender, EventArgs e)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Invoice.db3");
            var database = new SQLiteAsyncConnection(path);
            //Figure this out
            var invoice = InvoiceListView.SelectedItem;
            if (invoice == null)
            {
                await DisplayAlert(null, "You must select an Invoice to delete", "OK");

            }
            else
            {
                if (await database.Table<Invoice>().CountAsync() == 1)
                {
                    await database.DropTableAsync<Invoice>();
                    await database.CreateTableAsync<Invoice>();
                }
                else
                {
                    await database.DeleteAsync(invoice);
                }
                await DisplayAlert(null, "Invoice Deleted", "OK");
                await Navigation.PushModalAsync(new NavigationPage(new AllInvoices()));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Invoices = new ObservableCollection<Invoice>();


            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Invoice.db3");

            var database = new SQLiteConnection(path);

            database.CreateTable<Invoice>();

            var invoices = database.Table<Invoice>().OrderBy(x => x.Id).ToList();

            InvoiceListView.ItemsSource = invoices;

        }
    }
}