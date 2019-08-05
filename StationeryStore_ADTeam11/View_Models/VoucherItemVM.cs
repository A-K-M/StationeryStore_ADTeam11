using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.View_Models
{
    public class VoucherItemVM
    {
        private string _status;
        private string _itemDescription;
        private int _quantity;
        private string _reason;

        public string Status { get; set; }

        public string ItemDescription { get; set; }

        public int Quantity { get; set; }

        public string Reason { get; set; }
    }
}