using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.MobileModels
{
    public class ResponseListAndObj<T> : MResponse
    {
        private List<T> _resList;
        public List<T> ResList { get; set; }
    }
}