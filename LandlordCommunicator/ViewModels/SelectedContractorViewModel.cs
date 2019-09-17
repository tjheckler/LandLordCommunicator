using LandlordCommunicator.Models;
using Plugin.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LandlordCommunicator.ViewModels
{
   public class SelectedContractorViewModel
    {
        public Contractor Contractor { get; set; }
        public SelectedContractorViewModel(Contractor contractor = null)
        {
            
            Contractor = contractor;
        }
        Command callCommand;
        

        public Command CallCommand => callCommand ?? (callCommand = new Command(() =>
        {
            var phoneCallTask = CrossMessaging.Current.PhoneDialer;
            if (phoneCallTask.CanMakePhoneCall)
                phoneCallTask.MakePhoneCall(Contractor.PhoneNumber);
        }));

       
    }
}
