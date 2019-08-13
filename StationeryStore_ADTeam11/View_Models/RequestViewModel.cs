using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.View_Models
{
    public class RequestViewModel
    {
        private string _id;
        private string _employeeName;
        private DateTime _dateTime;

        public string Id { get; set; }
        public string EmployeeName { get; set; }
        public DateTime DateTime { get; set; }
    }
}