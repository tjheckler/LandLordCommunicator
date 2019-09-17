using LandlordCommunicator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LandlordCommunicator.ViewModels
{
   public class SelectedInvoiceViewModel
    {
        public Invoice Invoice { get; set; }
        public SelectedInvoiceViewModel(Invoice invoice = null)
        {
            //Title = tenant?.LastName;
            Invoice = invoice;
        }
    }
}
