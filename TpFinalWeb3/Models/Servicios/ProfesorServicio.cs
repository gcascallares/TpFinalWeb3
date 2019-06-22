using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TpFinalWeb3.Models.Servicios
{
    public class ProfesorServicio
    {
        public Profesor VerificarProfesorLogin(LoginServicio buscado)
        {
            MyContext ctx = new MyContext();
            Profesor profesorDb = ctx.Profesor.SingleOrDefault(x => x.Email == buscado.Email && x.Password == buscado.Password);
            if(profesorDb != null)
            {
                return profesorDb;
            }
            else
            {
                return null;
            }

        }

        public Pregunta BuscarPreguntaPorId(int id)
        {
            MyContext ctx = new MyContext();
            Pregunta preguntaPorId = ctx.Pregunta.Find(id);
            return preguntaPorId;
        }


        public void ModificarNroPregunta(int NroPregunta, int idPregunta)
        {
            MyContext ctx = new MyContext();
            var pregunta = ctx.Pregunta.Find(idPregunta);
            pregunta.Nro = NroPregunta;
            ctx.SaveChanges();
        }

        public void ModificarDescripcionPregunta(string descripcionPregunta, int idPregunta)
        {
            MyContext ctx = new MyContext();
            var pregunta = ctx.Pregunta.Find(idPregunta);
            pregunta.Pregunta1 = descripcionPregunta;
            ctx.SaveChanges();
        }

        public List<RespuestaAlumno> BuscarPreguntaEvaluar(int id)
        {
            MyContext ctx = new MyContext();
            List<RespuestaAlumno> respuestasPorId = (from r in ctx.RespuestaAlumno where r.IdPregunta == id  select r).ToList();
            return respuestasPorId;
        }

        public List<RespuestaAlumno> BuscarPreguntaEvaluarCorrecta(int id)
        {
            MyContext ctx = new MyContext();
            List<RespuestaAlumno> respuestasPorId = (from r in ctx.RespuestaAlumno where r.IdPregunta == id && r.IdResultadoEvaluacion == 1 select r).ToList();
            return respuestasPorId;
        }

        public List<RespuestaAlumno> BuscarPreguntaEvaluarSinCorreguir(int id)
        {
            MyContext ctx = new MyContext();
            List<RespuestaAlumno> respuestasPorId = (from r in ctx.RespuestaAlumno where r.IdPregunta == id && r.IdResultadoEvaluacion == null select r).ToList();
            return respuestasPorId;
        }

     
        public List<RespuestaAlumno> BuscarPreguntaEvaluarRegular(int id)
        {
            MyContext ctx = new MyContext();
            List<RespuestaAlumno> respuestasPorId = (from r in ctx.RespuestaAlumno where r.IdPregunta == id && r.IdResultadoEvaluacion == 2 select r).ToList();
            return respuestasPorId;
        }

        public List<RespuestaAlumno> BuscarPreguntaEvaluarMal(int id)
        {
            MyContext ctx = new MyContext();
            List<RespuestaAlumno> respuestasPorId = (from r in ctx.RespuestaAlumno where r.IdPregunta == id && r.IdResultadoEvaluacion == 3 select r).ToList();
            return respuestasPorId;
        }
    }
}