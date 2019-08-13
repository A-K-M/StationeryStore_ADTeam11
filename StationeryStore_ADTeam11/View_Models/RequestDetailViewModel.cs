using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using StationeryStore_ADTeam11.Models;

namespace StationeryStore_ADTeam11.View_Models
{
    public class RequestDetailViewModel
    {
        private string _itemId;
        private string _itemDespt;
        private int _neededQty;
        private int _actualQty;

        public string ItemId { get; set; }
        public string ItemDescription { get; set; }
        public int NeededQty { get; set; }
        public int ActualQty { get; set; }
    }
}