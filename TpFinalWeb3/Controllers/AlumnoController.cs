using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TpFinalWeb3.Helpers;
using TpFinalWeb3.Models.Servicios;

namespace TpFinalWeb3.Controllers
{
    public class AlumnoController : Controller
    {
        AlumnoServicio alumnoServicio = new AlumnoServicio();
        ProfesorServicio profesorServicio = new ProfesorServicio();
        PreguntaServicio preguntaServicio = new PreguntaServicio();
        public ActionResult VerPreguntasAlumno(int id)
        {
            MyContext ctx = new MyContext();
            //int id = (int)Session["idLogueado"];
            //int id = SesionHelper.IdUsuario;
            Alumno alum = alumnoServicio.buscarAlumnoPorId(id);
            ViewBag.PreguntasSinRespuesta = alumnoServicio.PreguntasSinResponder(id);
            return View(alum);
        }

        public ActionResult ResponderPregunta(int id)
        {
            MyContext ctx = new MyContext();
            Pregunta preg = profesorServicio.BuscarPreguntaPorId(id);
            return View(preg);
        }

        [HttpPost]
        public ActionResult GuardarRespuesta(Pregunta pregunta, string RespuestaAlumno)
        {
            MyContext ctx = new MyContext();
            string respuesta = RespuestaAlumno;       
            int id = pregunta.IdPregunta;
            int idAlumno = (int)Session["idLogueado"];
            //int idAlumno = SesionHelper.IdUsuario;
            Pregunta preg = ctx.Pregunta.Find(id);
            preguntaServicio.GuardarRespuesta(preg, respuesta, idAlumno);

            ctx.SaveChanges();
            return RedirectToAction("/VerPreguntasAlumno/"+idAlumno);
        }

        public ActionResult VerPreguntaFiltroCorrecta(int id)
        {
            MyContext ctx = new MyContext();
            Alumno alum = alumnoServicio.buscarAlumnoPorId(id);
            ViewBag.FiltroRespuesta = preguntaServicio.VerPreguntaEvaluarCorrecta(id);
            return View("VerPreguntaFiltro",alum);
        }

        public ActionResult VerPreguntaFiltroRegular(int id)
        {
            MyContext ctx = new MyContext();
            Alumno alum = alumnoServicio.buscarAlumnoPorId(id);
            ViewBag.FiltroRespuesta = preguntaServicio.VerPreguntaEvaluarRegular(id);
            return View("VerPreguntaFiltro",alum);
        }

        public ActionResult VerPreguntaFiltroMal(int id)
        {
            MyContext ctx = new MyContext();
            Alumno alum = alumnoServicio.buscarAlumnoPorId(id);
            ViewBag.FiltroRespuesta = preguntaServicio.VerPreguntaEvaluarMal(id);
            return View("VerPreguntaFiltro",alum);
        }

        public ActionResult VerPreguntaFiltroSinCorregir(int id)
        {
            MyContext ctx = new MyContext();
            Alumno alum = alumnoServicio.buscarAlumnoPorId(id);
            ViewBag.FiltroRespuesta = preguntaServicio.VerPreguntaSinCorregir(id);
            return View("VerPreguntaFiltro",alum);
        }

       
    }
}