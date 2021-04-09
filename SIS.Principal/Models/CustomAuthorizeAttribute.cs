using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIS.Principal.Models
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            //if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            //{
            //    filterContext.Result = new RedirectResult("~/Seguridad/Principal");
            //    return;
            //}

            //if (filterContext.Result is HttpUnauthorizedResult)
            //{
            //    filterContext.Result = new RedirectResult("~/Seguridad/AccessDenied");
            //    return;
            //}
            if (HttpContext.Current.Session["Usuario"] == null)
            {
                filterContext.Result = new RedirectResult("~/Seguridad/Login");
                return;
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Seguridad/Principal");
                return;
            }
        }
    }
}