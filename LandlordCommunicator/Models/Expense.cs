using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace LandlordCommunicator.Models
{
    [Table("Expense")]
    public class Expense
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal ExpenseAmount { get; set; }
        public string Location { get; set; }
        public string Contractor { get; set; }
        public string Payment { get; set; }
        public string Invoice { get; set; }
    }
}
