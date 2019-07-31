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
        private string _voucherId;


        public int Id { get; set; }

        public string ItemId { get; set; }

        public string VoucherId { get; set; }
    }
}