using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
                    return RedirectToAction("AlumnoIndex", alumno);
                }
            }
        }

        public ActionResult AlumnoIndex(Alumno alumno)
        {
            Session["idLogueado"] = alumno.IdAlumno;
            MyContext ctx = new MyContext();
            /*List <Pregunta> preguntas = ctx.Pregunta.ToList();
            var p = preguntas.Where(preg => preg.FechaDisponibleHasta < DateTime.Now);
            ViewBag.Preguntas = p;*/

            ViewBag.TodosLosAlumnos = ctx.Alumno.ToList();
            ViewBag.DosPreguntas = alumnoServicio.ultimasDosPreguntas();

            return View(alumno);
        }

        public ActionResult ProfesorIndex(Profesor profesor)
        {
            return View(profesor);
        }
    }
}