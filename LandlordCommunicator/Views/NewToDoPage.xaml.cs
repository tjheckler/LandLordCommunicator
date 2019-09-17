using LandlordCommunicator.Models;
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
	public partial class NewToDoPage : ContentPage
	{
        public ToDo ToDo { get; set; }

        //path to table if exists or needs created
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ToDo.db3");

        //loads new page
        public NewToDoPage()
        {

            InitializeComponent();

            BindingContext = this;

        }


        //Add and validate new Location
        async void Save_Clicked(object sender, EventArgs e)
        {
            var database = new SQLiteConnection(path);

            database.CreateTable<ToDo>(CreateFlags.ImplicitPK);
            var maxPk = database.Table<ToDo>().OrderByDescending(L => L.Id).FirstOrDefault();
            if (fileImage == null)
            {
                await DisplayAlert("No Picture!", "Picture is empty", "OK");
            }

            //If Empty, Do not Process
            if (EntryName.Text == null || EntryDescription == null )
            {

                await DisplayAlert(null, "One or More Fields Empty", "OK");
            }
            else
            {
                ToDo = new ToDo
                {
                    Image = fileImage.Path.ToString(),
                    Id = (maxPk == null ? 1 : maxPk.Id + 1),
                    Name = EntryName.Text,
                    Description = EntryDescription.Text,


                };
                //If not empty, Process
                database.Insert(ToDo);
                await DisplayAlert(null, "Successfully Added", "OK");
                //go back to list
                await Navigation.PopModalAsync();
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