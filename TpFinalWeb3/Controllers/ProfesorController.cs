using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TpFinalWeb3.Helpers;
using TpFinalWeb3.Models.Servicios;

namespace TpFinalWeb3.Controllers
{
    public class ProfesorController : Controller
    {
        PreguntaServicio preguntaServicio = new PreguntaServicio();
        ProfesorServicio profesorServicio = new ProfesorServicio();
        AlumnoServicio alumnoServicio = new AlumnoServicio();

        public ActionResult Inicio()
        {
            int id = (int)Session["idLogueado"];
            return RedirectToAction("ProfesorIndex");
        }


        public ActionResult Preguntas(int pagina = 1)
        {
            Paginador<Pregunta> paginador = preguntaServicio.Preguntas(pagina);
            return View(paginador);
        }



        //[ActionName("Preguntas/Crear")]
        public ActionResult PreguntasCrear()
        {
            Pregunta pregunta = new Pregunta();
            MyContext ctx = new MyContext();
            ViewBag.NroPregunta = (ctx.Pregunta.Count()) + 1;
            ViewBag.ListaClases = ctx.Clase.ToList();
            ViewBag.ListaTemas = ctx.Tema.ToList();
            return View (pregunta);
        }
        [HttpPost]
        public ActionResult PreguntasCrear(Pregunta p, int [] ListaClases, int [] ListaTemas)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Login");

            }
            else
            {
                int id = (int)Session["idLogueado"];
                profesorServicio.CrearPregunta(p, ListaClases, ListaTemas, id);
                return RedirectToAction("Preguntas");
            }
        }

        public ActionResult EliminarPregunta(int id)
        {
            if(profesorServicio.EliminarPregunta(id) is true)
            {
                return RedirectToAction("Preguntas");
            }
            else
            {
                MyContext ctx = new MyContext();
                int pagina = 1;
                ViewBag.MensajeError = "No puede elminar preguntas con respuestas";
                Paginador<Pregunta> paginador = preguntaServicio.Preguntas(pagina);
                return View("Preguntas", paginador);
            }
        }

        public ActionResult EvaluarPregunta(int id)
        {
            MyContext ctx = new MyContext();
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            ViewBag.respuestasPorId = profesorServicio.BuscarPreguntaEvaluar(id);
            ViewData["TodasCorregidas"] = profesorServicio.TotalCorregidas(id);
            ViewData["MejorRespuesta"] = profesorServicio.MejorRespuesta(id);
            return View(PreguntaPorId);
        }

        public ActionResult EvaluarPreguntaFiltroCorrecta(int id)
        {
            MyContext ctx = new MyContext();
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            ViewBag.respuestasPorId = profesorServicio.BuscarPreguntaEvaluarCorrecta(id);
            ViewData["TodasCorregidas"] = profesorServicio.TotalCorregidas(id);
            ViewData["MejorRespuesta"] = profesorServicio.MejorRespuesta(id);
            return View("EvaluarPregunta",PreguntaPorId);
        }

        public ActionResult EvaluarPreguntaFiltroSinCorreguir(int id)
        {
            MyContext ctx = new MyContext();
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            ViewBag.respuestasPorId = profesorServicio.BuscarPreguntaEvaluarSinCorreguir(id);
            ViewData["TodasCorregidas"] = profesorServicio.TotalCorregidas(id);
            ViewData["MejorRespuesta"] = profesorServicio.MejorRespuesta(id);
            return View("EvaluarPregunta", PreguntaPorId);
        }

        public ActionResult EvaluarPreguntaFiltroRegular(int id)
        {
            MyContext ctx = new MyContext();
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            ViewBag.respuestasPorId = profesorServicio.BuscarPreguntaEvaluarRegular(id);
            ViewData["TodasCorregidas"] = profesorServicio.TotalCorregidas(id);
            ViewData["MejorRespuesta"] = profesorServicio.MejorRespuesta(id);
            return View("EvaluarPregunta", PreguntaPorId);
        }

        public ActionResult EvaluarPreguntaFiltroMal(int id)
        {
            MyContext ctx = new MyContext();
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            ViewBag.respuestasPorId = profesorServicio.BuscarPreguntaEvaluarMal(id);
            ViewData["TodasCorregidas"] = profesorServicio.TotalCorregidas(id);
            ViewData["MejorRespuesta"] = profesorServicio.MejorRespuesta(id);
            return View("EvaluarPregunta", PreguntaPorId);
        }


        public ActionResult MejorRespuesta(int id, int idDos)
        {
            MyContext ctx = new MyContext();
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            RespuestaAlumno respuestaPorId = profesorServicio.BuscarRespuestaPorId(idDos);
            profesorServicio.ActivarMejorRespuesta(respuestaPorId);
            alumnoServicio.EnviarEmailMejorRespuesta(respuestaPorId);
            return RedirectToAction("EvaluarPregunta", new { id = PreguntaPorId.IdPregunta } );
        }

        public ActionResult ModificarPregunta(int id)
        {
            MyContext ctx = new MyContext();
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            ViewBag.ListaClases = ctx.Clase.ToList();
            ViewBag.ListaTemas = ctx.Tema.ToList();
            if (profesorServicio.VerificarRespuestas(id) == false)
            {
                ViewBag.AvisoRespuestas = "Ya se recibieron respuestas a esta pregunta, evite hacer modificaciones que puedan repercutir en las respuestas recibidas";
            }
            return View(PreguntaPorId);
        }

        [HttpPost]
        public ActionResult ModificarPregunta(Pregunta preguntaModificada, int[] ListaClases, int[] ListaTemas)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Login");

            }
            else
            {
                int idProfesor = (int)Session["idLogueado"];
                profesorServicio.ModificarPregunta(preguntaModificada, ListaClases, ListaTemas, idProfesor);
                return RedirectToAction("Preguntas");
            }
        }

        public ActionResult EvaluarRespuestaCorrecta(int id, int idDos)
        {
            MyContext ctx = new MyContext();
            int idProfesor = (int)Session["idLogueado"];
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            RespuestaAlumno respuestaPorId = profesorServicio.BuscarRespuestaPorId(idDos);
            alumnoServicio.EvaluarPreguntaCorrecta(idProfesor,respuestaPorId);
            alumnoServicio.EnviarEmailEvaluacion(idProfesor, respuestaPorId);
            return RedirectToAction("EvaluarPregunta", new { id = PreguntaPorId.IdPregunta });
        }

        public ActionResult EvaluarRespuestaRegular(int id, int idDos)
        {
            MyContext ctx = new MyContext();
            int idProfesor = (int)Session["idLogueado"];
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            RespuestaAlumno respuestaPorId = profesorServicio.BuscarRespuestaPorId(idDos);
            alumnoServicio.EvaluarPreguntaRegular(idProfesor, respuestaPorId);
            alumnoServicio.EnviarEmailEvaluacion(idProfesor, respuestaPorId);
            return RedirectToAction("EvaluarPregunta", new { id = PreguntaPorId.IdPregunta });
        }

        public ActionResult EvaluarRespuestaMal(int id, int idDos)
        {
            MyContext ctx = new MyContext();
            int idProfesor = (int)Session["idLogueado"];
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            RespuestaAlumno respuestaPorId = profesorServicio.BuscarRespuestaPorId(idDos);
            alumnoServicio.EvaluarPreguntaMal(idProfesor, respuestaPorId);
            alumnoServicio.EnviarEmailEvaluacion(idProfesor, respuestaPorId);
            return RedirectToAction("EvaluarPregunta", new { id = PreguntaPorId.IdPregunta });
        }
    }
}