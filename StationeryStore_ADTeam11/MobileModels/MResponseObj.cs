using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.MobileModels
{
    public class MResponseObj<T> : MResponse
    {
        private T _resObj;
        public T ResObj { get; set; }
    }
}