using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.View_Models
{
    public class VoucherItemVM
    {
        private int _voucherId;
        private string _status;
        private string _itemDescription;
        private int _quantity;
        private double price;
        private string _reason;

        public int VoucherID { get; set; }

        public string Status { get; set; }

        public double Price { get; set; }

        public string ItemDescription { get; set; }

        public int Quantity { get; set; }

        public string Reason { get; set; }
    }
}