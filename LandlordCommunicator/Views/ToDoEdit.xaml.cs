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
	public partial class ToDoEdit : ContentPage
	{
        public ToDo ToDo { get; set; }

        //path to table if exists or needs created
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ToDo.db3");

        ToDoDetailViewModel viewModel;

        public ToDoEdit(ToDoDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public ToDoEdit()
        {
            InitializeComponent();

            BindingContext = viewModel = new ToDoDetailViewModel();


        }


        //update location
        async void Save_Clicked(object sender, EventArgs e)
        {
            var database = new SQLiteAsyncConnection(path);

            if (fileImage != null)
            {
                ToDo todo = new ToDo()
                {
                    Image = fileImage.Path.ToString(),
                    Id = Convert.ToInt32(ID.Text),
                    Name = NAME.Text,
                    Description = DESCRIPTION.Text,


                };

                await database.UpdateAsync(todo);
                await DisplayAlert(null, "ToDo Item Updated Successfully", "OK");
                await Navigation.PushModalAsync(new NavigationPage(new AllToDo()));
            }
            else
            {
                if (viewModel.ToDo.Image != null)
                {
                    ToDo todo = new ToDo()
                    {
                        Image = viewModel.ToDo.Image,
                        Id = Convert.ToInt32(ID.Text),
                        Name = NAME.Text,
                        Description = DESCRIPTION.Text,


                    };

                    await database.UpdateAsync(todo);
                    await DisplayAlert(null, "ToDo Item Updated Successfully", "OK");
                    await Navigation.PushModalAsync(new NavigationPage(new AllToDo()));
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