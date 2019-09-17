using LandlordCommunicator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LandlordCommunicator.ViewModels
{
   public class ToDoDetailViewModel : BaseViewModel
    {
        public ToDo ToDo { get; set; }
        public ToDoDetailViewModel(ToDo todo = null)
        {
            //Title = location?.Name;
            ToDo = todo;
        }
    }
}
