using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TpFinalWeb3.Models.Servicios
{
    public class ProfesorServicio
    {
        public int VerificarProfesorLogin(Profesor buscado)
        {
            MyContext ctx = new MyContext();
            Profesor profesorDb = ctx.Profesor.SingleOrDefault(x => x.Email == buscado.Email);
            if (profesorDb.Email == buscado.Email && profesorDb.Password == buscado.Password)
            {
                return buscado.IdProfesor;
            }
            return 0;

        }
    }
}