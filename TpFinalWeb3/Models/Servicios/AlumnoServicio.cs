using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace TpFinalWeb3.Models.Servicios
{
    public class AlumnoServicio
    {
        public int VerificarAlumnoLogin(Alumno buscado)
        {
            MyContext ctx = new MyContext();
            Alumno alumnoDb = ctx.Alumno.SingleOrDefault(x => x.Email == buscado.Email);
            if(alumnoDb != null)
            {
                if (alumnoDb.Email == buscado.Email && alumnoDb.Password == buscado.Password)
                {
                    return alumnoDb.IdAlumno;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }
    }
}