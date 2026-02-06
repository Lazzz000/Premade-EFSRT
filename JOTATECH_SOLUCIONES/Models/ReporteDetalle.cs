using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JOTATECH_SOLUCIONES.Models
{
    public class ReporteDetalle
    {
        // Datos de la Cabecera
        public string NumeroBoleta { get; set; }
        public DateTime FechaVenta { get; set; }
        public string Vendedor { get; set; }
        public decimal TotalVenta { get; set; }

        // Datos del Detalle
        public string CodigoProducto { get; set; }
        public string DescripcionProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal TotalLinea { get; set; } // Subtotal por línea
    }
}