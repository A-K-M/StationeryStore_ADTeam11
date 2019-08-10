using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StationeryStore_ADTeam11.Models
{
    public class StockCard
    {
        private int _id;
        private string _itemId;
        private DateTime _date;
        private string _qty;
        private int _balance;
        private string _refType;

        public int Id { get; set; }
        public string ItemId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public string Qty { get; set; }
        public int Balance { get; set; }
        public string RefType { get; set; }


    }
}