using System.Web.Mvc;
using System.Collections.Generic;
// Importamos los namespaces necesarios
using JOTATECH_SOLUCIONES.CapaDatos;
using JOTATECH_SOLUCIONES.Models;

// [Authorize] asegura que solo usuarios logeados puedan acceder a este módulo
[Authorize]
public class ProductoController : Controller
{
    // -----------------------------------------------------
    // 1. LISTADO DE PRODUCTOS (Mapeado a Views/Producto/Index.cshtml)
    // -----------------------------------------------------
    // GET: Producto/Index
    public ActionResult Index()
    {
        // Llama a la capa de datos para obtener la lista
        List<Producto> productos = new CD_Producto().Listar();
        return View(productos);
    }

    // -----------------------------------------------------
    // 2. CREAR / EDITAR (Mapeado a Views/Producto/Guardar.cshtml)
    // -----------------------------------------------------

    // GET: Producto/Guardar (Muestra el formulario, cargando datos si es edición)
    //[Authorize(Roles = "Administrador")] // << NUEVA RESTRICCIÓN DE QUE SOLO ADMINISTRADOR TIENE ACCESO
    public ActionResult Guardar(int id = 0)
    {
        Producto oProducto = new Producto();

        // Si se recibe un ID, intentamos obtener los datos para editar
        if (id != 0)
        {
            oProducto = new CD_Producto().Obtener(id);
        }

        // Si el producto no se encontró, devolvemos un objeto vacío
        if (oProducto == null)
        {
            oProducto = new Producto();
        }

        return View(oProducto);
    }

    // POST: Producto/Guardar (Recibe y procesa el formulario)
    [HttpPost]
    //[Authorize(Roles = "Administrador")] // << NUEVA RESTRICCIÓN DE QUE SOLO ADMINISTRADOR TIENE ACCESO
    public ActionResult Guardar(Producto oProducto)
    {
        // ModelState.IsValid verifica las anotaciones [Required] en el modelo Producto
        if (ModelState.IsValid)
        {
            int resultado = new CD_Producto().Guardar(oProducto);
            if (resultado > 0)
            {
                // Éxito: Redirige al listado
                return RedirectToAction("Index");
            }
            else
            {
                // Fallo en la base de datos (ej. código repetido)
                ViewBag.Error = "Error al guardar el producto. Verifique que el código no esté repetido.";
                return View(oProducto);
            }
        }
        // Si la validación falla, regresa a la vista para mostrar errores
        return View(oProducto);
    }
}