using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.MobileModels
{
    public class MResponseTwoObject<T, Y> : MResponse
    {
        private T _resObj;
        public T ResObj { get; set; }
        private Y _resObj2;
        public Y ResObj2 { get; set; }
    }
}