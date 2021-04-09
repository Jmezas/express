using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

using System.Web.Mvc;
namespace SIS.Principal.Models
{
    public class SessionFilter: ActionFilterAttribute
    {
        Authentication Authentication = new Authentication();

        public int ViewId { get; set; }

        public override void OnActionExecuting(ActionExecutingContext FilterContext)
        {
            HttpCookie SessionCookie = HttpContext.Current.Request.Cookies["SessionCookie"];
            if (SessionCookie == null)
            {
                FilterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "Controller", "Seguridad" },
                        { "Action", "Login" }
                    }
                );
            }
            else
            {
                if (HttpContext.Current.Session["Usuario"] == null)
                    Authentication.RestartSession();
                if (ViewId != 0)
                {
                    if (!Authentication.IsValidView(ViewId))
                    {
                        FilterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                { "Controller", "Seguridad" },
                                { "Action", "Principal" }
                            }
                        );
                    }
                }
            }
        }
    }
}