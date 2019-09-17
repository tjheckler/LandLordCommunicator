using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace LandlordCommunicator.Models
{
    [Table("Contractor")]
    public class Contractor
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string ContractorType { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
    }
}
