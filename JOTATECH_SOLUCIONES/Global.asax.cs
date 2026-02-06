using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Security.Principal;

// ===========================================================================
// USINGS PARA EPPLUS Y EL MODELO (Importante: LicenseContext está aquí)
// ===========================================================================
using OfficeOpenXml;
using JOTATECH_SOLUCIONES.Models;
// ===========================================================================

namespace JOTATECH_SOLUCIONES
{
    public class MvcApplication : System.Web.HttpApplication
    {

        // =======================================================================
        // LÓGICA DE SEGURIDAD BASADA EN ROLES 
        // =======================================================================
        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (HttpContext.Current.Session != null)
                {
                    string rol = (string)HttpContext.Current.Session["RolUsuario"];

                    if (!string.IsNullOrEmpty(rol))
                    {
                        IIdentity id = User.Identity;
                        GenericPrincipal principal = new GenericPrincipal(id, new string[] { rol });
                        HttpContext.Current.User = principal;
                    }
                }
            }
        }

        // =======================================================================
        // MÉTODOS PREDETERMINADOS DE LA APLICACIÓN
        // =======================================================================
        protected void Application_Start()
        {
            // === CONFIGURACIÓN GLOBAL DE LA LICENCIA DE EPPLUS (SOLUCIÓN CS0200) ===
            // Usamos la propiedad estática 'LicenseContext' y el enum 'NonCommercial'.
            // Esta línea resuelve la excepción al inicio de la aplicación.
            // ========================================================================

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}