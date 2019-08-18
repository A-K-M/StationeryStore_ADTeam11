using StationeryStore_ADTeam11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace StationeryStore_ADTeam11.View_Models
{
    public class LowStockItemViewModel
    {
        private string _id;
        private int _categoryId;
        private string _categoryName;
        private string _description;
        private int _threshold;
        private int _balance;
        private int _reorderQty;
        private string _suggestedReorderQty;
        private string _uom;
        private Item _itemList;

        public string Id { get; set; }
        public int CategoryId {get;set;}
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int Threshold { get; set; }
        public int Balance { get; set; }
        public int ReorderQty { get; set; }
        public string SuggestedReorderQty { get; set; }
        public string Uom { get; set; }
        public Item ItemList { get; set; }



    }
}