using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.View_Models
{
    public class RequestReport
    {
        private int _reqYear;
        private int _categoryID;
        private int _qty;
        private int _totalQty;

        public int ReqYear { get; set; }
        public int CategoryID { get; set; }
        public int Qty { get; set; }
        public int TotalQty { get; set; }

    }
}