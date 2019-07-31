using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.Models
{
    public class PuechaserOrderItem
    {
        private int _id;
        private int _purchaseId;
        private string _itemId;
        private string _description;
        private int _qty;
        private double _price;
        private double _amount;

        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public string ItemId { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }
        public double Amount { get; set; }

    }
}