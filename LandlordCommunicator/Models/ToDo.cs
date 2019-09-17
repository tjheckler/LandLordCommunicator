using SQLite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace LandlordCommunicator.Models
{
    [Table("Todo")]
    public class ToDo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
