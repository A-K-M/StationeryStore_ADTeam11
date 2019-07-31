using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.Models
{
    public class Item
    {

        private int _id;
        private int _categoryId;
        private string _description;
        private int _thresholdValue;
        private int _reorderQty;
        private string _uom;
        private string _binNo;
        private string _firstSupplier;
        private decimal _firstPrice;
        private string _secondSupplier;
        private decimal _secondPrice;
        private string _thirdSupplier;
        private decimal _thirdPrice;

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public int ThresholdValue { get; set; }
        public int ReorderQty { get; set; }
        public string Uom { get; set; }
        public string BinNo { get; set; }
        public string FirstSupplier { get; set; }
        public decimal FirstPrice { get; set; }
        public string SecondSupplier { get; set; }
        public decimal SecondPrice { get; set; }
        public string ThirdSupplier { get; set; }
        public decimal ThirdPrice { get; set; }
 
    }
}