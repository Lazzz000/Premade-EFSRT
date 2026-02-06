using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JOTATECH_SOLUCIONES.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El campo Usuario es obligatorio.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
        [DataType(DataType.Password)]
        public string Clave { get; set; }

        public string NombreCompleto { get; set; }
        public string Rol { get; set; }
    }
}