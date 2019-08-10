using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.MobileModels
{
    public class MResponseList<T> : MResponse
    {
        private List<T> _resList;
        public List<T> ResList {
            get {
                return _resList;
            }
            set {
                _resList = value;
                Success = (_resList != null);
            }
        }
    }
}