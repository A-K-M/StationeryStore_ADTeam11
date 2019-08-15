using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.MobileModels
{
    public class MRequestItem
    {
        private int _requestId;
        private int _quantity;
        private string _description;

        public int RequestId { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
    }
}