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
        private int _qty;
        private int _balance;
        private string _refType;

        public int Id { get; set; }
        public string ItemId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public int Qty { get; set; }
        public int Balance { get; set; }
        [Display(Name = "Dept/Supplier/Reason")]
        public string RefType { get; set; }


    }
}