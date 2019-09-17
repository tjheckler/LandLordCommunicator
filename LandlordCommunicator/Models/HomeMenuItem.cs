using System;
using System.Collections.Generic;
using System.Text;

namespace LandlordCommunicator.Models
{
    public enum MenuItemType
    {
        Home,
        Tenants,
        Locations,
        Items,
        ToDo,
        Reporting,
        Contractors
        
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
