using System.Web;
using System.Web.Mvc;

namespace _22686640_Homework_Assignment_3
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
