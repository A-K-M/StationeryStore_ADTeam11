using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.Models
{
    public class ItemRequest
    {
        private int _id;
        private int _requestId;
        private string _description;
        private string _itemId;
        private int _neededQty;
        private int _actualQty;
        private string _deptName;


        public int Id { get; set; }
        public int RequestId { get; set; }
        public string Description { get; set; }
        public string ItemId { get; set; }
        public int NeededQty { get; set; }
        public int ActualQty { get; set; }
        public string DeptName { get; set; }

    }
}