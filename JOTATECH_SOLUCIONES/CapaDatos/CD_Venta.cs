using JOTATECH_SOLUCIONES.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace JOTATECH_SOLUCIONES.CapaDatos
{
    public class CD_Venta
    {
        private static string conexion = ConfigurationManager.ConnectionStrings["ConexionJotaTech"].ToString();

        // -----------------------------------------------------
        // 1. REGISTRAR VENTA (Transacción)
        // -----------------------------------------------------
        public Dictionary<string, string> RegistrarVenta(Venta oVenta)
        {
            // Diccionario para devolver el resultado (ID y Número de Boleta)
            Dictionary<string, string> resultado = new Dictionary<string, string>();
            resultado.Add("IdVenta", "0");
            resultado.Add("NumeroBoleta", "");

            using (SqlConnection oConexion = new SqlConnection(conexion))
            {
                // A. Convertir la lista de Detalles a formato XML para el stored procedure
                StringBuilder xmlDetalle = new StringBuilder();
                xmlDetalle.Append("<Detalles>");
                foreach (DetalleVenta dv in oVenta.Detalles)
                {
                    xmlDetalle.Append("<Detalle>");
                    xmlDetalle.Append($"<IdProducto>{dv.IdProducto}</IdProducto>");
                    xmlDetalle.Append($"<Cantidad>{dv.Cantidad}</Cantidad>");
                    xmlDetalle.Append($"<PrecioUnitario>{dv.PrecioUnitario}</PrecioUnitario>");
                    xmlDetalle.Append($"<TotalLinea>{dv.TotalLinea}</TotalLinea>");
                    xmlDetalle.Append("</Detalle>");
                }
                xmlDetalle.Append("</Detalles>");

                // B. Configurar la llamada al procedimiento
                SqlCommand cmd = new SqlCommand("sp_RegistrarVenta", oConexion);
                cmd.Parameters.AddWithValue("@IdUsuario", oVenta.IdUsuario);
                cmd.Parameters.AddWithValue("@TotalVenta", oVenta.TotalVenta);
                cmd.Parameters.AddWithValue("@XMLDetalle", xmlDetalle.ToString());
                cmd.CommandType = CommandType.StoredProcedure;

                // Parámetros de Salida (OUTPUT)
                cmd.Parameters.Add("@IdVentaGenerado", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@NumeroBoletaGenerado", SqlDbType.VarChar, 10).Direction = ParameterDirection.Output;

                try
                {
                    oConexion.Open();
                    cmd.ExecuteNonQuery(); // Ejecutar el procedimiento

                    // C. Leer los resultados OUTPUT
                    resultado["IdVenta"] = cmd.Parameters["@IdVentaGenerado"].Value.ToString();
                    resultado["NumeroBoleta"] = cmd.Parameters["@NumeroBoletaGenerado"].Value.ToString();
                }
                catch (Exception)
                {
                    // Si falla, los valores en 'resultado' seguirán siendo "0" y ""
                }
            }
            return resultado;
        }

        // -----------------------------------------------------
        // 2. OBTENER PRODUCTOS PARA EL FORMULARIO DE VENTA
        // -----------------------------------------------------
        // Se añade un método para buscar un producto por código para la vista de venta (usado por AJAX)
        public Producto BuscarProducto(string codigo)
        {
            // Reutilizamos parte de la lógica de Obtener de CD_Producto, simplificando la consulta
            Producto oProducto = null;
            using (SqlConnection oConexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("SELECT IdProducto, Codigo, Descripcion, PrecioVenta, Stock FROM tb_producto WHERE Codigo = @codigo AND Activo = 1", oConexion);
                cmd.Parameters.AddWithValue("@codigo", codigo);
                cmd.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            oProducto = new Producto()
                            {
                                IdProducto = Convert.ToInt32(dr["IdProducto"]),
                                Codigo = dr["Codigo"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                                Stock = Convert.ToInt32(dr["Stock"])
                            };
                        }
                    }
                }
                catch { oProducto = null; }
            }
            return oProducto;
        }

        // Archivo: CapaDatos/CD_Venta.cs (Añadir este método)

        public Venta ObtenerVenta(int idVenta)
        {
            Venta oVenta = null;
            using (SqlConnection oConexion = new SqlConnection(conexion))
            {
                // 1. Obtener la cabecera de la venta
                SqlCommand cmdCabecera = new SqlCommand("SELECT IdVenta, NumeroBoleta, FechaVenta, TotalVenta, u.NombreCompleto AS NombreUsuario FROM tb_venta v JOIN tb_usuario u ON v.IdUsuario = u.IdUsuario WHERE IdVenta = @id", oConexion);
                cmdCabecera.Parameters.AddWithValue("@id", idVenta);
                cmdCabecera.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmdCabecera.ExecuteReader();

                    if (dr.Read())
                    {
                        oVenta = new Venta()
                        {
                            IdVenta = Convert.ToInt32(dr["IdVenta"]),
                            NumeroBoleta = dr["NumeroBoleta"].ToString(),
                            FechaVenta = Convert.ToDateTime(dr["FechaVenta"]),
                            TotalVenta = Convert.ToDecimal(dr["TotalVenta"]),
                            NombreUsuario = dr["NombreUsuario"].ToString(), // Asume que agregaste NombreUsuario a la clase Venta
                            Detalles = new List<DetalleVenta>()
                        };
                    }
                    dr.Close(); // Cerrar el primer DataReader

                    // 2. Obtener los detalles de la venta
                    SqlCommand cmdDetalle = new SqlCommand("SELECT d.Cantidad, d.PrecioUnitario, d.TotalLinea, p.Codigo AS CodigoProducto, p.Descripcion AS DescripcionProducto FROM tb_detalle_venta d JOIN tb_producto p ON d.IdProducto = p.IdProducto WHERE IdVenta = @id", oConexion);
                    cmdDetalle.Parameters.AddWithValue("@id", idVenta);
                    cmdDetalle.CommandType = CommandType.Text;

                    SqlDataReader drDetalle = cmdDetalle.ExecuteReader();
                    while (drDetalle.Read())
                    {
                        oVenta.Detalles.Add(new DetalleVenta()
                        {
                            Cantidad = Convert.ToInt32(drDetalle["Cantidad"]),
                            PrecioUnitario = Convert.ToDecimal(drDetalle["PrecioUnitario"]),
                            TotalLinea = Convert.ToDecimal(drDetalle["TotalLinea"]),
                            CodigoProducto = drDetalle["CodigoProducto"].ToString(),
                            DescripcionProducto = drDetalle["DescripcionProducto"].ToString()
                        });
                    }
                    drDetalle.Close();
                }
                catch { oVenta = null; }
            }
            return oVenta;
        }
    }
}