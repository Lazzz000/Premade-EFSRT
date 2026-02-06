using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System;
// Importa el namespace de tu modelo para usar la clase Producto
using JOTATECH_SOLUCIONES.Models;

namespace JOTATECH_SOLUCIONES.CapaDatos
{
    public class CD_Producto
    {
        private static string conexion = ConfigurationManager.ConnectionStrings["ConexionJotaTech"].ToString();

        // -----------------------------------------------------
        // 1. OBTENER LISTA DE PRODUCTOS (Para la vista Index)
        // -----------------------------------------------------
        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();
            using (SqlConnection oConexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_ListarProductos", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Producto()
                            {
                                IdProducto = Convert.ToInt32(dr["IdProducto"]),
                                Codigo = dr["Codigo"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                Categoria = dr["Categoria"].ToString(),
                                PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                                Stock = Convert.ToInt32(dr["Stock"])
                            });
                        }
                    }
                }
                catch { lista = new List<Producto>(); }
            }
            return lista;
        }

        // -----------------------------------------------------
        // 2. OBTENER UN SOLO PRODUCTO (Para la Edición)
        // -----------------------------------------------------
        public Producto Obtener(int idProducto)
        {
            Producto oProducto = null;
            using (SqlConnection oConexion = new SqlConnection(conexion))
            {
                // Usamos la tabla directamente ya que es una búsqueda simple
                SqlCommand cmd = new SqlCommand("SELECT IdProducto, Codigo, Descripcion, Categoria, PrecioVenta, Stock FROM tb_producto WHERE IdProducto = @id", oConexion);
                cmd.Parameters.AddWithValue("@id", idProducto);
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
                                Categoria = dr["Categoria"].ToString(),
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

        // -----------------------------------------------------
        // 3. GUARDAR / ACTUALIZAR PRODUCTO
        // -----------------------------------------------------
        public int Guardar(Producto oProducto)
        {
            int idGenerado = 0;
            using (SqlConnection oConexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_GuardarProducto", oConexion);
                cmd.Parameters.AddWithValue("@idproducto", oProducto.IdProducto);
                cmd.Parameters.AddWithValue("@codigo", oProducto.Codigo);
                cmd.Parameters.AddWithValue("@descripcion", oProducto.Descripcion);
                cmd.Parameters.AddWithValue("@categoria", oProducto.Categoria ?? "");
                cmd.Parameters.AddWithValue("@precio", oProducto.PrecioVenta);
                cmd.Parameters.AddWithValue("@stock", oProducto.Stock);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    // Ejecuta y obtiene el ID generado
                    idGenerado = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception)
                {
                    idGenerado = 0;
                }
            }
            return idGenerado;
        }
    }
}