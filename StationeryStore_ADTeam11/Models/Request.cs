using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.Models
{
    public class Request
    {
        private string _id;
        private int _employeeID;
        private DateTime _dateTime;
        private string _status;
        private DateTime _disbursedDate;

        public string Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime DateTime { get; set; }
        public string Status { get; set; }
        public DateTime DisbursedDate { get; set; }

    }
}