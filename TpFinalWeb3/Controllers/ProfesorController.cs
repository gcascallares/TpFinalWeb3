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
            MyContext ctx = new MyContext();
            foreach (int IdClase in ListaClases)
            {
                Clase c = new Clase();
                c = ctx.Clase.Find(IdClase);
                p.Clase = c;
            }
            foreach (int IdTema in ListaTemas)
            {
                Tema t = new Tema();
                t = ctx.Tema.Find(IdTema);
                p.Tema = t;
            }
            p.IdProfesorCreacion = (int)Session["idLogueado"];
            p.FechaHoraCreacion = DateTime.Now;
            p.Nro = p.Nro;
            p.Pregunta1 = p.Pregunta1;
            ctx.Pregunta.Add(p);
            ctx.SaveChanges();
            return RedirectToAction("Preguntas");
        }

        public ActionResult EliminarPregunta(int id)
        {
            MyContext ctx = new MyContext();
            Pregunta p = ctx.Pregunta.FirstOrDefault(x => x.IdPregunta == id);
            if(p.RespuestaAlumno.Count()==0)
            {
                ctx.Pregunta.Remove(p);
                ctx.SaveChanges();
                return RedirectToAction("Preguntas");
            }
            else
            {
                string mensajeError = "No puede elminar preguntas con respuestas";
                ViewBag.Preguntas = ctx.Pregunta.ToList();
                ViewBag.MensajeError = mensajeError;
                return View("Preguntas");
            }
        }

        public ActionResult EvaluarPregunta(int id)
        {
            MyContext ctx = new MyContext();
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            ViewBag.respuestasPorId = profesorServicio.BuscarPreguntaEvaluar(id);
            return View(PreguntaPorId);
        }

        public ActionResult EvaluarPreguntaCorrecta(int id)
        {
            MyContext ctx = new MyContext();
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            ViewBag.respuestasPorId = profesorServicio.BuscarPreguntaEvaluarCorrecta(id);
            return View("EvaluarPregunta",PreguntaPorId);
        }

        public ActionResult EvaluarPreguntaSinCorreguir(int id)
        {
            MyContext ctx = new MyContext();
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            ViewBag.respuestasPorId = profesorServicio.BuscarPreguntaEvaluarSinCorreguir(id);
            return View("EvaluarPregunta", PreguntaPorId);
        }

        public ActionResult EvaluarPreguntaRegular(int id)
        {
            MyContext ctx = new MyContext();
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            ViewBag.respuestasPorId = profesorServicio.BuscarPreguntaEvaluarRegular(id);
            return View("EvaluarPregunta", PreguntaPorId);
        }

        public ActionResult EvaluarPreguntaMal(int id)
        {
            MyContext ctx = new MyContext();
            Pregunta PreguntaPorId = profesorServicio.BuscarPreguntaPorId(id);
            ViewBag.respuestasPorId = profesorServicio.BuscarPreguntaEvaluarMal(id);
            return View("EvaluarPregunta", PreguntaPorId);
        }


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
            MyContext ctx = new MyContext();
            int idPregunta = preguntaModificada.IdPregunta;
            Pregunta preguntaPorId = profesorServicio.BuscarPreguntaPorId(idPregunta);


            /* foreach (int IdClase in ListaClases)
             {
                 Clase c = new Clase();
                 c = ctx.Clase.Find(IdClase);
                 preguntaModificada.Clase = c;
                 pregunta.Clase = preguntaModificada.Clase;
             }
             foreach (int IdTema in ListaTemas)
             {
                 Tema t = new Tema();
                 t = ctx.Tema.Find(IdTema);
                 preguntaModificada.Tema = t;
                 pregunta.Tema = preguntaModificada.Tema;
             }*/

            preguntaModificada.Nro = preguntaModificada.Nro;
            preguntaModificada.Pregunta1 = preguntaModificada.Pregunta1;
            preguntaModificada.FechaHoraModificacion = DateTime.Now;
            preguntaModificada.FechaDisponibleDesde = preguntaModificada.FechaDisponibleDesde;
            preguntaModificada.FechaDisponibleHasta = preguntaModificada.FechaDisponibleHasta;

            preguntaPorId.Nro = preguntaModificada.Nro;
            preguntaPorId.Pregunta1 = preguntaModificada.Pregunta1;
            preguntaPorId.FechaHoraModificacion = preguntaModificada.FechaHoraModificacion;
            preguntaPorId.FechaDisponibleDesde = preguntaModificada.FechaDisponibleDesde;
            preguntaPorId.FechaDisponibleHasta = preguntaModificada.FechaDisponibleHasta;

            ctx.SaveChanges();

            return RedirectToAction("Preguntas");

            /* MyContext ctx = new MyContext();
             int idPregunta = preguntaModificada.IdPregunta;
             Pregunta pregunta = profesorServicio.BuscarPreguntaPorId(idPregunta);

             //Modificar Nro
             preguntaModificada.Nro = preguntaModificada.Nro;
             int NroPregunta = preguntaModificada.Nro;
             profesorServicio.ModificarNroPregunta(NroPregunta,idPregunta);

             //Modificar Descripcion Pregunta
             preguntaModificada.Pregunta1 = preguntaModificada.Pregunta1;
             string descripcionPregunta = preguntaModificada.Pregunta1;
             profesorServicio.ModificarDescripcionPregunta(descripcionPregunta, idPregunta);

             //Modificar FechaDisponibleDesde 
            // preguntaModificada.FechaDisponibleDesde = preguntaModificada.FechaDisponibleDesde;
             //DateTime fechaDisponibleDesdeNueva = preguntaModificada.FechaDisponibleDesde;
             //profesorServicio.ModificarFechaDesdePregunta(fechaDisponibleDesdeNueva, idPregunta);*/


        }
    }
}