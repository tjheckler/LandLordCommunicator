using LandlordCommunicator.Models;
using Plugin.ExternalMaps;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LandlordCommunicator.ViewModels
{
    public class SelectedLocationViewModel :BaseViewModel
    {
        public Locations Location { get; set; }
        public SelectedLocationViewModel(Locations location = null)
        {
            //Title = location?.Name;
            Location = location;
        }

        Command navigateCommand;

        public Command NavigateCommand
        {
            get
            {
                return navigateCommand ?? (navigateCommand = new Command(() =>
                CrossExternalMaps.Current.NavigateTo(Location.Name, Location.Latitude, Location.Longitude)));
            }
        }
    }
}
