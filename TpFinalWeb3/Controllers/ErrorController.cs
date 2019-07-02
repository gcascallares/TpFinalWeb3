using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TpFinalWeb3.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Error(int error = 0)
        {
            switch (error)
            {
                case 505:
                    ViewBag.Title2 = "Ocurrio un error inesperado";
                    ViewBag.Description = "Lamentamos lo sucedido, no volvera a pasar :)";
                    break;

                case 404:
                    ViewBag.Title2 = "Página no encontrada";
                    ViewBag.Description = "La URL que está intentando ingresar no existe D:";
                    break;

                default:
                    ViewBag.Title2 = "Página no encontrada";
                    ViewBag.Description = "Algo salio muy mal :(";
                    break;
            }

            return View();
        }
    }
}