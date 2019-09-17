using LandlordCommunicator.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

using Plugin.Messaging;

namespace LandlordCommunicator.ViewModels
{
   public class SelectedTenantViewModel
    {
        public Tenant Tenant { get; set; }
        public SelectedTenantViewModel(Tenant tenant = null)
        {
            //Title = tenant?.LastName;
            Tenant = tenant;
        }
        Command callCommand;
        Command emailCommand;

        public Command CallCommand => callCommand ?? (callCommand = new Command(() => 
        {
        var phoneCallTask = CrossMessaging.Current.PhoneDialer;
            if (phoneCallTask.CanMakePhoneCall)
                phoneCallTask.MakePhoneCall(Tenant.PhoneNumber);
        }));

        public Command EmailCommand => emailCommand ?? (emailCommand = new Command(() =>
        {
            var emailTask = CrossMessaging.Current.EmailMessenger;
            if (emailTask.CanSendEmail)
                emailTask.SendEmail(Tenant.EmailAddress);
        }));
    }
   
}
