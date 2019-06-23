using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TpFinalWeb3.Models.Servicios;

namespace TpFinalWeb3.Controllers
{
    public class ProfesorController : Controller
    {

        ProfesorServicio profesorServicio = new ProfesorServicio();


        public ActionResult Inicio()
        {
            return RedirectToAction("ProfesorIndex");
        }


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
            int id=(int)Session["idLogueado"];
            profesorServicio.CrearPregunta(p,ListaClases,ListaTemas,id);
            return RedirectToAction("Preguntas");
        }

        public ActionResult EliminarPregunta(int id)
        {
            if(profesorServicio.EliminarPregunta(id) is true)
            {
                return RedirectToAction("Preguntas");
            }
            else
            {
                string mensajeError = "No puede elminar preguntas con respuestas";
                MyContext ctx = new MyContext();
                ViewBag.Preguntas = ctx.Pregunta.ToList();
                ViewBag.MensajeError = mensajeError;
                return View("Preguntas");
            }
        }
<<<<<<< HEAD

        public ActionResult EvaluarPregunta(int id)
        {
            MyContext ctx = new MyContext();
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            ViewBag.respuestasPorId = profesorServicio.BuscarPreguntaEvaluar(id);
            return View(PreguntaPorId);
        }

        public ActionResult EvaluarPreguntaFiltroCorrecta(int id)
        {
            MyContext ctx = new MyContext();
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            ViewBag.respuestasPorId = profesorServicio.BuscarPreguntaEvaluarCorrecta(id);
            return View("EvaluarPregunta",PreguntaPorId);
        }

        public ActionResult EvaluarPreguntaFiltroSinCorreguir(int id)
        {
            MyContext ctx = new MyContext();
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            ViewBag.respuestasPorId = profesorServicio.BuscarPreguntaEvaluarSinCorreguir(id);
            return View("EvaluarPregunta", PreguntaPorId);
        }

        public ActionResult EvaluarPreguntaFiltroRegular(int id)
        {
            MyContext ctx = new MyContext();
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            ViewBag.respuestasPorId = profesorServicio.BuscarPreguntaEvaluarRegular(id);
            return View("EvaluarPregunta", PreguntaPorId);
        }

        public ActionResult EvaluarPreguntaFiltroMal(int id)
        {
            MyContext ctx = new MyContext();
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            ViewBag.respuestasPorId = profesorServicio.BuscarPreguntaEvaluarMal(id);
            return View("EvaluarPregunta", PreguntaPorId);
        }

=======
        public ActionResult ModificarPregunta(int id)
        {
            MyContext ctx = new MyContext();
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            ViewBag.ListaClases = ctx.Clase.ToList();
            ViewBag.ListaTemas = ctx.Tema.ToList();
            return View(PreguntaPorId);
        }

        [HttpPost]
        public ActionResult ModificarPregunta(Pregunta preguntaModificada, int[] ListaClases, int[] ListaTemas)
        {
            int idProfesor = (int)Session["idLogueado"];
            profesorServicio.ModificarPregunta(preguntaModificada, ListaClases, ListaTemas, idProfesor);
            return RedirectToAction("Preguntas");
        }
>>>>>>> refs/remotes/origin/GabiNuevo
    }
}