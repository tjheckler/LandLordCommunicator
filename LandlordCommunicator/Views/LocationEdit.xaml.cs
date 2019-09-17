using LandlordCommunicator.Models;
using LandlordCommunicator.Services;
using LandlordCommunicator.ViewModels;
using Plugin.Geolocator;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LandlordCommunicator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationEdit : ContentPage
    {
        public Locations Location { get; set; }
        public Tenant Tenant = new Tenant();
      
        public ObservableCollection<Tenant> Tenants { get; set; }
        //path to table if exists or needs created
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Location.db3");
        string path2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tenant.db3");

        SelectedLocationViewModel viewModel;

        public LocationEdit(SelectedLocationViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
            var database = new SQLiteConnection(path2);

            Tenants = new ObservableCollection<Tenant>();

            var tenants = database.Table<Tenant>().OrderBy(x => x.Id).ToList();

            foreach (Tenant tenant in tenants)
            {
                Tenants.Add(tenant);
            }
            Tenant vacant = new Tenant();
            {
                vacant.FirstName = "Vacant";
                vacant.LastName = "Property";
                vacant.SecondaryFirstName = "Empty";
                vacant.SecondaryLastName = "Building";
                vacant.Image = "/Assets/Icon-72.png";
                vacant.EmailAddress = "N/A";
                vacant.PhoneNumber = "N/A";
                vacant.Id = 100000;
            }
            Tenants.Add(vacant);
            TenantPicker.ItemsSource = Tenants;
        }

        public LocationEdit()
        {
            InitializeComponent();

            BindingContext = viewModel = new SelectedLocationViewModel();
            var database = new SQLiteConnection(path2);

            Tenants = new ObservableCollection<Tenant>();

            var tenants = database.Table<Tenant>().OrderBy(x => x.Id).ToList();

            foreach (Tenant tenant in tenants)
            {
                Tenants.Add(tenant);
            }

            TenantPicker.ItemsSource = Tenants;
            TenantPicker.SelectedItem = TenantPicker.Items[TenantPicker.SelectedIndex].LastOrDefault().ToString();

        }

        Tenant selectedItem;
        public Tenant SelectedItem
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

        private async void GPS_Clicked(object sender, EventArgs e)
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(30));
                LATITUDE.Text = position.Latitude.ToString();
                LONGITUDE.Text = position.Longitude.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
        }

        //update location
        async void Save_Clicked(object sender, EventArgs e)
        {
            var database = new SQLiteAsyncConnection(path);
            //TODO: Fix If tenant does not need to be changed exception problem
            if (TenantPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Must Re-Select Tenant, or Mark Vacant in Edit Mode", "OK");
            }
            else
            {

                if (NAME.Text != null && ADDRESS.Text != null && SUITE.Text != null
                   && CITY.Text != null && ZIPCODE.Text != null &&
                  Convert.ToDouble(LATITUDE.Text).ToString() != null &&
                  Convert.ToDouble(LONGITUDE.Text).ToString() != null &&
                  Convert.ToDecimal(RENTAMOUNT.Text) >= 0 && fileImage != null)
                {
                    Locations location = new Locations()
                    {

                        Image = fileImage.Path.ToString(),
                        Id = Convert.ToInt32(viewModel.Location.Id),
                        Name = NAME.Text,
                        Address = ADDRESS.Text,
                        City = CITY.Text,
                        Suite = SUITE.Text,
                        Latitude = Convert.ToDouble(LATITUDE.Text),
                        Longitude = Convert.ToDouble(LONGITUDE.Text),
                        Zipcode = ZIPCODE.Text,
                        RentAmount = Convert.ToDecimal(RENTAMOUNT.Text),
                        Tenant = TenantPicker.Items[TenantPicker.SelectedIndex]
                    };
                    await database.UpdateAsync(location);
                    await DisplayAlert("YAY!", "Location Updated Successfully", "OK");
                    await Navigation.PushModalAsync(new NavigationPage(new AllLocations()));
                }
                else
                {
                    if (viewModel.Location.Image != null)
                    {
                        Locations location = new Locations()
                        {

                            Image = viewModel.Location.Image,
                            Id = Convert.ToInt32(viewModel.Location.Id),
                            Name = NAME.Text,
                            Address = ADDRESS.Text,
                            City = CITY.Text,
                            Suite = SUITE.Text,
                            Latitude = Convert.ToDouble(LATITUDE.Text),
                            Longitude = Convert.ToDouble(LONGITUDE.Text),
                            Zipcode = ZIPCODE.Text,
                            RentAmount = Convert.ToDecimal(RENTAMOUNT.Text),
                            Tenant = TenantPicker.Items[TenantPicker.SelectedIndex]
                        };
                        await database.UpdateAsync(location);
                        await DisplayAlert("YAY!", "Location Updated Successfully", "OK");
                        await Navigation.PushModalAsync(new NavigationPage(new AllLocations()));
                    }
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