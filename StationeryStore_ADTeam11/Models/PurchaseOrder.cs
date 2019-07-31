using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.Models
{
    public class PurchaseOrder
    {
        private int _id;
        private int _empId;
        private DateTime _date;
        private string _supplierId;
        private DateTime _deliveredDate;

        public int Id { get; set; }
        public int EmpId { get; set; }
        public DateTime Date { get; set; }
        public string SupplierId { get; set; }
        public DateTime DeliveredDate { get; set; }
    }
}