﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.Models
{
    public class Delegation
    {
        private int _id;
        private int _employeeId;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _employeeName = "Hnin";
        private bool _status;
        private string _reason;

        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EmployeeName { get; set; }
        public bool Status { get; set; }
        public string Reason { get; set; }
       
    }
}