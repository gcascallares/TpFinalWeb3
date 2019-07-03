using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TpFinalWeb3.Helpers;
using TpFinalWeb3.Models.Servicios;

namespace TpFinalWeb3.Controllers
{
    [Authorize(Roles ="profesor")]
    public class ProfesorController : Controller
    {
        MyContext ctx = new MyContext();
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



        public ActionResult PreguntasCrear()
        {
            Pregunta pregunta = new Pregunta();
            ViewBag.NroPregunta = (ctx.Pregunta.Count()) + 1;
            ViewBag.ListaClases = ctx.Clase.ToList();
            ViewBag.ListaTemas = ctx.Tema.ToList();
            return View (pregunta);
        }
        [HttpPost]
        public ActionResult PreguntasCrear(Pregunta p, int [] ListaClases, int [] ListaTemas)
        {
            if(profesorServicio.VerificarNroPregunta(p.Nro) == false)
            {
                ViewBag.ErrorNro = "Ya existe una pregunta con este número de Pregunta";
                ViewBag.NroPregunta = (ctx.Pregunta.Count()) + 1;
                ViewBag.ListaClases = ctx.Clase.ToList();
                ViewBag.ListaTemas = ctx.Tema.ToList();
                return View("PreguntasCrear", p);
            }
            else if (p.FechaDisponibleHasta<p.FechaDisponibleDesde)
            {
                ViewBag.ErrorFechas = "La fecha disponible desde debe ser superior a la fecha hasta";
                ViewBag.NroPregunta = (ctx.Pregunta.Count()) + 1;
                ViewBag.ListaClases = ctx.Clase.ToList();
                ViewBag.ListaTemas = ctx.Tema.ToList();
                return View("PreguntasCrear",p);
            }
            else
            {
                int id = Helpers.SesionHelper.IdUsuario;
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
                int pagina = 1;
                ViewBag.MensajeError = "No puede elminar preguntas con respuestas";
                Paginador<Pregunta> paginador = preguntaServicio.Preguntas(pagina);
                return View("Preguntas", paginador);
            }
        }

        public ActionResult EvaluarPregunta(int id)
        {
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            ViewBag.respuestasPorId = profesorServicio.BuscarPreguntaEvaluar(id);
            ViewData["TodasCorregidas"] = profesorServicio.TotalCorregidas(id);
            ViewData["MejorRespuesta"] = profesorServicio.MejorRespuesta(id);
            return View(PreguntaPorId);
        }

        public ActionResult EvaluarPreguntaFiltroCorrecta(int id)
        {
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            ViewBag.respuestasPorId = profesorServicio.BuscarPreguntaEvaluarCorrecta(id);
            ViewData["TodasCorregidas"] = profesorServicio.TotalCorregidas(id);
            ViewData["MejorRespuesta"] = profesorServicio.MejorRespuesta(id);
            return View("EvaluarPregunta",PreguntaPorId);
        }

        public ActionResult EvaluarPreguntaFiltroSinCorreguir(int id)
        {
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            ViewBag.respuestasPorId = profesorServicio.BuscarPreguntaEvaluarSinCorreguir(id);
            ViewData["TodasCorregidas"] = profesorServicio.TotalCorregidas(id);
            ViewData["MejorRespuesta"] = profesorServicio.MejorRespuesta(id);
            return View("EvaluarPregunta", PreguntaPorId);
        }

        public ActionResult EvaluarPreguntaFiltroRegular(int id)
        {
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            ViewBag.respuestasPorId = profesorServicio.BuscarPreguntaEvaluarRegular(id);
            ViewData["TodasCorregidas"] = profesorServicio.TotalCorregidas(id);
            ViewData["MejorRespuesta"] = profesorServicio.MejorRespuesta(id);
            return View("EvaluarPregunta", PreguntaPorId);
        }

        public ActionResult EvaluarPreguntaFiltroMal(int id)
        {
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            ViewBag.respuestasPorId = profesorServicio.BuscarPreguntaEvaluarMal(id);
            ViewData["TodasCorregidas"] = profesorServicio.TotalCorregidas(id);
            ViewData["MejorRespuesta"] = profesorServicio.MejorRespuesta(id);
            return View("EvaluarPregunta", PreguntaPorId);
        }


        public ActionResult MejorRespuesta(int id, int idDos)
        {
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            RespuestaAlumno respuestaPorId = profesorServicio.BuscarRespuestaPorId(idDos);
            profesorServicio.ActivarMejorRespuesta(respuestaPorId);
            alumnoServicio.EnviarEmailMejorRespuesta(respuestaPorId);
            return RedirectToAction("EvaluarPregunta", new { id = PreguntaPorId.IdPregunta } );
        }

        public ActionResult ModificarPregunta(int id)
        {
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
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(preguntaModificada.IdPregunta);
            if (preguntaModificada.Nro != PreguntaPorId.Nro || profesorServicio.VerificarNroPregunta(preguntaModificada.Nro))
            {
                    ViewBag.ErrorNro = "Ya existe una pregunta con este número de Pregunta";
                    ViewBag.NroPregunta = (ctx.Pregunta.Count()) + 1;
                    ViewBag.ListaClases = ctx.Clase.ToList();
                    ViewBag.ListaTemas = ctx.Tema.ToList();
                    return View("ModificarPregunta", PreguntaPorId);
            }
            else if (preguntaModificada.FechaDisponibleHasta < preguntaModificada.FechaDisponibleDesde)
            {
                ViewBag.ErrorFechas = "La fecha disponible desde debe ser superior a la fecha hasta";
                ViewBag.NroPregunta = (ctx.Pregunta.Count()) + 1;
                ViewBag.ListaClases = ctx.Clase.ToList();
                ViewBag.ListaTemas = ctx.Tema.ToList();
                return View("ModificarPregunta", PreguntaPorId);
            }
            else
            {
                int idProfesor = Helpers.SesionHelper.IdUsuario;
                profesorServicio.ModificarPregunta(preguntaModificada, ListaClases, ListaTemas, idProfesor);
                return RedirectToAction("Preguntas");
            }
        }

        public ActionResult EvaluarRespuestaCorrecta(int id, int idDos)
        {
            int idProfesor = (int)Session["idLogueado"];
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            RespuestaAlumno respuestaPorId = profesorServicio.BuscarRespuestaPorId(idDos);
            alumnoServicio.EvaluarPreguntaCorrecta(idProfesor,respuestaPorId);
            alumnoServicio.EnviarEmailEvaluacion(idProfesor, respuestaPorId);
            return RedirectToAction("EvaluarPregunta", new { id = PreguntaPorId.IdPregunta });
        }

        public ActionResult EvaluarRespuestaRegular(int id, int idDos)
        {
            int idProfesor = (int)Session["idLogueado"];
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            RespuestaAlumno respuestaPorId = profesorServicio.BuscarRespuestaPorId(idDos);
            alumnoServicio.EvaluarPreguntaRegular(idProfesor, respuestaPorId);
            alumnoServicio.EnviarEmailEvaluacion(idProfesor, respuestaPorId);
            return RedirectToAction("EvaluarPregunta", new { id = PreguntaPorId.IdPregunta });
        }

        public ActionResult EvaluarRespuestaMal(int id, int idDos)
        {
            int idProfesor = (int)Session["idLogueado"];
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            RespuestaAlumno respuestaPorId = profesorServicio.BuscarRespuestaPorId(idDos);
            alumnoServicio.EvaluarPreguntaMal(idProfesor, respuestaPorId);
            alumnoServicio.EnviarEmailEvaluacion(idProfesor, respuestaPorId);
            return RedirectToAction("EvaluarPregunta", new { id = PreguntaPorId.IdPregunta });
        }

        [ActionName("AcercaDe")]
        public ActionResult AcercaDe()
        {
            int id = Helpers.SesionHelper.IdUsuario;
            Profesor prof = profesorServicio.buscarProfesorPorId(id);
            ViewBag.Layout = "/Views/Shared/_ProfesoresLayout.cshtml";
            ViewBag.Objeto = "@model TpFinalWeb3.Profesor";
            ViewBag.IdProfesor = id;
            return View("/Views/Alumno/AcercaDe.cshtml",prof);

        }

    }
}