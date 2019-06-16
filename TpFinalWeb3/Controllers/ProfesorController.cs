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
        //[ActionName("Preguntas/Crear")]
        public ActionResult PreguntasCrear()
        {
            Pregunta pregunta = new Pregunta();
            MyContext ctx = new MyContext();
            ViewBag.NroPregunta = (ctx.Pregunta.Count()) + 1;
            ViewBag.ListaClases = ctx.Clase.ToList();
            ViewBag.ListaTemas = ctx.Tema.ToList();
            return View(pregunta);
        }
        [HttpPost]
        //[ActionName("Preguntas/Crear")]
        public ActionResult PreguntasCrear(Pregunta p, int [] ListaClases, int [] ListaTemas)
        {
            MyContext ctx = new MyContext();
            ctx.Pregunta.Add(p);
            foreach (int IdClase in ListaClases)
            {
                Clase c = ctx.Clase.Find(IdClase);
                p.Clase = c;
            }
            foreach (int IdTema in ListaTemas)
            {
                Tema t = ctx.Tema.Find(IdTema);
                p.Tema = t;
            }
            ctx.SaveChanges();
            return RedirectToAction("Preguntas");
        }

    }
}