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
        public ActionResult Login(string Email, string Password)
        {
            
            if (Request.Form["Profesor"] != null)
            {
                Profesor profesor = new Profesor();
                profesor.Email = Email;
                profesor.Password = Password;
                if (profesorServicio.VerificarProfesorLogin(profesor) != 0)
                {
                     return RedirectToAction("HomeProfesor");
                }
                else
                {
                    ViewBag.MensajeError = "error usuario y/o contraseña";
                    return View();
                }
            }
            else if (Request.Form["Profesor"] == null)
            {
                Alumno alumno = new Alumno();
                alumno.Email = Email;
                alumno.Password = Password;
                if (alumnoServicio.VerificarAlumnoLogin(alumno) > 0)
                {
                    return RedirectToAction("AlumnoIndex");
                }
                else
                {
                    ViewBag.MensajeError = "error usuario y/o contraseña";
                    return View();
                }
            }
            else
            {
                ViewBag.MensajeError = "error usuario y/o contraseña";
                return View();
            }
        }

        public ActionResult AlumnoIndex()
        {
            return View();
        }

        public ActionResult ProfesorIndex()
        {
            return View();
        }
    }
}