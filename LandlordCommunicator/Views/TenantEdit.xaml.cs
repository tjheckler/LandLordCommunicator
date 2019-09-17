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
	public partial class TenantEdit : ContentPage
	{
        public Tenant Tenant { get; set; }

        //path to table if exists or needs created
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tenant.db3");

        SelectedTenantViewModel viewModel;

        public TenantEdit(SelectedTenantViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public TenantEdit()
        {
            InitializeComponent();

            BindingContext = viewModel = new SelectedTenantViewModel();


        }



        //update tenant
        async void Save_Clicked(object sender, EventArgs e)
        {
            var database = new SQLiteAsyncConnection(path);

            if (fileImage != null)
            {
                Tenant tenant = new Tenant()
                {
                    Image = fileImage.Path.ToString(),
                    Id = Convert.ToInt32(ID.Text),
                    FirstName = FIRSTNAME.Text,
                    LastName = LASTNAME.Text,
                    SecondaryFirstName = SECONDARYFIRSTNAME.Text,
                    SecondaryLastName = SECONDARYLASTNAME.Text,
                    PhoneNumber = PHONENUMBER.Text,
                    EmailAddress = EMAILADDRESS.Text
                };

                await database.UpdateAsync(tenant);
                await DisplayAlert(null, "Tenant Updated Successfully", "OK");
                await Navigation.PushModalAsync(new NavigationPage(new AllTenants()));
            }
            else
            {
                if (viewModel.Tenant.Image != null)
                {
                    Tenant tenant = new Tenant()
                    {
                        Image = viewModel.Tenant.Image,
                        Id = Convert.ToInt32(ID.Text),
                        FirstName = FIRSTNAME.Text,
                        LastName = LASTNAME.Text,
                        SecondaryFirstName = SECONDARYFIRSTNAME.Text,
                        SecondaryLastName = SECONDARYLASTNAME.Text,
                        PhoneNumber = PHONENUMBER.Text,
                        EmailAddress = EMAILADDRESS.Text
                    };

                    await database.UpdateAsync(tenant);
                    await DisplayAlert(null, "Tenant Updated Successfully", "OK");
                    await Navigation.PushModalAsync(new NavigationPage(new AllTenants()));
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