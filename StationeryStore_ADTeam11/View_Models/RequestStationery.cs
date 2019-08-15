using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.View_Models
{
    public class RequestStationery
    {
        private string _itemId;
        private int _quantity;

        public string ItemId { get; set; }
        public int Quantity { get; set; }
    }
}