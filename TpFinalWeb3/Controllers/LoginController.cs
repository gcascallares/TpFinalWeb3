using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using TpFinalWeb3.Helpers;
using TpFinalWeb3.Models.Servicios;


namespace TpFinalWeb3.Controllers
{
    public class LoginController : Controller
    {
        AlumnoServicio alumnoServicio = new AlumnoServicio();
        ProfesorServicio profesorServicio = new ProfesorServicio();

        // GET: Login
        //[AllowAnonymous]
        public ActionResult Login()
        {
           // ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginServicio login)
        {
            if (!ModelState.IsValid)
            {
                return View();

            }
            else
            {
                if (login.CheckProfesor == 1)
                {
                    if (profesorServicio.VerificarProfesorLogin(login) == 0)
                    {
                        ViewBag.MensajeError = "Email y/o Contraseña inválidos";
                        return View();
                    }
                    else
                    {
                       
                        int idP = profesorServicio.VerificarProfesorLogin(login);
                        Session["id"] = idP;
                        Helpers.SesionHelper.IdUsuario = idP;
                        Helpers.SesionHelper.RolUsuario = "profesor";
                        login.Roles = "profesor";
                        FormsAuthentication.SetAuthCookie(login.Email, false);

                        var authTicket = new FormsAuthenticationTicket(1, login.Email, DateTime.Now, DateTime.Now.AddMinutes(20), false, login.Roles);
                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                        var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        HttpContext.Response.Cookies.Add(authCookie);
                        return RedirectToAction("ProfesorIndex");
                    }
                }
                else
                {
                    if (alumnoServicio.VerificarAlumnoLogin(login) == 0)
                    {
                        ViewBag.MensajeError = "Email y/o Contraseña inválidos";
                        return View();
                    }
                    else
                    {
                        int idA = alumnoServicio.VerificarAlumnoLogin(login);
                        Session["id"] = idA;
                        Helpers.SesionHelper.IdUsuario = idA;
                        Helpers.SesionHelper.RolUsuario = "alumno";
                        login.Roles = "alumno";
                        FormsAuthentication.SetAuthCookie(login.Email, false);

                        var authTicket = new FormsAuthenticationTicket(1, login.Email, DateTime.Now, DateTime.Now.AddMinutes(20), false, login.Roles);
                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                        var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        HttpContext.Response.Cookies.Add(authCookie);
                        return RedirectToAction("AlumnoIndex", new { id = idA });
                    }
                }
            }
        }
        [Authorize(Roles = "alumno")]
        public ActionResult AlumnoIndex(int id)
        {
            //int id = (int)Session["idLogueado"];
            MyContext ctx = new MyContext();
            Alumno alumno = alumnoServicio.buscarAlumnoPorId(id);
            Session["idLogueado"] = alumno.IdAlumno;

            /*List <Pregunta> preguntas = ctx.Pregunta.ToList();
            var p = preguntas.Where(preg => preg.FechaDisponibleHasta < DateTime.Now);
            ViewBag.Preguntas = p;*/


            ViewBag.TodosLosAlumnos = ctx.Alumno.ToList();
            ViewBag.DosPreguntas = alumnoServicio.ultimasDosPreguntas();
            ViewBag.PreguntasSinRespuesta = alumnoServicio.PreguntasSinResponder(alumno.IdAlumno);
            ViewBag.TablaDePosiciones = alumnoServicio.TablaDePosiciones();

            return View(alumno);
        }
        [Authorize(Roles = "profesor")]
        public ActionResult ProfesorIndex()
        {
            int id = Helpers.SesionHelper.IdUsuario;
            Profesor profesor = profesorServicio.buscarProfesorPorId(id);
            Session["idLogueado"] = profesor.IdProfesor;
            return View(profesor);
        }

        public ActionResult CerrarSesion()
        {
            Session["idLogueado"] = "";
            Helpers.SesionHelper.IdUsuario = 0;
            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}