using System;

using LandlordCommunicator.Models;

namespace LandlordCommunicator.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = "";
            Item = item;


        }
    }
}
