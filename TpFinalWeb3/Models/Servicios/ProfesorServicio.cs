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

        public void CrearPregunta(Pregunta p, int[] ListaClases, int[] ListaTemas, int id)
        {
            MyContext ctx = new MyContext();
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
            MyContext ctx = new MyContext();
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

        public Pregunta BuscarPreguntaPorId(int id)
        {
            MyContext ctx = new MyContext();
            Pregunta preguntaPorId = ctx.Pregunta.Find(id);
            return preguntaPorId;
        }

        public void ModificarPregunta(Pregunta preguntaModificada, int[] ListaClases, int[] ListaTemas, int idProfesor)
        {
            MyContext ctx = new MyContext();
            int idPregunta = preguntaModificada.IdPregunta;
            Pregunta preguntaPorId = BuscarPreguntaPorId(idPregunta);
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
    }
}