using LandlordCommunicator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LandlordCommunicator.ViewModels
{
    public class SelectedExpenseViewModel
    {
        public Expense Expense { get; set; }
        public SelectedExpenseViewModel(Expense expense = null)
        {
            //Title = tenant?.LastName;
            Expense = expense;
        }
       
    }
}
