// Archivo: Controllers/AccesoController.cs
using JOTATECH_SOLUCIONES.CapaDatos;
using JOTATECH_SOLUCIONES.Models; // Asegúrate de que esta línea esté, si no, agréglala
using System.Web.Mvc;
using System.Web.Security; // Para FormsAuthentication

using JOTATECH_SOLUCIONES.CapaNegocio;

public class AccesoController : Controller
{
    // GET: Acceso/Login
    [AllowAnonymous]
    public ActionResult Login()
    {
        return View();
    }

    // POST: Acceso/Login
    [HttpPost]
    [AllowAnonymous]
    public ActionResult Login(Usuario oUsuario)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Debe ingresar usuario y contraseña.";
            return View(oUsuario);
        }


        //llamamos a CN_Usuario desde la capa de neggocio para que la logica del negocio maneje la validacion
        
        Usuario usuarioValidado = new CN_Usuario().ValidarUsuario(oUsuario.Username, oUsuario.Clave);

        if (usuarioValidado != null)
        {
            FormsAuthentication.SetAuthCookie(usuarioValidado.Username, false);

            // Guardar datos en la sesión
            Session["UsuarioActivo"] = usuarioValidado;
            Session["RolUsuario"] = usuarioValidado.Rol;

            // Redirigir al Dashboard
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View(oUsuario);
        }
    }

    // GET: Acceso/CerrarSesion
    [Authorize]
    public ActionResult CerrarSesion()
    {
        FormsAuthentication.SignOut();
        Session.Clear();
        Session.Abandon();
        return RedirectToAction("Login", "Acceso");
    }
}