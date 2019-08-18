using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.Util
{
    public class DateUtils
    {
        public static DateTime getLastRequestDate() {
            DateTime resDate = DateTime.Now.AddDays(1);
            while (resDate.DayOfWeek != DayOfWeek.Thursday)
                resDate = resDate.AddDays(-1);
            return resDate;
        }
    }
}