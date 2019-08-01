using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.Models
{
    public class AdjustmentVoucher
    {
        private int _id;
        private int _employeeId;
        private string _status;
        private DateTime _date;

        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
    }
}