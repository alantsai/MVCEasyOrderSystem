using MvcEasyOrderSystem.Filters;
using System.Web;
using System.Web.Mvc;

namespace MvcEasyOrderSystem
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            //使得Simplemembership可以使用 WebSecurity、Role等等和使用者登陸系統相關。
            filters.Add(new InitializeSimpleMembershipAttribute());
        }
    }
}