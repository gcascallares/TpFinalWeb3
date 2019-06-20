using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace TpFinalWeb3.Models.Servicios
{
    public class AlumnoServicio
    {
        public Alumno VerificarAlumnoLogin(LoginServicio buscado)
        {
            MyContext ctx = new MyContext();
            Alumno alumnoDb = ctx.Alumno.SingleOrDefault(x => x.Email == buscado.Email && x.Password == buscado.Password);
            if(alumnoDb != null)
            {
                return alumnoDb;
            }
            else
            {
                return null;
            }

        }

<<<<<<< HEAD
        public List<Pregunta> ultimasDosPreguntas()
        {
            MyContext ctx = new MyContext();
            List<Pregunta> dosPreguntas = ((from p in ctx.Pregunta where p.FechaDisponibleHasta < DateTime.Now orderby p.FechaDisponibleHasta descending select p).Take(2)).ToList();
            return dosPreguntas;
=======
        public List<Alumno> TablaDePosiciones()
        {
            MyContext ctx = new MyContext();
            List<Alumno> tablaDePosiciones = new List<Alumno>();
            tablaDePosiciones = ctx.Alumno.OrderByDescending(x => x.CantidadMejorRespuesta).OrderByDescending(x => x.CantidadRespuestasCorrectas).OrderByDescending(x => x.PuntosTotales).ToList();
            return tablaDePosiciones;
        }

        public List<Pregunta> PreguntasSinResponder(int id)
        {
            MyContext ctx = new MyContext();
            List<Pregunta> preguntasSinResponder = new List<Pregunta>();
              var preguntasSR =(
                from p in ctx.Pregunta
                join r in ctx.RespuestaAlumno on p.IdPregunta equals r.IdPregunta
                join a in ctx.Alumno on r.IdAlumno equals a.IdAlumno
                where a.IdAlumno != id
                select p).Distinct();
            preguntasSinResponder = preguntasSR.ToList();
            return preguntasSinResponder;
>>>>>>> master
        }
    }
}