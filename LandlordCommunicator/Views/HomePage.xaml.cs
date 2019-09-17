using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LandlordCommunicator.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		public HomePage ()
		{
			InitializeComponent ();

            Location.Clicked += async (sender, e) =>
            {
                Location.IsEnabled = false;
                await Navigation.PushAsync(new AllLocations());
                Location.IsEnabled = true;
            };

            Tenant.Clicked += async (sender, e) =>
            {
                Location.IsEnabled = false;
                await Navigation.PushAsync(new AllTenants());
                Location.IsEnabled = true;
            };

            Inventory.Clicked += async (sender, e) =>
            {
                Location.IsEnabled = false;
                await Navigation.PushAsync(new AllItems());
                Location.IsEnabled = true;
            };

            ToDo.Clicked += async (sender, e) =>
            {
                Location.IsEnabled = false;
                await Navigation.PushAsync(new AllToDo());
                Location.IsEnabled = true;
            };

            Reporting.Clicked += async (sender, e) =>
            {
                Location.IsEnabled = false;
                await Navigation.PushAsync(new Reporting());
                Location.IsEnabled = true;
            };

            Contractor.Clicked += async (sender, e) =>
            {
                Location.IsEnabled = false;
                await Navigation.PushAsync(new AllContractors());
                Location.IsEnabled = true;
            };

            Expenses.Clicked += async (sender, e) =>
            {
                Expenses.IsEnabled = false;
                 await Navigation.PushAsync(new AllExpenses());
                Expenses.IsEnabled = true;
            };

            Invoices.Clicked += async (sender, e) =>
            {
                Invoices.IsEnabled = false;
                 await Navigation.PushAsync(new AllInvoices());
                Invoices.IsEnabled = true;
            };
        }

       
    }
}