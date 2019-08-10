using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.MobileModels
{
    public class MAdjustmentItem
    {
        private string _itemId;
        private string _description;
        private int _quantity;
        private string _reason;

        public string ItemId { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string Reason { get; set; }

    }
}