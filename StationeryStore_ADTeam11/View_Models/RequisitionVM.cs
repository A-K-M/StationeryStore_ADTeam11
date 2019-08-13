using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.View_Models
{
    public class RequisitionVM
    {
        private string _id;
        private string _employeeName;
        private DateTime _date;
        private int _quantity;
        private string _status;

        public string Id { get; set; }
        public string EmployeeName { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }
}