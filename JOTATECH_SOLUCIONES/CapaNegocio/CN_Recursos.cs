using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Security.Cryptography;
using System.Text;

namespace JOTATECH_SOLUCIONES.CapaNegocio
{
    public class CN_Recursos
    {
        //para convertir y hashear a s sha256
        public static string ConvertirSha256(string texto) {
        
                StringBuilder sb = new StringBuilder(); 
                using(SHA256 hash= SHA256Managed.Create())
                {
                    Encoding enc = Encoding.UTF8;
                    byte[] result =hash.ComputeHash(enc.GetBytes(texto));

                    foreach(byte b in result)
                    sb.Append(b.ToString("x2"));
                }
            return sb.ToString();        
        }
    }
}