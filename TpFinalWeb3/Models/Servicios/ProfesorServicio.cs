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
            return buscado.IdProfesor;
        }
    }
}