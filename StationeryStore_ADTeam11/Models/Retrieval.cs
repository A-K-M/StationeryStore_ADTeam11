using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.Models
{
    public class Retrieval
    {
        private int _id;
        private string _itemId;
        private string _description;
        private string _binNo;
        private DateTime _date;
        private int _retrievalQty;
        private int _qty;

        public int Id { get; set; }

        public string ItemId { get; set; }
        public string Description { get; set; }
        public string BinNo { get; set; }
        public DateTime Date { get; set; }
        public int RetrievalQty { get; set; }
        public int Qty { get; set; }
    }
}