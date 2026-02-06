using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace JOTATECH_SOLUCIONES.CapaDatos
{
    public class CD_Dashboard
    {
        private static string conexion = ConfigurationManager.ConnectionStrings["ConexionJotaTech"].ToString();

        // -----------------------------------------------------
        // 1. Obtener el total de productos activos
        // -----------------------------------------------------
        public int ObtenerTotalProductos()
        {
            int total = 0;
            using (SqlConnection oConexion = new SqlConnection(conexion))
            {
                // Consulta SQL para contar productos con Activo = 1
                string query = "SELECT COUNT(*) FROM tb_producto WHERE Activo = 1";
                SqlCommand cmd = new SqlCommand(query, oConexion);

                try
                {
                    oConexion.Open();
                    total = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch { total = 0; }
            }
            return total;
        }

        // -----------------------------------------------------
        // 2. Obtener la suma total de ventas del mes
        // -----------------------------------------------------
        public decimal ObtenerVentaTotalDelMes()
        {
            decimal totalVenta = 0;
            using (SqlConnection oConexion = new SqlConnection(conexion))
            {
                // SQL para sumar las ventas del mes y año actual
                string query = @"
                    SELECT ISNULL(SUM(TotalVenta), 0) 
                    FROM tb_venta 
                    WHERE MONTH(FechaVenta) = MONTH(GETDATE()) 
                    AND YEAR(FechaVenta) = YEAR(GETDATE())";

                SqlCommand cmd = new SqlCommand(query, oConexion);

                try
                {
                    oConexion.Open();
                    totalVenta = Convert.ToDecimal(cmd.ExecuteScalar());
                }
                catch { totalVenta = 0; }
            }
            return totalVenta;
        }

        // -----------------------------------------------------
        // 3. Obtener la cantidad de boletas emitidas en el mes
        // -----------------------------------------------------
        public int ObtenerBoletasEmitidasDelMes()
        {
            int cantidad = 0;
            using (SqlConnection oConexion = new SqlConnection(conexion))
            {
                // SQL para contar las boletas (registros de venta) del mes y año actual
                string query = @"
                    SELECT COUNT(*) 
                    FROM tb_venta 
                    WHERE MONTH(FechaVenta) = MONTH(GETDATE()) 
                    AND YEAR(FechaVenta) = YEAR(GETDATE())";

                SqlCommand cmd = new SqlCommand(query, oConexion);

                try
                {
                    oConexion.Open();
                    cantidad = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch { cantidad = 0; }
            }
            return cantidad;
        }
    }
}