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
        public ActionResult Login(string Email, String Password)
        {
            /*string value = usuario["Profesor"];
            if (value == "True")
            {
                Profesor profesor = new Profesor();
                profesor.Email = usuario["Email"];
                profesor.Password = usuario["Password"];
                if (profesorServicio.VerificarProfesorLogin(profesor) != 0)
                {
                 return RedirectToAction("HomeProfesor");
                }
            }
            if (value == "False")
            {*/
                Alumno alumno = new Alumno();
                alumno.Email = Email;
                alumno.Password = Password;
                if (alumnoServicio.VerificarAlumnoLogin(alumno) != 0)
                {
                    return RedirectToAction("HomeAlumno");
                }
            //}
            ViewBag.MensajeError = "error usuario y/o contraseña";
            return View();
        }
    }
}