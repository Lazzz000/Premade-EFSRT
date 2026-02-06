using JOTATECH_SOLUCIONES.CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JOTATECH_SOLUCIONES.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            CD_Dashboard oDashboard = new CD_Dashboard();

            // 1. Obtener los valores del Dashboard
            int totalProductos = oDashboard.ObtenerTotalProductos();
            decimal ventaTotalMes = oDashboard.ObtenerVentaTotalDelMes();
            int boletasMes = oDashboard.ObtenerBoletasEmitidasDelMes();

            // 2. Pasar los valores a la vista usando ViewBag
            ViewBag.TotalProductos = totalProductos;
            // Formato de moneda para la vista
            ViewBag.VentaTotalMes = "S/ " + ventaTotalMes.ToString("N2");
            ViewBag.BoletasMes = boletasMes;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}