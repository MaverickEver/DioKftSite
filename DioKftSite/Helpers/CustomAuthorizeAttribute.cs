using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }

    public enum SessionItems
    {
        UserName,
        UserId
    }

}