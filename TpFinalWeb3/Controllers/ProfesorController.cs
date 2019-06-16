using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TpFinalWeb3.Controllers
{
    public class ProfesorController : Controller
    {
        public ActionResult Preguntas()
        {
            MyContext ctx = new MyContext();
            ViewBag.Preguntas = ctx.Pregunta.ToList();
            return View();
        }
        [ActionName("Preguntas/Crear")]
        public ActionResult PreguntasCrear()
        {
            Pregunta pregunta = new Pregunta();
            return View(pregunta);
        }

    }
}