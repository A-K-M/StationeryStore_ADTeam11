using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.View_Models
{
    public class AdjustmentVoucherViewModel
    {
        private string _name;
        private int _id;
        private DateTime _date;
        private string _status;
        private int _totalQuantity;

        public string Name { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public int TotalQuantity { get; set; }
    }
}