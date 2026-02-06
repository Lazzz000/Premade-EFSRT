using JOTATECH_SOLUCIONES.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace JOTATECH_SOLUCIONES.CapaDatos
{
    public class CD_Usuario
    {
        private static string conexion = ConfigurationManager.ConnectionStrings["ConexionJotaTech"].ToString();

        public Usuario ValidarUsuario(string usuario, string clave)
        {
            Usuario oUsuario = null;
            using (SqlConnection oConexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_ValidarUsuario", oConexion);
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@clave", clave);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            oUsuario = new Usuario()
                            {
                                IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                Username = dr["Usuario"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                NombreCompleto = dr["NombreCompleto"].ToString(),
                                Rol = dr["Rol"].ToString()
                            };
                        }
                    }
                }
                catch (Exception)
                {
                    oUsuario = null;
                }
            }
            return oUsuario;
        }
    }
}