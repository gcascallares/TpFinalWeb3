using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TpFinalWeb3.Helpers
{
    public static class SesionHelper
    {
        public static int IdUsuario
        {
            get
            {
                if (HttpContext.Current.Session["ids"] == null)
                {
                    return 0;
                }
                return (int)HttpContext.Current.Session["ids"];
            }
            set { HttpContext.Current.Session["ids"] = value; }
        }
        public static string RolUsuario { get; set; }
    }
}