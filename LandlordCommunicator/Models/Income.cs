using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace LandlordCommunicator.Models
{
    [Table("Income")]
    public class Income
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Location { get; set; }
        public string Tenant { get; set; }
        public string AmountPaid { get; set; }
        public DateTime Date { get; set; }
    }
}
