﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TpFinalWeb3.Models.Servicios
{
    public class LoginServicio
    {
        [Required(ErrorMessage ="Debe ingresar un Email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(400,ErrorMessage ="Debe ingresar un Email mar corto")]
        [EmailAddress(ErrorMessage = "Debe ingresar un Email valido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Debe ingresar una contraseña")]
        [DataType(DataType.Password)]
        [StringLength(400,ErrorMessage = "Debe ingresar una contraseña mas corta")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        public int CheckProfesor { get; set; }
    }
}