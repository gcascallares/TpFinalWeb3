using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TpFinalWeb3.Helpers;
using TpFinalWeb3.Models.Servicios;

namespace TpFinalWeb3.Controllers
{
    [Authorize(Roles = "alumno")]
    public class AlumnoController : Controller
    {
        AlumnoServicio alumnoServicio = new AlumnoServicio();
        ProfesorServicio profesorServicio = new ProfesorServicio();
        PreguntaServicio preguntaServicio = new PreguntaServicio();
        public ActionResult VerPreguntasAlumno(int id)
        {
            //int id = (int)Session["idLogueado"];
            //int id = SesionHelper.IdUsuario;
            Alumno alum = alumnoServicio.buscarAlumnoPorId(id);
            ViewBag.PreguntasSinRespuesta = preguntaServicio.PreguntasSinResponder(id);

            return View(alum);
        }

        public ActionResult ResponderPregunta(int id)
        {
            Pregunta preg = profesorServicio.BuscarPreguntaPorId(id);
            int idAlumno = (int)Session["idLogueado"];
            Alumno alumno = alumnoServicio.buscarAlumnoPorId(idAlumno);
            ViewData["Alumno"] = alumno;
            return View(preg);
        }

        [ValidateInput(false)]
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
            profesorServicio.EnviarEmailRespuestaAlumno(preg, respuesta, idAlumno);
            ctx.SaveChanges();
            return RedirectToAction("/VerPreguntasAlumno/"+idAlumno);
        }

        public ActionResult VerPreguntaFiltroCorrecta(int id)
        {
            Alumno alum = alumnoServicio.buscarAlumnoPorId(id);
            ViewBag.FiltroRespuesta = preguntaServicio.VerPreguntaEvaluarCorrecta(id);
            return View("VerPreguntaFiltro",alum);
        }

        public ActionResult VerPreguntasFiltroTodas(int id)
        {
            Alumno alum = alumnoServicio.buscarAlumnoPorId(id);
            ViewBag.FiltroRespuestaC = preguntaServicio.VerPreguntaEvaluarCorrecta(id);
            ViewBag.FiltroRespuestaR = preguntaServicio.VerPreguntaEvaluarRegular(id);
            ViewBag.FiltroRespuestaM = preguntaServicio.VerPreguntaEvaluarMal(id);
            ViewBag.FiltroRespuestaS = preguntaServicio.VerPreguntaSinCorregir(id);
            ViewBag.FiltroPreguntasSinResponder = preguntaServicio.PreguntasSinResponder(id);
            return View(alum);
        }

        public ActionResult VerPreguntaFiltroRegular(int id)
        {
            Alumno alum = alumnoServicio.buscarAlumnoPorId(id);
            ViewBag.FiltroRespuesta = preguntaServicio.VerPreguntaEvaluarRegular(id);
            return View("VerPreguntaFiltro",alum);
        }

        public ActionResult VerPreguntaFiltroMal(int id)
        {
            Alumno alum = alumnoServicio.buscarAlumnoPorId(id);
            ViewBag.FiltroRespuesta = preguntaServicio.VerPreguntaEvaluarMal(id);
            return View("VerPreguntaFiltro",alum);
        }

        public ActionResult VerPreguntaFiltroSinCorregir(int id)
        {
            Alumno alum = alumnoServicio.buscarAlumnoPorId(id);
            ViewBag.FiltroRespuesta = preguntaServicio.VerPreguntaSinCorregir(id);
            return View("VerPreguntaFiltro",alum);
        }

        public ActionResult VerRespuesta(int id)
        {
            MyContext ctx = new MyContext();
            RespuestaAlumno respuesta = ctx.RespuestaAlumno.Find(id);
            return View(respuesta);
        }


    }
}