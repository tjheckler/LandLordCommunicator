using Plugin.Media.Abstractions;
using SQLite;
using System;
using Xamarin.Forms;

namespace LandlordCommunicator.Models
{
    [Table("Item")]
    public class Item
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public int Quantity { get; set; }
    }
}