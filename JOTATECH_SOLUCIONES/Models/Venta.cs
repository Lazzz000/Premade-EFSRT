using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JOTATECH_SOLUCIONES.Models
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public string NumeroBoleta { get; set; }
        public DateTime FechaVenta { get; set; }
        public decimal TotalVenta { get; set; }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }

        public List<DetalleVenta> Detalles { get; set; } // Lista de productos vendidos
    }
}