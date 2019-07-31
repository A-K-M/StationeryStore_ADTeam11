using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.Models
{
    public class ItemSupplier
    {
        private int _id;
        private string _itemId;
        private string _supplierId;


        public int Id { get; set; }

        public string ItemId { get; set; }

        public string SupplierId { get; set; }
    }
}