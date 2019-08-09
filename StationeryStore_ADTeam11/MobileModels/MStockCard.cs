using StationeryStore_ADTeam11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.MobileModels
{
    public class MStockCard
    {
        private string _itemCode;
        private string _description;
        private string _bin;
        private List<StockCard> _dataList;
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string Bin { get; set; }
        public List<StockCard> DataList { get;set;}
    }
}