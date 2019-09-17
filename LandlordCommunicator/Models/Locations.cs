using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LandlordCommunicator.Models
{
    [Table("Location")]
    public class Locations
    {
       [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Suite { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public decimal RentAmount { get; set; }
        public string Tenant { get; set; }
       

        
        
    }
}
