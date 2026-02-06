using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JOTATECH_SOLUCIONES.Models
{
    public class DetalleVenta
    {
        public int IdDetalleVenta { get; set; }
        public int IdVenta { get; set; }
        public int IdProducto { get; set; }

        // Propiedades del Producto (para mostrar en la vista)
        public string CodigoProducto { get; set; }
        public string DescripcionProducto { get; set; }

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal TotalLinea { get; set; }
    }
}