using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace TpFinalWeb3.Models.Servicios
{
    public class AlumnoServicio
    {
        MyContext ctx = new MyContext();
        public int VerificarAlumnoLogin(LoginServicio buscado)
        {
            Alumno alumnoDb = ctx.Alumno.SingleOrDefault(x => x.Email == buscado.Email && x.Password == buscado.Password);
            if(alumnoDb != null)
            {
                return alumnoDb.IdAlumno;
            }
            else
            {
                return 0;
            }

        }

        public Alumno buscarAlumnoPorId(int id)
        {
            Alumno alumno = ctx.Alumno.Find(id);
            return alumno;
        }
        public List<Pregunta> ultimasDosPreguntas()
        {
            var ultimasDosPreguntas = (from p in ctx.Pregunta.Include("RespuestaAlumno").Include("Alumno")
                                                 join r in ctx.RespuestaAlumno on p.IdPregunta equals r.IdPregunta
                                                 join a in ctx.Alumno on r.IdAlumno equals a.IdAlumno
                                       where p.FechaDisponibleHasta < DateTime.Now
                                       orderby p.FechaDisponibleHasta descending
                                       select p).Take(2).ToList();
            return ultimasDosPreguntas;
        }

        public List<Alumno> TablaDePosiciones()
        {
            List<Alumno> tablaDePosiciones = new List<Alumno>();
            tablaDePosiciones = ctx.Alumno.OrderByDescending(x => x.CantidadMejorRespuesta).OrderByDescending(x => x.CantidadRespuestasCorrectas).OrderByDescending(x => x.PuntosTotales).ToList();
            return tablaDePosiciones;
        }

        public List<Pregunta> PreguntasSinResponder(int id)
        {
            List<Pregunta> preguntasSinResponder = new List<Pregunta>();
            var preguntasSR = (from p in ctx.Pregunta.Include("RespuestaAlumno")
                              from r in ctx.RespuestaAlumno
                              where r.IdAlumno != id
                             select p).Distinct();
            var respondidas = (from p in ctx.Pregunta
                              join r in ctx.RespuestaAlumno on p.IdPregunta equals r.IdPregunta
                              join a in ctx.Alumno on r.IdAlumno equals a.IdAlumno
                              where a.IdAlumno == id
                              select p).Distinct();
            preguntasSinResponder = preguntasSR.Except(respondidas).ToList();
            return preguntasSinResponder;
        }

        public void EnviarEmailMejorRespuesta(RespuestaAlumno respuestaPorId)
        {
            Alumno alumno = ctx.Alumno.Find(respuestaPorId.IdAlumno);
            Pregunta pregunta = ctx.Pregunta.Find(respuestaPorId.IdPregunta);
            System.Net.Mail.MailMessage mensaje = new System.Net.Mail.MailMessage();
            string email = alumno.Email;
            mensaje.To.Add(email);
            string asunto = "Su respuesta ha sido marcada como la mejor. ¡Felicitaciones!";
            mensaje.Subject = asunto;
            mensaje.SubjectEncoding = System.Text.Encoding.UTF8;

            int alumnoId = respuestaPorId.IdAlumno;
            string preguntaTxt = pregunta.Pregunta1;
            string respuesta = respuestaPorId.Respuesta;
            string linkRespuesta = ("http://localhost:53443/Alumno/VerPreguntasAlumno/1");
            string linkPosiciones = ("http://localhost:53443/Login/AlumnoIndex/" + alumnoId);


            mensaje.Body = ("Su respuesta ha sido marcada como la mejor! Pregunta: " + preguntaTxt + " Su Respuesta: " + respuesta + " " + linkRespuesta + " Posiciones: " + linkPosiciones + "" + " Felicidades!");
            mensaje.BodyEncoding = System.Text.Encoding.UTF8;
            mensaje.IsBodyHtml = true;
            mensaje.From = new System.Net.Mail.MailAddress("alan.boca12@gmail.com");

            System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();
            cliente.Credentials = new System.Net.NetworkCredential("restocomidas@gmail.com", "unlam2018");
            cliente.Port = 587;
            cliente.EnableSsl = true;
            cliente.Host = "smtp.gmail.com";

            cliente.Send(mensaje);


        }
        public void EvaluarPreguntaCorrecta(int idProfesor, RespuestaAlumno respuestaPorId)
        {
            int PuntajeMaximoPorRespuestaCorrecta = Convert.ToInt32(WebConfigurationManager.AppSettings["PuntajeMaximoPorRespuestaCorrecta"]);
            int CupoMaximoRespuestasCorrectas = Convert.ToInt32(WebConfigurationManager.AppSettings["CupoMaximoRespuestasCorrectas"]);


            int idrespuesta = respuestaPorId.IdRespuestaAlumno;
            RespuestaAlumno respuesta = ctx.RespuestaAlumno.Find(idrespuesta);

            int idAlumno = respuestaPorId.IdAlumno;
            Alumno alumno = ctx.Alumno.Find(idAlumno);

            int idPre = respuestaPorId.IdPregunta;

            List<RespuestaAlumno> RespuestasCorrectasHastaElMomento = (from r in ctx.RespuestaAlumno where r.IdPregunta == idPre && r.IdResultadoEvaluacion == 1 select r).ToList();

            var SumaRespuestasCorrectasHastaElMomento = RespuestasCorrectasHastaElMomento.Select(x => x.IdResultadoEvaluacion).Sum();

            respuesta.RespuestasCorrectasHastaElMomento = SumaRespuestasCorrectasHastaElMomento;

            int respCorr = (int)SumaRespuestasCorrectasHastaElMomento;

            int puntajeRespuesta = PuntajeMaximoPorRespuestaCorrecta - (1000 / CupoMaximoRespuestasCorrectas * (respCorr));

 
                if (puntajeRespuesta >= 0)
                {
                     respuesta.Puntos = puntajeRespuesta;

                     int puntosTotalesSumandos = (int)alumno.PuntosTotales + puntajeRespuesta;
                     alumno.PuntosTotales = puntosTotalesSumandos;


                }
                else
                {
                    respuesta.Puntos = 100;
                    int puntosTotalesSumandos = (int)alumno.PuntosTotales + 100;
                    alumno.PuntosTotales = puntosTotalesSumandos;

            }

            respuesta.FechaHoraEvaluacion = DateTime.Now;
            respuesta.IdProfesorEvaluador = idProfesor;
            respuesta.IdResultadoEvaluacion = 1;

            ctx.SaveChanges();

        }

        public void EvaluarPreguntaRegular(int idProfesor, RespuestaAlumno respuestaPorId)
        {
            int PuntajeMaximoPorRespuestaCorrecta = Convert.ToInt32(WebConfigurationManager.AppSettings["PuntajeMaximoPorRespuestaCorrecta"]);
            int CupoMaximoRespuestasCorrectas = Convert.ToInt32(WebConfigurationManager.AppSettings["CupoMaximoRespuestasCorrectas"]);


            int idrespuesta = respuestaPorId.IdRespuestaAlumno;
            RespuestaAlumno respuesta = ctx.RespuestaAlumno.Find(idrespuesta);

            int idAlumno = respuestaPorId.IdAlumno;
            Alumno alumno = ctx.Alumno.Find(idAlumno);

            var RespuestasCorrectasHastaElMomento = (from r in ctx.RespuestaAlumno where r.IdPregunta == respuestaPorId.IdPregunta && r.IdResultadoEvaluacion == 1 select r).ToList();

            var SumaRespuestasCorrectasHastaElMomento = RespuestasCorrectasHastaElMomento.Select(x => x.IdResultadoEvaluacion).Sum();

            respuesta.RespuestasCorrectasHastaElMomento = SumaRespuestasCorrectasHastaElMomento;

            int respCorr = (int)SumaRespuestasCorrectasHastaElMomento;

            int puntajeRespuesta = (PuntajeMaximoPorRespuestaCorrecta - (1000 / CupoMaximoRespuestasCorrectas * (respCorr)))/2;

            if (puntajeRespuesta >= 0)
            {
                respuesta.Puntos = puntajeRespuesta;
                int puntosTotalesSumandos = (int)alumno.PuntosTotales + puntajeRespuesta;
                alumno.PuntosTotales = puntosTotalesSumandos;
            }
            else
            {
                respuesta.Puntos = 100;
                int puntosTotalesSumandos = (int)alumno.PuntosTotales + 100;
                alumno.PuntosTotales = puntosTotalesSumandos;
            }

            respuesta.FechaHoraEvaluacion = DateTime.Now;
            respuesta.IdProfesorEvaluador = idProfesor;
            respuesta.IdResultadoEvaluacion = 2;
            ctx.SaveChanges();

        }

        public void EvaluarPreguntaMal(int idProfesor, RespuestaAlumno respuestaPorId)
        {

            int idrespuesta = respuestaPorId.IdRespuestaAlumno;
            RespuestaAlumno respuesta = ctx.RespuestaAlumno.Find(idrespuesta);

            respuesta.Puntos = 0;

            respuesta.FechaHoraEvaluacion = DateTime.Now;
            respuesta.IdProfesorEvaluador = idProfesor;
            respuesta.IdResultadoEvaluacion = 3;

            ctx.SaveChanges();

        }

        public void EnviarEmailEvaluacion(int idProfesor, RespuestaAlumno respuestaPorId)
        {

            Alumno alumno = ctx.Alumno.Find(respuestaPorId.IdAlumno);
            Pregunta pregunta = ctx.Pregunta.Find(respuestaPorId.IdPregunta);
            int idrespuesta = respuestaPorId.IdRespuestaAlumno;
            RespuestaAlumno respuesta = ctx.RespuestaAlumno.Find(idrespuesta);

            System.Net.Mail.MailMessage mensaje = new System.Net.Mail.MailMessage();
            string email = alumno.Email;
            mensaje.To.Add(email);
            string asunto = "Su respuesta fue calificada como"+respuesta.ResultadoEvaluacion.Resultado;
            mensaje.Subject = asunto;
            mensaje.SubjectEncoding = System.Text.Encoding.UTF8;

            int alumnoId = respuestaPorId.IdAlumno;
            string preguntaTxt = respuesta.Pregunta.Pregunta1;
            string respuestaTxt = respuesta.Respuesta;
            string linkRespuesta = ("http://localhost:53443/Alumno/VerPreguntasAlumno/1");
            string linkPosiciones = ("http://localhost:53443/Login/AlumnoIndex/" + alumnoId);


            mensaje.Body = ("Pregunta: " + preguntaTxt + " Su Respuesta: " + respuestaTxt + " " + linkRespuesta + " Posiciones: " + linkPosiciones);
            mensaje.BodyEncoding = System.Text.Encoding.UTF8;
            mensaje.IsBodyHtml = true;
            mensaje.From = new System.Net.Mail.MailAddress("alan.boca12@gmail.com");

            System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();
            cliente.Credentials = new System.Net.NetworkCredential("restocomidas@gmail.com", "unlam2018");
            cliente.Port = 587;
            cliente.EnableSsl = true;
            cliente.Host = "smtp.gmail.com";

            cliente.Send(mensaje);

        }
    }
}