using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.Models
{
    public class StockCard
    {
        private int _id;
        private string _itemId;
        private DateTime _date;
        private int _qty;
        private int _balance;
        private string _refType;

        public int Id { get; set; }
        public string ItemId { get; set; }
        public DateTime Date { get; set; }
        public int Qty { get; set; }
        public int Balance { get; set; }
        public string RefType { get; set; }


    }
}