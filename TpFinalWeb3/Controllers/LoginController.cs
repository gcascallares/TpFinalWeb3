using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TpFinalWeb3.Helpers;
using TpFinalWeb3.Models.Servicios;


namespace TpFinalWeb3.Controllers
{
    public class LoginController : Controller
    {
        AlumnoServicio alumnoServicio = new AlumnoServicio();
        ProfesorServicio profesorServicio = new ProfesorServicio();
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginServicio login)
        {
            if (login.CheckProfesor == 1)
            {
                if (profesorServicio.VerificarProfesorLogin(login) is null)
                {
                    ViewBag.MensajeError = "Email y/o Contraseña inválidos";
                    return View();
                }
                else
                {
                    Profesor profesor = new Profesor();
                    profesor = profesorServicio.VerificarProfesorLogin(login);
                    return RedirectToAction("ProfesorIndex", profesor);
                }
            }
            else
            {
                if (alumnoServicio.VerificarAlumnoLogin(login) is null)
                {
                    ViewBag.MensajeError = "Email y/o Contraseña inválidos";
                    return View();
                }
                else
                {
                    Alumno alumno = new Alumno();
                    alumno = alumnoServicio.VerificarAlumnoLogin(login);
                    //int id = alumno.IdAlumno;
                    return RedirectToAction("AlumnoIndex", new { id = alumno.IdAlumno });
                }
            }
        }

        public ActionResult AlumnoIndex(int id)
        {
            //Session["idLogueado"] = alumno.IdAlumno;
            //int id = (int)Session["idLogueado"];
            MyContext ctx = new MyContext();
            AlumnoServicio alumnoServicio = new AlumnoServicio();
            Alumno alumno = alumnoServicio.buscarAlumnoPorId(id);
            
            /*List <Pregunta> preguntas = ctx.Pregunta.ToList();
            var p = preguntas.Where(preg => preg.FechaDisponibleHasta < DateTime.Now);
            ViewBag.Preguntas = p;*/


            ViewBag.TodosLosAlumnos = ctx.Alumno.ToList();
            ViewBag.DosPreguntas = alumnoServicio.ultimasDosPreguntas();
            ViewBag.PreguntasSinRespuesta = alumnoServicio.PreguntasSinResponder(alumno.IdAlumno);
            ViewBag.TablaDePosiciones = alumnoServicio.TablaDePosiciones();

            return View(alumno);
        }

        public ActionResult ProfesorIndex(Profesor profesor)
        {
            Session["idLogueado"] = profesor.IdProfesor;
            return View(profesor);
        }
    }
}