using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.MobileModels
{
    public class MResponseListAndObj<ListType1,ObjType> : MResponseObj<ObjType>
    {
        private List<ListType1> _resList;
        public List<ListType1> ResList { get; set; }
    }
}