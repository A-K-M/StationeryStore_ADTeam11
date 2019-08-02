using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.Models
{
    public class Item
    {

        private string _id;
        private int _categoryId;
        private string _description;
        private int _thresholdValue;
        private int _reorderQty;
        private string _uom;
        private string _binNo;
        private string _firstSupplier;
        private double _firstPrice;
        private string _secondSupplier;
        private double _secondPrice;
        private string _thirdSupplier;
        private double _thirdPrice;

        public string Id { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public int ThresholdValue { get; set; }
        public int ReorderQty { get; set; }
        public string Uom { get; set; }
        public string BinNo { get; set; }
        public string FirstSupplier { get; set; }
        public double FirstPrice { get; set; }
        public string SecondSupplier { get; set; }
        public double SecondPrice { get; set; }
        public string ThirdSupplier { get; set; }
        public double ThirdPrice { get; set; }
 
    }
}