using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using LandlordCommunicator.Models;
using System.IO;
using SQLite;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace LandlordCommunicator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        //path to table if exists or needs created
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Item.db3");

        //loads new page
        public NewItemPage()
        {

            InitializeComponent();

            BindingContext = this;

            StepperCost.ValueChanged += (sender, e) =>
            {
                EntryCost.Text = string.Format("{0}", e.NewValue);
            };

            StepperQuantity.ValueChanged += (sender, e) =>
            {
                EntryQuantity.Text = Convert.ToString(StepperQuantity.Value);
            };
        }


        //Add and validate new Location
        async void Save_Clicked(object sender, EventArgs e)
        {
            var database = new SQLiteConnection(path);

            database.CreateTable<Item>(CreateFlags.ImplicitPK);
            var maxPk = database.Table<Item>().OrderByDescending(L => L.Id).FirstOrDefault();
            //load from page


            if (fileImage == null)
            {
                await DisplayAlert("No Picture!", "Picture is empty", "OK");
            }

            //If Empty, Do not Process
            if ( EntryDescription.Text == null || Convert.ToDecimal(EntryCost.Text).ToString() == null
                || Convert.ToDecimal(EntryCost.Text).ToString() == null || EntryName.Text == null)
            {

                await DisplayAlert(null, "One or More Fields Empty", "OK");
            }
            else
            {
                Item = new Item
                {
                    Image = fileImage.Path.ToString(),
                    Id = (maxPk == null ? 1 : maxPk.Id + 1),
                    Name = EntryName.Text,
                    Description = EntryDescription.Text,
                    Quantity = Convert.ToInt32(EntryQuantity.Text),
                    Cost = Convert.ToDecimal(EntryCost.Text)

                };

                //If not empty, Process
                database.Insert(Item);
                await DisplayAlert(null, "Successfully Added", "OK");
                //go back to list
                await Navigation.PopToRootAsync();
            }

        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            //go back to list
            await Navigation.PopModalAsync();

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