using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.View_Models
{
    public class ReorderStockListVM
    {
        private int _id;
        private DateTime _requestedDate;
        private string _empName;
        private string _status;

        public int Id { get; set; }
        public DateTime RequestedDate { get; set; }
        public string EmpName { get; set; }
        public string Status { get; set; }
    }
}