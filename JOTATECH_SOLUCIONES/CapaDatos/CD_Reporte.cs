using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using JOTATECH_SOLUCIONES.Models;

namespace JOTATECH_SOLUCIONES.CapaDatos
{
    public class CD_Reporte
    {
        private static string conexion = ConfigurationManager.ConnectionStrings["ConexionJotaTech"].ToString();

        public List<Venta> ReporteVentasPorFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            List<Venta> listaVentas = new List<Venta>();

            using (SqlConnection oConexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_ReporteVentasPorFecha", oConexion);
                cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@FechaFin", fechaFin);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listaVentas.Add(new Venta()
                            {
                                IdVenta = Convert.ToInt32(dr["IdVenta"]),
                                NumeroBoleta = dr["NumeroBoleta"].ToString(),
                                FechaVenta = Convert.ToDateTime(dr["FechaVenta"]),
                                NombreUsuario = dr["UsuarioVendedor"].ToString(),
                                TotalVenta = Convert.ToDecimal(dr["TotalVenta"])
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    // Manejo de error
                    listaVentas = new List<Venta>();
                }
            }
            return listaVentas;
        }

        // -----------------------------------------------------
        // 2. Obtener el Reporte con TODOS los Detalles (Para Excel)
        // -----------------------------------------------------
        public List<ReporteDetalle> ObtenerReporteDetallado(DateTime fechaInicio, DateTime fechaFin)
        {
            List<ReporteDetalle> listaDetalle = new List<ReporteDetalle>();

            using (SqlConnection oConexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_ReporteDetalleVentas", oConexion);
                cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@FechaFin", fechaFin);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listaDetalle.Add(new ReporteDetalle()
                            {
                                NumeroBoleta = dr["NumeroBoleta"].ToString(),
                                FechaVenta = Convert.ToDateTime(dr["FechaVenta"]),
                                Vendedor = dr["Vendedor"].ToString(),
                                TotalVenta = Convert.ToDecimal(dr["TotalVenta"]), // Total de la Venta COMPLETA

                                CodigoProducto = dr["CodigoProducto"].ToString(),
                                DescripcionProducto = dr["DescripcionProducto"].ToString(),
                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                PrecioUnitario = Convert.ToDecimal(dr["PrecioUnitario"]),
                                TotalLinea = Convert.ToDecimal(dr["TotalLinea"])
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    listaDetalle = new List<ReporteDetalle>();
                }
            }
            return listaDetalle;
        }
    }
}