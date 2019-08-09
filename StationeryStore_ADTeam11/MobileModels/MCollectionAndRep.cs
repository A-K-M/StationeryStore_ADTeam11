using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.MobileModels
{
    public class MCollectionAndRep
    {
        private int _pointId;
        private int _repId;
        private string _pointName;
        private string _repName;

        public int PointId { get; set; }
        public string PointName { get; set; }
        public int RepId { get; set; }
        public string RepName { get; set; }
    }
}