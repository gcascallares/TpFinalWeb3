using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TpFinalWeb3.Models;

namespace TpFinalWeb3.Models.Servicios
{
    public class PreguntaServicio
    {

        public int OrdenRespuesta(int idPregunta)
        {
            MyContext ctx = new MyContext();
            int orden = (
            from r in ctx.RespuestaAlumno
            join p in ctx.Pregunta on r.IdPregunta equals p.IdPregunta
            where p.IdPregunta == idPregunta
            select r).Count();

            return orden;
        }
        public void GuardarRespuesta(Pregunta preg, string respuesta, int idAlumno)
        {
            MyContext ctx = new MyContext();
            RespuestaAlumno respuestaAlumno = new RespuestaAlumno();
            respuestaAlumno.IdPregunta = preg.IdPregunta;
            respuestaAlumno.IdAlumno = idAlumno;
            respuestaAlumno.FechaHoraRespuesta = DateTime.Now;
            respuestaAlumno.Respuesta = respuesta;
            int orden = OrdenRespuesta(preg.IdPregunta);

            respuestaAlumno.Orden = orden + 1;
            ctx.RespuestaAlumno.Add(respuestaAlumno);
            ctx.SaveChanges();
        }

        public Pregunta BuscarPreguntaPorId(int id)
        {
            MyContext ctx = new MyContext();
            Pregunta preguntaPorId = ctx.Pregunta.Find(id);
            return preguntaPorId;
        }

        public List<RespuestaAlumno> VerPreguntaEvaluarCorrecta(int id)
        {
            MyContext ctx = new MyContext();
            List<RespuestaAlumno> respuestasCorrectas = 
            (from r in ctx.RespuestaAlumno where r.IdAlumno==id && r.IdResultadoEvaluacion == 1 select r).ToList();
            return respuestasCorrectas;
        }

        public List<RespuestaAlumno> VerPreguntaEvaluarRegular(int id)
        {
            MyContext ctx = new MyContext();
            List<RespuestaAlumno> respuestasRegular =
            (from r in ctx.RespuestaAlumno where r.IdAlumno == id && r.IdResultadoEvaluacion == 2 select r).ToList();
            return respuestasRegular;
        }

        public List<RespuestaAlumno> VerPreguntaEvaluarMal(int id)
        {
            MyContext ctx = new MyContext();
            List<RespuestaAlumno> respuestasMal =
            (from r in ctx.RespuestaAlumno where r.IdAlumno == id && r.IdResultadoEvaluacion == 3 select r).ToList();
            return respuestasMal;
        }

        public List<RespuestaAlumno> VerPreguntaSinCorregir(int id)
        {
            MyContext ctx = new MyContext();
            List<RespuestaAlumno> respuestasSinCorregir = 
            (from r in ctx.RespuestaAlumno
             where r.IdAlumno == id && r.IdResultadoEvaluacion == null select r).ToList();
            return respuestasSinCorregir;
        }
    }
}