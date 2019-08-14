using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.View_Models
{
    public class StationeryRequest
    {
        private int _requestid;
        private string _empName;
        private DateTime _date;
        private int _totalItem;
        private string _status;
        private string _description;

        public int RequestId { get; set; }
        public string EmpName { get; set; }
        public DateTime Date { get; set; }
        public int TotalItem { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }
}