using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebPractice.Models;

using System.Data;
using System.Data.SqlClient;

using ClosedXML.Excel;
using System.IO;

namespace WebPractice.Controllers
{
    public class HomeController : Controller
    {
        private readonly string DefaultConnection;

        public HomeController(IConfiguration config)
        {
            DefaultConnection = config.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Exportar_excel()
        {
            DataTable tabla_usuario = new DataTable();

            using (var conexion = new SqlConnection(DefaultConnection))
            {
                conexion.Open();
                using (var adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = new SqlCommand("sp_reporte_usuario", conexion);
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                    adapter.Fill(tabla_usuario);
                }
            }

            using (var libro = new XLWorkbook())
            {
                tabla_usuario.TableName = "Usuarios";
                var hoja = libro.Worksheets.Add(tabla_usuario);
                hoja.ColumnsUsed().AdjustToContents();

                using (var memoria = new MemoryStream())
                {
                    libro.SaveAs(memoria);

                    var nombreExcel = string.Concat("Reporte usuario ", DateTime.Now.ToString(), ".xlsx");

                    return File(memoria.ToArray(), "application/vdn.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }

            }

            //return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
