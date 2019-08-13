using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StationeryStore_ADTeam11.Models;

namespace StationeryStore_ADTeam11.View_Models
{
    public class RetrievalList
    {
        private string _itemId;
        private int _total;
        private List<Retrieval> _retrievalList;
        private List<Outstanding> _outstandingList;
        private List<ItemRequest> _itemReqList;

        public string ItemId { get; set; }
        public int Total { get; set; }
        public List<Retrieval> retrievals { get; set; }
        public List<Outstanding> outstandings { get; set; }
        public List<ItemRequest> ItemReqList { get; set; }
    }
}