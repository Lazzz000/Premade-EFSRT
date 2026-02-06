using System.Web.Mvc;
using JOTATECH_SOLUCIONES.CapaDatos;
using JOTATECH_SOLUCIONES.Models;
using System.Web.Script.Serialization; // Necesario para serializar a JSON
using System.Collections.Generic;
using System;

[Authorize]
public class VentaController : Controller
{
    // GET: Venta/Crear
    public ActionResult Crear()
    {
        // Se usa la vista Crear para el formulario de nueva venta
        return View();
    }

    // POST: Venta/BuscarProducto (Llamado vía AJAX)
    [HttpPost]
    public JsonResult BuscarProducto(string codigo)
    {
        // 1. Llama a la capa de datos para buscar por código
        Producto producto = new CD_Venta().BuscarProducto(codigo);

        // 2. Prepara la respuesta (objeto anónimo con estado y datos)
        if (producto != null)
        {
            return Json(new { estado = true, data = producto });
        }
        else
        {
            return Json(new { estado = false, data = (Producto)null });
        }
    }

    // POST: Venta/RegistrarVenta (Llamado vía AJAX para guardar la transacción)
    [HttpPost]
    public JsonResult RegistrarVenta(string VentaJSON)
    {
        // 1. Deserializar el JSON recibido a la clase Venta
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        Venta oVenta = serializer.Deserialize<Venta>(VentaJSON);

        // 2. Obtener el ID del usuario activo de la sesión
        Usuario usuarioActivo = (Usuario)Session["UsuarioActivo"];
        oVenta.IdUsuario = usuarioActivo.IdUsuario;

        // 3. Registrar la venta en la base de datos (Transacción)
        Dictionary<string, string> resultado = new CD_Venta().RegistrarVenta(oVenta);

        // 4. Devolver la respuesta al cliente
        if (resultado["IdVenta"] != "0")
        {
            return Json(new { estado = true, idVenta = resultado["IdVenta"], numeroBoleta = resultado["NumeroBoleta"] });
        }
        else
        {
            return Json(new { estado = false, mensaje = "Error al registrar la venta. Verifique el stock." });
        }
    }

    // GET: Venta/Boleta/5 (Vista para imprimir el comprobante)
    public ActionResult Boleta(int id)
    {
        // Llama al método para obtener la venta completa (cabecera + detalle)
        Venta oVenta = new CD_Venta().ObtenerVenta(id);

        if (oVenta == null)
        {
            // Si no se encuentra la venta, redirigir al dashboard o mostrar error
            return RedirectToAction("Index", "Home");
        }

        // Pasar el objeto Venta completo a la vista
        return View(oVenta);
    }
}