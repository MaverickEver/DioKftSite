using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DioKftSite.Helpers
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {        
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var name = (string)httpContext.Session[SessionItems.UserName.ToString()];
            var id = (string)httpContext.Session[SessionItems.UserId.ToString()];

            return !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(id);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {            
                var route = new RouteValueDictionary(new { controller = "Login", action = "Index" });
                filterContext.Result = new RedirectToRouteResult(route);
            }

        }
    }    


    public enum SessionItems
    {
        UserName,
        UserId
    }

}