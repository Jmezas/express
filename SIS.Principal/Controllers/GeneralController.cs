using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIS.Business;
using SIS.Entity;
using SIS.Principal.Models;
namespace SIS.Principal.Controllers
{
    public class GeneralController : Controller
    {

        BGeneral General = new BGeneral();
        Authentication Authentication = new Authentication();
        public ActionResult ListTareas()
        {
            return View();
        }

        [HttpPost]
        public void ListaTareas(string filtro, string FechaIncio, string FechaFin, int numPag, int allReg, int Cant)
        {
            try
            {
              
                Utils.Write(
                    ResponseType.JSON,
                    General.ListaTarea(filtro, FechaIncio, FechaFin, numPag, allReg, Cant)
                );
            }
            catch (Exception Exception)
            {
                Utils.Write(
                    ResponseType.JSON,
                    "{ Code: 1, ErrorMessage: \"" + Exception.Message + "\" }"
                );
            }
        }

    }
}
