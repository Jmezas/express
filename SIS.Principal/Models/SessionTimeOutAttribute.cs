using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIS.Principal.Models
{
    public class SessionTimeOutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext context = HttpContext.Current;

            if (context.Session != null)
            {
                if (context.Session["Usuario"] == null)
                {
                    context.Response.Redirect("~/Seguridad/Login");
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}