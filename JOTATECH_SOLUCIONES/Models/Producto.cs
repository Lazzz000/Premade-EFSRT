using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JOTATECH_SOLUCIONES.Models
{
    public class Producto
    {
        // Clave primaria
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El campo Código es obligatorio")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "La Descripción es obligatoria")]
        public string Descripcion { get; set; }

        // Campo opcional (puede ser nulo en la base de datos si no usas categorías)
        public string Categoria { get; set; }

        [Display(Name = "Precio Venta")]
        [Required(ErrorMessage = "El campo Precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero")]
        public decimal PrecioVenta { get; set; }

        [Required(ErrorMessage = "El campo Stock es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        // Si estás usando la base de datos que definimos, podrías añadir un campo Activo
        public bool Activo { get; set; }
    }
}