using JOTATECH_SOLUCIONES.CapaDatos;
using JOTATECH_SOLUCIONES.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Web.Mvc;
using OfficeOpenXml; // Asegúrate de tener este using

[Authorize] // Aseguramos que solo usuarios logeados puedan acceder
public class ReporteController : Controller
{
    // GET: Reporte/Index (Vista principal de reportes)
    public ActionResult Index()
    {
        // La vista inicialmente no tendrá datos
        return View(new List<Venta>());
    }

    // POST: Reporte/GenerarReporte (Muestra el listado en la vista)
    [HttpPost]
    public ActionResult GenerarReporte(string fechaInicio, string fechaFin)
    {
        // 1. Convertir las cadenas de fecha (desde el formulario HTML) a DateTime
        if (DateTime.TryParse(fechaInicio, out DateTime fInicio) && DateTime.TryParse(fechaFin, out DateTime fFin))
        {
            // 2. Llamar a la capa de datos para obtener el reporte
            List<Venta> reporte = new CD_Reporte().ReporteVentasPorFecha(fInicio, fFin);

            // 3. Pasar el reporte a la vista
            ViewBag.FechaInicio = fInicio.ToShortDateString();
            ViewBag.FechaFin = fFin.ToShortDateString();

            // Retornamos la vista Index, pero con la lista de ventas como modelo
            return View("Index", reporte);
        }

        // Si las fechas son inválidas, se vuelve a la vista con un modelo vacío.
        ViewBag.Error = "Formato de fecha inválido.";
        return View("Index", new List<Venta>());
    }

    // POST: Reporte/ExportarExcel (Genera y descarga el archivo XLSX)
    [HttpPost]
    public ActionResult ExportarExcel(string fechaInicio, string fechaFin)
    {
        // 1. Validar y convertir fechas
        if (!DateTime.TryParse(fechaInicio, out DateTime fInicio) || !DateTime.TryParse(fechaFin, out DateTime fFin))
        {
            // Si las fechas son inválidas, redirige sin exportar.
            return RedirectToAction("Index");
        }

        // 2. Obtener los datos DETALLADOS
        List<ReporteDetalle> reporte = new CD_Reporte().ObtenerReporteDetallado(fInicio, fFin);

        // Si no hay datos, redirige
        if (reporte == null || reporte.Count == 0)
        {
            return RedirectToAction("Index");
        }

        // 3. Configurar EPPlus y generar el archivo

        // La configuración de la licencia DEBE estar en Global.asax.cs

        // === CÓDIGO DESCOMENTADO Y CORREGIDO ===
        using (ExcelPackage package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("DetalleVentas");

            // --- CABECERA ---
            worksheet.Cells["A1"].Value = "REPORTE DETALLADO DE VENTAS JOTA TECH";
            worksheet.Cells["A2"].Value = $"Periodo: {fInicio.ToShortDateString()} - {fFin.ToShortDateString()}";
            worksheet.Cells["A1:H1"].Merge = true;
            worksheet.Cells["A1"].Style.Font.Bold = true;

            // --- ENCABEZADOS DE COLUMNA (Asegurar que coincidan con el modelo ReporteDetalle) ---
            worksheet.Cells[4, 1].Value = "BOLETA N°";
            worksheet.Cells[4, 2].Value = "FECHA EMISIÓN";
            worksheet.Cells[4, 3].Value = "VENDEDOR";
            worksheet.Cells[4, 4].Value = "COD. PRODUCTO";
            worksheet.Cells[4, 5].Value = "DESCRIPCIÓN";
            worksheet.Cells[4, 6].Value = "CANTIDAD";
            worksheet.Cells[4, 7].Value = "P. UNITARIO";
            worksheet.Cells[4, 8].Value = "TOTAL VENTA"; // Total de la venta (se repite por línea, pero es útil)

            // Estilo de Encabezado
            using (var range = worksheet.Cells[4, 1, 4, 8])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(200, 200, 200)); // Gris claro
            }

            // --- LLENAR DATOS ---
            int row = 5;
            foreach (var item in reporte)
            {
                worksheet.Cells[row, 1].Value = item.NumeroBoleta;
                worksheet.Cells[row, 2].Value = item.FechaVenta.ToString("dd/MM/yyyy HH:mm");
                worksheet.Cells[row, 3].Value = item.Vendedor;
                worksheet.Cells[row, 4].Value = item.CodigoProducto;
                worksheet.Cells[row, 5].Value = item.DescripcionProducto;
                worksheet.Cells[row, 6].Value = item.Cantidad;
                worksheet.Cells[row, 7].Value = item.PrecioUnitario;
                worksheet.Cells[row, 8].Value = item.TotalVenta;

                // Formato de moneda para columnas 7 y 8
                worksheet.Cells[row, 7].Style.Numberformat.Format = "S/ #,##0.00";
                worksheet.Cells[row, 8].Style.Numberformat.Format = "S/ #,##0.00";
                row++;
            }

            // Ajustar ancho de columnas
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            // 4. Devolver el archivo para descarga
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            string excelName = $"Reporte_Detallado_Ventas_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        } // Fin del using (ExcelPackage package = new ExcelPackage())
    }
}