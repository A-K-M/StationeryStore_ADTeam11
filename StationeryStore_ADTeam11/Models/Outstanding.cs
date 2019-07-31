using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.Models
{
    public class Outstanding
    {
        private int _id;
        private int _qty;
        private string _status;
        private DateTime _dateTime;

        public int Id { get; set; }
        public int Qty { get; set; }
        public string Status { get; set; }
        public DateTime DateTime { get; set; }
        
    }
}