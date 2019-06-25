﻿using System;
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
                if (profesorServicio.VerificarProfesorLogin(login) == 0)
                {
                    ViewBag.MensajeError = "Email y/o Contraseña inválidos";
                    return View();
                }
                else
                {
                    int idP = profesorServicio.VerificarProfesorLogin(login);
                    Helpers.SesionHelper.IdUsuario = idP;
                    return RedirectToAction("ProfesorIndex");
                }
            }
            else
            {
                if (alumnoServicio.VerificarAlumnoLogin(login)==0)
                {
                    ViewBag.MensajeError = "Email y/o Contraseña inválidos";
                    return View();
                }
                else
                {
                    int idA = alumnoServicio.VerificarAlumnoLogin(login);
                    Helpers.SesionHelper.IdUsuario = idA;
                    return RedirectToAction("AlumnoIndex", new { id = idA });
                }
            }
        }

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

        public ActionResult ProfesorIndex()
        {
            int id = Helpers.SesionHelper.IdUsuario;
            Profesor profesor = profesorServicio.buscarProfesorPorId(id);
            Session["idLogueado"] = profesor.IdProfesor;
            return View(profesor);
        }

    }
}