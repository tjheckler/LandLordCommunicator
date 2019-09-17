using LandlordCommunicator.Models;
using LandlordCommunicator.Services;
using LandlordCommunicator.ViewModels;
using Newtonsoft.Json;
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
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LandlordCommunicator.Views
{
  
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewLocationPage : ContentPage
    {
       
        //declare location 
        public Locations Location { get; set; }
        public Tenant Tenant = new Tenant();
        public List<Locations> Locations { get; set; }
        public ObservableCollection<Tenant> Tenants { get; set; }
        AllTenantsViewModel viewModel;
        //path to table if exists or needs created
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Location.db3");
        string path2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tenant.db3");
        //loads new page
        public NewLocationPage()
        {

            InitializeComponent();

            BindingContext = this;
            var database = new SQLiteConnection(path2);

            Tenants = new ObservableCollection<Tenant>();

            var tenants = database.Table<Tenant>().OrderBy(x => x.Id).ToList();
            
            foreach(Tenant tenant in tenants)
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
                catch(Exception ex)
                {
                    DisplayAlert("WHOA!","Well, looks like that is not available at this time", "OK");
                }
            }
        }

        //Add and validate new Location
        async void Save_Clicked(object sender, EventArgs e)
          {
            var database = new SQLiteConnection(path);
            
            database.CreateTable<Location>(CreateFlags.ImplicitPK);
            var maxPk = database.Table<Locations>().OrderByDescending(L => L.Id).FirstOrDefault();
            //load from page
           

            // create database connection
           if (fileImage == null)
            {
                await DisplayAlert("No Picture!", "Picture is empty", "OK");
            }

           if (EntryLatitude == null)
            {
                EntryLatitude.Text = "0";
            }

           if(EntryLongitude == null)
            {
                EntryLongitude.Text = "0";
            }

           if(EntrySuite.Text == null)
            {
                EntrySuite.Text = "N/A";
            }
            //If Empty, Do not Process
            if (EntryName.Text == null || EntryAddress.Text == null  
                || EntryCity.Text == null || EntryZipCode.Text ==null || 
                Convert.ToDecimal(EntryRentAmount.Text) <= 0 )
            {

                await DisplayAlert("Empty Field!", "Address Fields Must Be Complete", "OK");
            }
            else
            {
                Location = new Locations
                {
                    Image = fileImage.Path.ToString(),
                    Id = (maxPk == null ? 1 : maxPk.Id + 1),
                    Name = EntryName.Text,
                    Address = EntryAddress.Text,
                    Suite = EntrySuite.Text,
                    City = EntryCity.Text,
                    Zipcode = EntryZipCode.Text,
                    Latitude = Convert.ToDouble(EntryLatitude.Text),
                    Longitude = Convert.ToDouble(EntryLongitude.Text),
                    RentAmount = Convert.ToDecimal(EntryRentAmount.Text),
                    Tenant = TenantPicker.Items[TenantPicker.SelectedIndex]
                };
                //If not empty, Process
                database.Insert(Location);
                await DisplayAlert("YAY", "Successfully Added "+ Location.Name, "OK");
                //go back to list
                await Navigation.PopToRootAsync();
            }
            
        }

            async void Cancel_Clicked(object sender, EventArgs e)
            {
                //go back to list
                 await Navigation.PopModalAsync();
            
            }

        private async void GPS_Clicked(object sender, EventArgs e)
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(30));
                EntryLatitude.Text = position.Latitude.ToString();
                EntryLongitude.Text = position.Longitude.ToString();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
        }
        private MediaFile fileImage;
        private async void Image_Clicked(object sender, EventArgs e)
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