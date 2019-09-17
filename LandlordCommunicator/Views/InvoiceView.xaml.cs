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
	public partial class InvoiceView : ContentPage
	{
        public Invoice Invoice { get; set; }

        //path to table if exists or needs created
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Invoice.db3");

        SelectedInvoiceViewModel viewModel;

        public InvoiceView(SelectedInvoiceViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public InvoiceView()
        {
            InitializeComponent();

            BindingContext = viewModel = new SelectedInvoiceViewModel();



        }



        //update location
        async void Save_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushModalAsync(new NavigationPage(new AllInvoices()));
        }
    }
}