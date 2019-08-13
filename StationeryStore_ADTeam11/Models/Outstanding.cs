using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.Models
{
    public class Outstanding
    {
        private int _id;
        private int _itemReqId;
        private int _qty;
        private string _status;
        private DateTime _dateTime;
        private int _remainingQty;
        private string reqId;
        private string _itemId;

        public int Id { get; set; }
        public int ItemReqId { get; set; }
        public int Qty { get; set; }
        public string Status { get; set; }
        public DateTime DateTime { get; set; }
        public int RemainingQty { get; set; }
        public string ReqId { get; set; }
        public string ItemId { get; set; }

    }
}