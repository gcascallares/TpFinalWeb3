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

        public List<Pregunta> ultimasDosPreguntas()
        {
            MyContext ctx = new MyContext();
            List<Pregunta> dosPreguntas = ((from p in ctx.Pregunta where p.FechaDisponibleHasta < DateTime.Now orderby p.FechaDisponibleHasta descending select p).Take(2)).ToList();
            return dosPreguntas;
        }
    }
}