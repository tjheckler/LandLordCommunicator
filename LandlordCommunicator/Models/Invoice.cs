using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace LandlordCommunicator.Models
{
    [Table("Invoice")]
    public class Invoice
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
       
    }
}
