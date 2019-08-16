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
        private double _balance;
        private int _reorderQty;
        private int _suggestedReorderQty;
        private string _uom;

        public string Id { get; set; }
        public int CategoryId {get;set;}
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int Threshold { get; set; }
        public double Balance { get; set; }
        public int ReorderQty { get; set; }
        public int SuggestedReorderQty { get; set; }
        public string Uom { get; set; }



    }
}