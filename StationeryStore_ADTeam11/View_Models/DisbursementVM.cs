using StationeryStore_ADTeam11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.View_Models
{
    public class DisbursementVM
    {
        private string _deptId;
        private int _collectionPointID;
        private string _collectionPointName;
        private string _deptName;
        private string _repName;
        private string _phone;
        private List<ItemRequest> _itemList = new List<ItemRequest>();

        public string DeptId { get; set; }
        public int CollectionPointID { get; set; }
        public string CollectionPointName { get; set; }
        public string DeptName { get; set; }
        public string RepName { get; set; }
        public string Phone { get; set; }
        public List<ItemRequest> ItemList { get; set; }
        public void AddItem(ItemRequest item) { _itemList.Add(item);  }



    }
}