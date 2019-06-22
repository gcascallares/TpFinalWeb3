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

        public RespuestaAlumno BuscarPreguntaEvaluar(int id)
        {
            MyContext ctx = new MyContext();
            RespuestaAlumno respuestasPorId = ctx.RespuestaAlumno.Find(id);
            return respuestasPorId;
        }

    }
}