using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using JOTATECH_SOLUCIONES.CapaDatos;
using JOTATECH_SOLUCIONES.Models;


namespace JOTATECH_SOLUCIONES.CapaNegocio
{
    public class CN_Usuario
    {
        private CD_Usuario objCapaDato = new CD_Usuario();

        public Usuario ValidarUsuario(string usuario, string clave){
            //encripto claves antes de buscar a la bd
            string claveEncriptada = CN_Recursos.ConvertirSha256(clave);

            //llamar a la capa datos con la clave ya encriptada
            return objCapaDato.ValidarUsuario(usuario, claveEncriptada);
         }
                     
    }
}