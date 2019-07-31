using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.Models
{
    public class Retrieval
    {
        private int _id;
        private string _itemId;
        private DateTime _date;
        private string _status;
        private int _employeeId;
        private int _retrievalQty;

        public int Id { get; set; }

        public string ItemId { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; }

        public int EmployeeId { get; set; }

        public int RetrievalQty { get; set; }
    }
}