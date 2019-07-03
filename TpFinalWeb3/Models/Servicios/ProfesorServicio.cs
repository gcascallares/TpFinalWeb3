using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TpFinalWeb3.Models.Servicios
{
    public class ProfesorServicio
    {
        MyContext ctx = new MyContext();
        public int VerificarProfesorLogin(LoginServicio buscado)
        {
            Profesor profesorDb = ctx.Profesor.SingleOrDefault(x => x.Email == buscado.Email && x.Password == buscado.Password);
            if(profesorDb != null)
            {
                return profesorDb.IdProfesor;
            }
            else
            {
                return 0;
            }
        }
        public Profesor buscarProfesorPorId(int id)
        {
            Profesor profesor = ctx.Profesor.Find(id);
            return profesor;
        }
        public List<RespuestaAlumno> BuscarPreguntaEvaluar(int id)
        {
            List<RespuestaAlumno> respuestasPorId = (from r in ctx.RespuestaAlumno where r.IdPregunta == id orderby r.FechaHoraRespuesta ascending select r).ToList();
            return respuestasPorId;
        }

        public List<RespuestaAlumno> BuscarPreguntaEvaluarCorrecta(int id)
        {
            List<RespuestaAlumno> respuestasPorId = (from r in ctx.RespuestaAlumno where r.IdPregunta == id && r.IdResultadoEvaluacion == 1 select r).ToList();
            return respuestasPorId;
        }

        public List<RespuestaAlumno> BuscarPreguntaEvaluarSinCorreguir(int id)
        {
            List<RespuestaAlumno> respuestasPorId = (from r in ctx.RespuestaAlumno where r.IdPregunta == id && r.IdResultadoEvaluacion == null select r).ToList();
            return respuestasPorId;
        }

     
        public List<RespuestaAlumno> BuscarPreguntaEvaluarRegular(int id)
        {
            List<RespuestaAlumno> respuestasPorId = (from r in ctx.RespuestaAlumno where r.IdPregunta == id && r.IdResultadoEvaluacion == 2 select r).ToList();
            return respuestasPorId;
        }

        public List<RespuestaAlumno> BuscarPreguntaEvaluarMal(int id)
        {
            List<RespuestaAlumno> respuestasPorId = (from r in ctx.RespuestaAlumno where r.IdPregunta == id && r.IdResultadoEvaluacion == 3 select r).ToList();
            return respuestasPorId;
        }

        public void CrearPregunta(Pregunta p, int[] ListaClases, int[] ListaTemas, int id)
        {
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
            p.IdProfesorCreacion = id;
            p.FechaHoraCreacion = DateTime.Now;
            p.Nro = p.Nro;
            p.Pregunta1 = p.Pregunta1;
            ctx.Pregunta.Add(p);
            ctx.SaveChanges();
        }

        public Boolean EliminarPregunta(int id)
        {
            Pregunta p = ctx.Pregunta.FirstOrDefault(x => x.IdPregunta == id);
            if (p.RespuestaAlumno.Count() == 0)
            {
                ctx.Pregunta.Remove(p);
                ctx.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }

        public Boolean TotalCorregidas(int id)
        {
            List<RespuestaAlumno> q = (from r in ctx.RespuestaAlumno where r.IdPregunta == id && r.IdResultadoEvaluacion == null select r).ToList();

            if (q.Count() == 0)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public Boolean MejorRespuesta(int id)
        {
            List<RespuestaAlumno> q = (from r in ctx.RespuestaAlumno where r.IdPregunta == id && r.MejorRespuesta == true select r).ToList();

            if (q.Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public RespuestaAlumno BuscarRespuestaPorId(int idRespuestaAlumno)
        {
            RespuestaAlumno respuestaPorId = ctx.RespuestaAlumno.Find(idRespuestaAlumno);
            return respuestaPorId;
        }
        public void ActivarMejorRespuesta(RespuestaAlumno respuestaPorId)
        {
            int idrespuesta = respuestaPorId.IdRespuestaAlumno;
            RespuestaAlumno respuesta = ctx.RespuestaAlumno.Find(idrespuesta);
            respuesta.MejorRespuesta = true;
            int PuntosTotales = (int)respuesta.Puntos + 500;
            respuesta.Puntos = PuntosTotales;
            ctx.SaveChanges();
        }


        public Pregunta BuscarPreguntaPorId(int id)
        {
            Pregunta preguntaPorId = ctx.Pregunta.Find(id);
            return preguntaPorId;
        }

        public Boolean VerificarNroPregunta(int Nro)
        {
            List<Pregunta> preguntasPorNro = ctx.Pregunta.Where(x => x.Nro == Nro).ToList();
            if(preguntasPorNro.Count()==0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ModificarPregunta(Pregunta preguntaModificada, int[] ListaClases, int[] ListaTemas, int idProfesor)
        {
            MyContext ctx = new MyContext();
            int idPregunta = preguntaModificada.IdPregunta;
            Pregunta preguntaPorId = ctx.Pregunta.Find(idPregunta);
            preguntaModificada.FechaHoraModificacion = DateTime.Now;
            preguntaPorId.Nro = preguntaModificada.Nro;
            preguntaPorId.Pregunta1 = preguntaModificada.Pregunta1;
            preguntaPorId.FechaHoraModificacion = preguntaModificada.FechaHoraModificacion;
            preguntaPorId.FechaDisponibleDesde = preguntaModificada.FechaDisponibleDesde;
            preguntaPorId.FechaDisponibleHasta = preguntaModificada.FechaDisponibleHasta;
            preguntaPorId.IdProfesorModificacion = idProfesor;
            foreach (int IdClase in ListaClases)
            {
                Clase c = new Clase();
                c = ctx.Clase.Find(IdClase);
                preguntaPorId.Clase = c;
            }
            foreach (int IdTema in ListaTemas)
            {
                Tema t = new Tema();
                t = ctx.Tema.Find(IdTema);
                preguntaPorId.Tema = t;
            }
            ctx.SaveChanges();
        }

        public void EnviarEmailRespuestaAlumno(Pregunta preg, string respuesta, int idAlumno)
        {
            MyContext ctx = new MyContext();
            Alumno alumno = ctx.Alumno.Find(idAlumno);
            Pregunta pregunta = ctx.Pregunta.Find(preg.IdPregunta);

            RespuestaAlumno respuestaAlumno = ctx.RespuestaAlumno.SingleOrDefault(x => x.IdPregunta == pregunta.IdPregunta && x.IdAlumno == alumno.IdAlumno);
            Profesor profe = ctx.Profesor.Find(pregunta.IdProfesorCreacion);


            //string email = profe.Email;
            //List<string> listaEmailsProfesores = (from p in ctx.Profesor select p.Email).ToList();  

            System.Net.Mail.MailMessage mensaje = new System.Net.Mail.MailMessage();
     
            mensaje.To.Add("pnsanchez@unlam.edu.ar");
            mensaje.Bcc.Add("matiaspaz@test.com");
            mensaje.Bcc.Add("marianojuiz@test.com");
          

            string asunto = "Respuesta a Pregunta " + pregunta.Nro + " Orden: " + respuestaAlumno.Orden + " Apellido " + alumno.Apellido;
                mensaje.Subject = asunto;
                mensaje.SubjectEncoding = System.Text.Encoding.UTF8;


                string evaluarPregunta = ("http://localhost:53443/Profesor/EvaluarPregunta/" + pregunta.IdPregunta);


                mensaje.Body = ("Pregunta: " + pregunta.Pregunta1 + " Alumno: " + alumno.Nombre + " " + alumno.Apellido + " Orden: " + respuestaAlumno.Orden + "" + " Respuesta:" + respuesta + "Evaluar: " + evaluarPregunta);
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


        public Boolean VerificarRespuestas(int id)
        {
            MyContext ctx = new MyContext();
            var respuestas = (from p in ctx.Pregunta
                              join r in ctx.RespuestaAlumno on p.IdPregunta equals r.IdPregunta
                              where p.IdPregunta == id
                              select p).ToList();
            if(respuestas.Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}