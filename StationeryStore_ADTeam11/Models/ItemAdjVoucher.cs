using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.Models
{
    public class ItemAdjVoucher
    {
        private int _id;
        private string _itemId;
        private int _voucherId;
        private int _quantity;
        private string _reason;

        public int Id { get; set; }

        public string ItemId { get; set; }

        public int VoucherId { get; set; }

        public int Quantity { get; set; }

        public string Reason { get; set; }
    }
}