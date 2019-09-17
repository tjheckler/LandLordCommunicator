using LandlordCommunicator.Models;
using LandlordCommunicator.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
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
	public partial class InvoiceEdit : ContentPage
	{
        public Invoice Invoice { get; set; }

        //path to table if exists or needs created
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Invoice.db3");

        SelectedInvoiceViewModel viewModel;

        public InvoiceEdit(SelectedInvoiceViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;

            
        }

        public InvoiceEdit()
        {
            InitializeComponent();

            BindingContext = viewModel = new SelectedInvoiceViewModel();

           
        }


        //add these changes to tenant and todo edit pages
        //update location
        async void Save_Clicked(object sender, EventArgs e)
        {
            var database = new SQLiteAsyncConnection(path);

            if (fileImage != null)
            {
                Invoice item = new Invoice()
                {

                    Image = fileImage.Path.ToString(),
                    Id = Convert.ToInt32(viewModel.Invoice.Id),
                   
                    Description = DESCRIPTION.Text,
                    

                };
                await database.UpdateAsync(item);
                await DisplayAlert(null, "Invoice Updated Successfully", "OK");
                await Navigation.PushModalAsync(new NavigationPage(new AllInvoices()));
            }
            else
            {
                if (viewModel.Invoice.Image != null)
                {
                    Invoice item = new Invoice()
                    {

                        Image = viewModel.Invoice.Image,
                        Id = Convert.ToInt32(viewModel.Invoice.Id),
                        
                        Description = DESCRIPTION.Text,
                       

                    };
                    await database.UpdateAsync(item);
                    await DisplayAlert(null, "Invoice Updated Successfully", "OK");
                    await Navigation.PushModalAsync(new NavigationPage(new AllInvoices()));
                }
            }
        }

        private MediaFile fileImage;
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("No Picture", "Not Supported", "OK");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync();
            if (file == null)
                return;

            fileImage = file;
            Picture.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();



                return stream;


            });
        }


    }
}
