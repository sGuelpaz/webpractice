using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;
using WebPractice.Data;
using WebPractice.Models;

namespace WebPractice.Controllers
{
    public class RegistrosController : Controller
    {
        private byte[] GetByteArrayFromImage(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RegistrosController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Registros
        public async Task<IActionResult> Index()
        {
            return View(await _context.Registro.ToListAsync());
        }

        public async Task<IActionResult> Disableds()
        {
            return View(await _context.Registro.ToListAsync());
        }

        // GET: Registros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registros = await _context.Registro
                .FirstOrDefaultAsync(m => m.IdRegistro == id);
            if (registros == null)
            {
                return NotFound();
            }

            return View(registros);
        }

        // GET: Registros/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Registros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
       Create([Bind("IdRegistro,Imagen,Documento,Nombre,Apellidos,FechaNac,Direccion,Edad,Celular,Genero,Deporte,Trabaja,Sueldo,Estado,FechaReg")] Registros registros, IFormFile ImageFile)
        {
            //if (ModelState.IsValid)
            if (ImageFile != null && ImageFile.Length > 0)
            {
                registros.Imagen = GetByteArrayFromImage(ImageFile);
                _context.Add(registros);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(registros);
        }


        // GET: Registros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registros = await _context.Registro.FindAsync(id);
            if (registros == null)
            {
                return NotFound();
            }
            return View(registros);
        }

        // POST: Registros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRegistro,Imagen,Documento,Nombre,Apellidos,FechaNac,Direccion,Edad,Celular,Genero,Deporte,Trabaja,Sueldo,Estado,FechaReg")] Registros registros)
        {
            if (id != registros.IdRegistro)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registros);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrosExists(registros.IdRegistro))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(registros);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id,
        //    [Bind("IdRegistro,Imagen,Documento,Nombre,Apellidos,FechaNac,Direccion,Celular,Genero,Deporte,Trabaja,Sueldo,Estado")] Registros registros, IFormFile ImageFile)
        //{
        //    if (id != registros.IdRegistro)
        //    {
        //        return NotFound();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        if ((ImageFile != null) || (registros.Imagen.Length > 0))
        //        {
        //            try
        //            {
        //                if (ImageFile != null && ImageFile.Length > 0)
        //                {
        //                    registros.Imagen = GetByteArrayFromImage(ImageFile);
        //                    _context.Update(registros);
        //                }
        //                await _context.SaveChangesAsync();
        //                return RedirectToAction(nameof(Index));
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (!RegistrosExists(registros.IdRegistro))
        //                {
        //                    return NotFound();
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //        }
        //    }
        //    return View(registros);
        //}


        // GET: Registros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registros = await _context.Registro
                .FirstOrDefaultAsync(m => m.IdRegistro == id);
            if (registros == null)
            {
                return NotFound();
            }

            return View(registros);
        }

        // POST: Registros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registros = await _context.Registro.FindAsync(id);
            _context.Registro.Remove(registros);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistrosExists(int id)
        {
            return _context.Registro.Any(e => e.IdRegistro == id);
        }

        public IActionResult Imprimir()
        {
            string renderFormat = "PDF";
            string mimetype = "application/PDF";
            using var report = new LocalReport();
            // Ruta donde está el archivo rdcl
            report.ReportPath =
           $"{this._webHostEnvironment.WebRootPath}\\Reportes\\ReportGeneral.rdlc";
            DataSet.DataSet1 ds = new DataSet.DataSet1();
            DataSet.DataSet1TableAdapters.RegistroTableAdapter sda = new
           DataSet.DataSet1TableAdapters.RegistroTableAdapter();
            sda.Fill(ds.Registro);
            ReportDataSource rds = new ReportDataSource("DataSetRegistros",
           (object)ds.Registro);
            report.DataSources.Add(rds);
            //report.Refresh();
            // Convierte el archivo en binario
            var pdf = report.Render(renderFormat);
            report.Refresh();
            // Descarga el documento en un archivo pdf
            //return File(pdf,mimetype,"report." + "pdf");
            // Muestra el reporte en la pantalla
            return File(pdf, mimetype);
        }

        public IActionResult DescargarPdf()
        {
            string renderFormat = "PDF";
            string mimetype = "application/PDF";
            using var report = new LocalReport();
            // Ruta donde está el archivo rdcl
            report.ReportPath =
           $"{this._webHostEnvironment.WebRootPath}\\Reportes\\ReportGeneral.rdlc";
            DataSet.DataSet1 ds = new DataSet.DataSet1();
            DataSet.DataSet1TableAdapters.RegistroTableAdapter sda = new
           DataSet.DataSet1TableAdapters.RegistroTableAdapter();
            sda.Fill(ds.Registro);
            ReportDataSource rds = new ReportDataSource("DataSetRegistros",
           (object)ds.Registro);
            report.DataSources.Add(rds);
            report.Refresh();
            // Convierte el archivo en binario
            var pdf = report.Render(renderFormat);
            // Descarga el documento en un archivo pdf
            return File(pdf, mimetype, "report." + "pdf");
            // Muestra el reporte en la pantalla
            //return File(pdf, mimetype);
        }

        public ActionResult Graficas()
        {
            return View();
        }

        [HttpGet]
        public JsonResult DatosPersonas()
        {
            List<Registros> objLista = new List<Registros>();
            var conn = _context.Database.GetDbConnection();
            try
            {
                using (var command = conn.CreateCommand())
                {
                    conn.Open();
                    string query1 = "Select Nombre,edad From Registro";
                    command.CommandText = query1;
                    DbDataReader reader1 = command.ExecuteReader();
                    while (reader1.Read())
                        {
                        objLista.Add(new Registros()
                        {
                            Nombre = reader1["Nombre"].ToString(),
                            Edad = int.Parse(reader1["Edad"].ToString()),
                        });
                    }
                    conn.Close();
                }
            }
            catch (Exception e1)
            {
                string error = e1.ToString();
            }
            finally
            {
                conn.Close();
            }
            return Json(objLista);
        }
    }
}
