using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.View_Models
{
    public class DepartmentDelegation
    {
        private int _employeeId;
        private string _employeeName;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _status;
        private string _reason;

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
    }
}