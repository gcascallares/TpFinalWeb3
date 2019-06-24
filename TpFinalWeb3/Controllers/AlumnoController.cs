using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TpFinalWeb3.Models.Servicios;

namespace TpFinalWeb3.Controllers
{
    public class AlumnoController : Controller
    {
        public ActionResult VerPreguntasAlumno()
        {
            MyContext ctx = new MyContext();
            AlumnoServicio alumnoServicio = new AlumnoServicio();
            int id = (int)Session["idLogueado"];
            ViewBag.PreguntasSinRespuesta = alumnoServicio.PreguntasSinResponder(id);
            return View();
        }

        public ActionResult ResponderPregunta(int id)
        {
            MyContext ctx = new MyContext();
            ProfesorServicio profesorServicio = new ProfesorServicio();
            Pregunta preg = profesorServicio.BuscarPreguntaPorId(id);
            return View(preg);
        }

        [HttpPost]
        public ActionResult GuardarRespuesta(Pregunta pregunta, string RespuestaAlumno)
        {
            MyContext ctx = new MyContext();
            string respuesta = RespuestaAlumno;
            ProfesorServicio profesorServicio = new ProfesorServicio();
            PreguntaServicio preguntaServicio = new PreguntaServicio();
            int id = pregunta.IdPregunta;
            int idAlumno = (int)Session["idLogueado"];
            Pregunta preg = profesorServicio.BuscarPreguntaPorId(id);
            preguntaServicio.GuardarRespuesta(preg, respuesta, idAlumno);

            ctx.SaveChanges();
            return RedirectToAction("/VerPreguntasAlumno");
        }

    }
}