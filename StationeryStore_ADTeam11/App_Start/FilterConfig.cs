using System.Web;
using System.Web.Mvc;

namespace StationeryStore_ADTeam11
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
