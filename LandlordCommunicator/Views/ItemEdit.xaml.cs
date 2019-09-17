using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using LandlordCommunicator.Models;
using LandlordCommunicator.ViewModels;
using System.IO;
using SQLite;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace LandlordCommunicator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemEdit : ContentPage
    {
        public Item Item { get; set; }

        //path to table if exists or needs created
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Item.db3");

        ItemDetailViewModel viewModel;

        public ItemEdit(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;

            StepperCost.ValueChanged += (sender, e) =>
            {
                COST.Text = string.Format("{0}", e.NewValue);
            };

            StepperQuantity.ValueChanged += (sender, e) =>
            {
                QUANTITY.Text = Convert.ToString(StepperQuantity.Value);
            };
        }

        public ItemEdit()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemDetailViewModel();

            StepperCost.ValueChanged += (sender, e) =>
            {
                COST.Text = string.Format("{0}",e.NewValue);
            };

            StepperQuantity.ValueChanged += (sender, e) =>
            {
                QUANTITY.Text = Convert.ToString(StepperQuantity.Value);
            };
        }


       //add these changes to tenant and todo edit pages
        //update location
        async void Save_Clicked(object sender, EventArgs e)
        {
            var database = new SQLiteAsyncConnection(path);
            
            if (fileImage != null)
            {
                Item item = new Item()
                {

                    Image = fileImage.Path.ToString(),
                    Id = Convert.ToInt32(viewModel.Item.Id),
                    Name = NAME.Text,
                    Description = DESCRIPTION.Text,
                    Cost = Convert.ToDecimal(COST.Text),
                    Quantity = Convert.ToInt32(QUANTITY.Text)

                };
                await database.UpdateAsync(item);
                await DisplayAlert(null, "Item Updated Successfully", "OK");
                await Navigation.PushModalAsync(new NavigationPage(new AllItems()));
            }
            else
            {
                if (viewModel.Item.Image != null)
                {
                    Item item = new Item()
                    {

                        Image = viewModel.Item.Image,
                        Id = Convert.ToInt32(viewModel.Item.Id),
                        Name = NAME.Text,
                        Description = DESCRIPTION.Text,
                        Cost = Convert.ToDecimal(COST.Text),
                        Quantity = Convert.ToInt32(QUANTITY.Text)

                    };
                    await database.UpdateAsync(item);
                    await DisplayAlert(null, "Item Updated Successfully", "OK");
                    await Navigation.PushModalAsync(new NavigationPage(new AllItems()));
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
