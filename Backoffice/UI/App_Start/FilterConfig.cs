using System.Web;
using System.Web.Mvc;
using UI.Common;

namespace UI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RequreSecureConnectionFilter());
        }
    }
}
