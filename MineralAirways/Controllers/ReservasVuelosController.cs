using ClosedXML.Excel;
using MineralAirways.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MineralAirways.Controllers
{
    public class ReservasVuelosController : BaseController
    {
        private DAPEntities db = new DAPEntities();

        // GET: ReservasVuelos

        //public ActionResult Manifiesto(string sortOrder, string currentFilter, string searchString, int? page)
        //{
        //    ViewBag.CurrentSort = sortOrder;
        //    ViewBag.NumVueloSortParm = String.IsNullOrEmpty(sortOrder) ? "numvuelo_desc" : "";
        //    //ViewBag.FechaHoraSalidaSortParm = sortOrder == "Date" ? "fechahorasalida_desc" : "Date";
        //    ViewBag.RutSortParm = String.IsNullOrEmpty(sortOrder) ? "rut_desc" : "";
        //    ViewBag.AsientoSortParm = String.IsNullOrEmpty(sortOrder) ? "asiento_desc" : "";
            
        //    if (searchString != null)
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }

        //    ViewBag.CurrentFilter = searchString;
        //    var reservasVuelos = from s in db.ReservasVuelos.Include(r => r.Asientos).Include(r => r.Aviones).Include(r => r.Pasajeros).Include(r => r.Vuelos).Include(r=> r.Tramos)
        //                         select s;
        //    //if (!String.IsNullOrEmpty(searchString))
        //    //{
        //    //    reservasVuelos = reservasVuelos.Where(s=>s.Vuelos.NumeroVuelo.)||
        //    //}
        //    switch (sortOrder)
        //    {
        //        case "numvuelo_desc":
        //            reservasVuelos = reservasVuelos.OrderByDescending(s => s.Vuelos.NumeroVuelo);
        //            break;
        //        //case "fechahorasalida_desc":
        //        //    reservasVuelos = reservasVuelos.OrderByDescending(s => s.Vuelos.FechaHoraSalida);
        //        //    break;
        //        case "rut_desc":
        //            reservasVuelos = reservasVuelos.OrderByDescending(s => s.Pasajeros.Rut);
        //            break;
        //        case "asiento_desc":
        //            reservasVuelos = reservasVuelos.OrderByDescending(s => s.Asientos.Descripcion);
        //            break;
        //        //case "cargo_desc":
        //        //    pasajeros = pasajeros.OrderByDescending(s => s.Cargo);
        //        //    break;
        //        //case "mail_desc":
        //        //    pasajeros = pasajeros.OrderByDescending(s => s.EMail);
        //        //    break;
        //        //case "asiento_desc":
        //        //    pasajeros = pasajeros.OrderByDescending(s => s.AsientoAsignado);
        //        //    break;
        //        //default:  // Name ascending 
        //        //    pasajeros = pasajeros.OrderBy(s => s.Rut);
        //        //    break;
        //    }
        //    int pageSize = 10;
        //    int pageNumber = (page ?? 1);
        //    return View(reservasVuelos.ToPagedList(pageNumber, pageSize));
        //}
        public ActionResult Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.RutSortParm = String.IsNullOrEmpty(sortOrder) ? "rut_desc" : "";
            ViewBag.NumVueloSortParm = String.IsNullOrEmpty(sortOrder) ? "numvuelo_desc" : "";
            ViewBag.AsientoSortParm = String.IsNullOrEmpty(sortOrder) ? "asiento_desc" : "";
            ViewBag.SalidaVueloSortParm = sortOrder == "FechaSalida" ? "fechasalidavuelo_desc" : "FechaSalida";
            ViewBag.ConfirmacionVueloSortParm = sortOrder == "FechaConfirmacion" ? "fechaconfirmacionvuelo_desc" : "FechaConfirmacion";
            ViewBag.CancelacionVueloSortParm = sortOrder == "FechaCancelacion" ? "fechacancelacionvuelo_desc" : "FechaCancelacion";
            var reservasvuelos = from s in db.ReservasVuelos.Include(r => r.Asientos).Include(r => r.Aviones).Include(r => r.Pasajeros).Include(r => r.Vuelos)
                                 select s;
            
            switch (sortOrder)
            {
                case "name_desc":
                    reservasvuelos = reservasvuelos.OrderByDescending(s => s.Pasajeros.Nombres);
                    break;
                case "rut_desc":
                    reservasvuelos = reservasvuelos.OrderByDescending(s => s.Pasajeros.Rut);
                    break;
                case "asiento_desc":
                    reservasvuelos = reservasvuelos.OrderBy(s => s.Asientos.AsientoID);
                    break;
                case "numvuelo_desc":
                    reservasvuelos = reservasvuelos.OrderByDescending(s => s.Vuelos.NumeroVuelo);
                    break;
                case "FechaSalida":
                    reservasvuelos = reservasvuelos.OrderBy(s => s.Vuelos.FechaHoraSalida);
                    break;
                case "fechasalidavuelo_desc":
                    reservasvuelos = reservasvuelos.OrderByDescending(s => s.Vuelos.FechaHoraSalida);
                    break;
                case "fechaConfirmacion":
                    reservasvuelos = reservasvuelos.OrderBy(s => s.FechaHoraConfirmación);
                    break;
                case "fechaconfirmacionvuelo_desc":
                    reservasvuelos = reservasvuelos.OrderByDescending(s => s.FechaHoraConfirmación);
                    break;
                case "fechaCancelacion":
                    reservasvuelos = reservasvuelos.OrderBy(s => s.FechaHoraCancelacion);
                    break;
                case "fechacancelacionvuelo_desc":
                    reservasvuelos = reservasvuelos.OrderByDescending(s => s.FechaHoraCancelacion);
                    break;
                default:
                    reservasvuelos = reservasvuelos.OrderBy(s => s.Pasajeros.Nombres);
                    break;
            }
            //var reservasVuelos = db.ReservasVuelos.Include(r => r.Asientos).Include(r => r.Aviones).Include(r => r.Pasajeros).Include(r => r.Vuelos);
            return View(reservasvuelos.ToList());            
        }

        // GET: ReservasVuelos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReservasVuelos reservasVuelos = db.ReservasVuelos.Find(id);
            if (reservasVuelos == null)
            {
                return HttpNotFound();
            }
            return View(reservasVuelos);
        }

        // GET: ReservasVuelos/Create
        public ActionResult Create()
        {
            ViewBag.AsientoID = new SelectList(db.Asientos, "AsientoID", "Descripcion");
            ViewBag.AvionID = new SelectList(db.Aviones, "AvionID", "MatriculaDelAvion");
            ViewBag.PasajeroID = new SelectList(db.Pasajeros, "PasajeroID", "Rut");
            ViewBag.VueloID = new SelectList(db.Vuelos, "VueloID", "NumeroVuelo");            
            return View();
        }

        // POST: ReservasVuelos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReservaVueloID,TramoID,AvionID,PasajeroID,ConfirmacionAsiento,FechaHoraConfirmación,FechaHoraCancelacion,VueloID,AsientoID")] ReservasVuelos reservasVuelos)
        {
            if (ModelState.IsValid)
            {
                db.ReservasVuelos.Add(reservasVuelos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AsientoID = new SelectList(db.Asientos, "AsientoID", "Descripcion", reservasVuelos.AsientoID);
            ViewBag.AvionID = new SelectList(db.Aviones, "AvionID", "MatriculaDelAvion", reservasVuelos.AvionID);
            ViewBag.PasajeroID = new SelectList(db.Pasajeros, "PasajeroID", "Rut", reservasVuelos.PasajeroID);
            ViewBag.VueloID = new SelectList(db.Vuelos, "VueloID", "NumeroVuelo", reservasVuelos.VueloID);            
            return View(reservasVuelos);
        }

        // GET: ReservasVuelos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReservasVuelos reservasVuelos = db.ReservasVuelos.Find(id);
            if (reservasVuelos == null)
            {
                return HttpNotFound();
            }
            ViewBag.AsientoID = new SelectList(db.Asientos, "AsientoID", "ColumnaAsiento", reservasVuelos.AsientoID);
            ViewBag.AvionID = new SelectList(db.Aviones, "AvionID", "MatriculaDelAvion", reservasVuelos.AvionID);
            ViewBag.PasajeroID = new SelectList(db.Pasajeros, "PasajeroID", "Rut", reservasVuelos.PasajeroID);
            ViewBag.VueloID = new SelectList(db.Vuelos, "VueloID", "VueloID", reservasVuelos.VueloID);            
            return View(reservasVuelos);
        }

        // POST: ReservasVuelos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReservaVueloID,TramoID,AvionID,PasajeroID,ConfirmacionAsiento,FechaHoraConfirmación,FechaHoraCancelacion,VueloID,AsientoID")] ReservasVuelos reservasVuelos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservasVuelos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AsientoID = new SelectList(db.Asientos, "AsientoID", "ColumnaAsiento", reservasVuelos.AsientoID);
            ViewBag.AvionID = new SelectList(db.Aviones, "AvionID", "MatriculaDelAvion", reservasVuelos.AvionID);
            ViewBag.PasajeroID = new SelectList(db.Pasajeros, "PasajeroID", "Rut", reservasVuelos.PasajeroID);
            ViewBag.VueloID = new SelectList(db.Vuelos, "VueloID", "VueloID", reservasVuelos.VueloID);            
            return View(reservasVuelos);
        }

        // GET: ReservasVuelos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReservasVuelos reservasVuelos = db.ReservasVuelos.Find(id);
            if (reservasVuelos == null)
            {
                return HttpNotFound();
            }
            return View(reservasVuelos);
        }

        // POST: ReservasVuelos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReservasVuelos reservasVuelos = db.ReservasVuelos.Find(id);
            db.ReservasVuelos.Remove(reservasVuelos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DescargarPlanilla()
        {
            ViewBag.processToken = Convert.ToInt64(DateTime.Now.Hour.ToString() + DateTime.Now.Minute + DateTime.Now.Second +
                                       DateTime.Now.Millisecond);
            return View(new ReservaVueloFiltroPlanillaVm { FechaHoraSalida = DateTime.Now });
        }

        public FileStreamResult ExportarExcel(int? numeroVuelo, string fechaSalida, long processToken)
        {
            try
            {
                MakeProcessTokenCookie(processToken);

                DateTime? fechaDate = new DateTime();
                if (!string.IsNullOrEmpty(fechaSalida)) {
                    var format = "dd-MM-yyyy HH:mm";
                    fechaDate = DateTime.ParseExact(fechaSalida, format, CultureInfo.InvariantCulture);
                }
                                
                var libro = ExportarArchivoExcel(numeroVuelo, fechaDate.ToString() == "01-01-0001 0:00:00" ? null : fechaDate);
                var memoryStream = new MemoryStream();
                libro.SaveAs(memoryStream);
                libro.Dispose();

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                memoryStream.Flush();
                memoryStream.Position = 0;

                var nombreArchivo = string.Format("SMA_ReservasVuelos_{0}.xlsx", "Collahuasi");
                return File(memoryStream, "ReservasVuelos", nombreArchivo);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private XLWorkbook ExportarArchivoExcel(int? numeroVuelo, DateTime? fechaSalida = null)
        {
            using (var workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                using (var hojaReservasVuelo = workbook.Worksheets.Add("Datos ReservasVuelos"))
                {
                    //var hojaParam = workbook.Worksheets.Add("Parametros").Hide();
                    //hojaParam.Columns().AdjustToContents();

                    hojaReservasVuelo.Cell(1, 1).Value = "N°";
                    hojaReservasVuelo.Cell(1, 1).Style.Font.FontColor = XLColor.White;
                    hojaReservasVuelo.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.Blue;
                    hojaReservasVuelo.Cell(1, 1).Style.Font.Bold = true;

                    hojaReservasVuelo.Cell(1, 2).Value = "RUT";
                    hojaReservasVuelo.Cell(1, 2).Style.Font.FontColor = XLColor.White;
                    hojaReservasVuelo.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.Blue;
                    hojaReservasVuelo.Cell(1, 2).Style.Font.Bold = true;

                    hojaReservasVuelo.Cell(1, 3).Value = "Cargo";
                    hojaReservasVuelo.Cell(1, 3).Style.Font.FontColor = XLColor.White;
                    hojaReservasVuelo.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.Blue;
                    hojaReservasVuelo.Cell(1, 3).Style.Font.Bold = true;

                    hojaReservasVuelo.Cell(1, 4).Value = "Nombre del Pasajero";
                    hojaReservasVuelo.Cell(1, 4).Style.Font.FontColor = XLColor.White;
                    hojaReservasVuelo.Cell(1, 4).Style.Fill.BackgroundColor = XLColor.Blue;
                    hojaReservasVuelo.Cell(1, 4).Style.Font.Bold = true;

                    hojaReservasVuelo.Cell(1, 5).Value = "Asiento";
                    hojaReservasVuelo.Cell(1, 5).Style.Font.FontColor = XLColor.White;
                    hojaReservasVuelo.Cell(1, 5).Style.Fill.BackgroundColor = XLColor.Blue;
                    hojaReservasVuelo.Cell(1, 5).Style.Font.Bold = true;

                    hojaReservasVuelo.Cell(1, 6).Value = "Fecha y Hora de Salida";
                    hojaReservasVuelo.Cell(1, 6).Style.Font.FontColor = XLColor.White;
                    hojaReservasVuelo.Cell(1, 6).Style.Fill.BackgroundColor = XLColor.Blue;
                    hojaReservasVuelo.Cell(1, 6).Style.Font.Bold = true;

                    hojaReservasVuelo.Cell(1, 7).Value = "Número de Vuelo";
                    hojaReservasVuelo.Cell(1, 7).Style.Font.FontColor = XLColor.White;
                    hojaReservasVuelo.Cell(1, 7).Style.Fill.BackgroundColor = XLColor.Blue;
                    hojaReservasVuelo.Cell(1, 7).Style.Font.Bold = true;

                    hojaReservasVuelo.Cell(1, 8).Value = "Vuelo";
                    hojaReservasVuelo.Cell(1, 8).Style.Font.FontColor = XLColor.White;
                    hojaReservasVuelo.Cell(1, 8).Style.Fill.BackgroundColor = XLColor.Blue;
                    hojaReservasVuelo.Cell(1, 8).Style.Font.Bold = true;

                    hojaReservasVuelo.Cell(1, 9).Value = "Dirección";
                    hojaReservasVuelo.Cell(1, 9).Style.Font.FontColor = XLColor.White;
                    hojaReservasVuelo.Cell(1, 9).Style.Fill.BackgroundColor = XLColor.Blue;
                    hojaReservasVuelo.Cell(1, 9).Style.Font.Bold = true;

                    using (var entity = new DAPEntities())
                    {
                        numeroVuelo = numeroVuelo == 0 ? null : numeroVuelo;

                        var reservasvuelosBd = entity.ReservasVuelos.Where(r => r.ConfirmacionAsiento == true
                        && (numeroVuelo == null || r.Vuelos.NumeroVuelo == numeroVuelo)
                        && (fechaSalida == null || r.Vuelos.FechaHoraSalida == fechaSalida)
                        ).ToList();

                        var fila = 2;
                        var contadorReg = 1;
                        foreach (ReservasVuelos reserVItem in reservasvuelosBd.OrderBy(x => x.Pasajeros.PrimerApellido))
                        {
                            hojaReservasVuelo.Cell(fila, 1).DataType = XLCellValues.Number;
                            hojaReservasVuelo.Cell(fila, 1).Value = contadorReg;

                            hojaReservasVuelo.Cell(fila, 2).DataType = XLCellValues.Text;
                            hojaReservasVuelo.Cell(fila, 2).Value = reserVItem.Pasajeros.Rut;

                            hojaReservasVuelo.Cell(fila, 3).DataType = XLCellValues.Text;
                            hojaReservasVuelo.Cell(fila, 3).Value = reserVItem.Pasajeros.Cargo;

                            hojaReservasVuelo.Cell(fila, 4).DataType = XLCellValues.Text;
                            hojaReservasVuelo.Cell(fila, 4).Value = reserVItem.Pasajeros.PrimerApellido + " " + reserVItem.Pasajeros.SegundoApellido + ", " + reserVItem.Pasajeros.Nombres;

                            hojaReservasVuelo.Cell(fila, 5).DataType = XLCellValues.Text;
                            hojaReservasVuelo.Cell(fila, 5).Value = reserVItem.Asientos.Descripcion;

                            hojaReservasVuelo.Cell(fila, 6).DataType = XLCellValues.Text;
                            hojaReservasVuelo.Cell(fila, 6).Value = reserVItem.Tramos.FechaHoraSalida;

                            hojaReservasVuelo.Cell(fila, 7).DataType = XLCellValues.Text;
                            hojaReservasVuelo.Cell(fila, 7).Value = reserVItem.Vuelos.NumeroVuelo;

                            hojaReservasVuelo.Cell(fila, 8).DataType = XLCellValues.Text;
                            hojaReservasVuelo.Cell(fila, 8).Value = reserVItem.Vuelos.Rutas1.Codigo + "-" + reserVItem.Vuelos.Rutas.Codigo;

                            hojaReservasVuelo.Cell(fila, 9).DataType = XLCellValues.Text;
                            hojaReservasVuelo.Cell(fila, 9).Value = reserVItem.Tramos.Rutas1.Ciudad + "-" + reserVItem.Tramos.Rutas.Ciudad;

                            fila++;
                            contadorReg++;
                        }
                    }

                    //Finalizar archivo Excel
                    hojaReservasVuelo.Columns().AdjustToContents();
                }               

                return workbook;
            }            
        }

        public FileStreamResult ExportarExcelCancelado(int numeroVuelo, string fechaInicio, string fechaFin, long processToken)
        {
            try
            {
                MakeProcessTokenCookie(processToken);

                var format = "dd-MM-yyyy HH:mm";
                var fechaDateInicio = new DateTime();
                var fechaDateFin = new DateTime();
                             
                fechaDateInicio = DateTime.ParseExact(fechaInicio, format, CultureInfo.InvariantCulture);
                fechaDateFin = DateTime.ParseExact(fechaFin, format, CultureInfo.InvariantCulture);               

                var libro = ExportarArchivoExcelCancelado(numeroVuelo, fechaDateInicio, fechaDateFin);
                var memoryStream = new MemoryStream();
                libro.SaveAs(memoryStream);
                libro.Dispose();

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                memoryStream.Flush();
                memoryStream.Position = 0;

                var nombreArchivo = string.Format("SMA_ReservasVuelosCancelados_{0}.xlsx", "Collahuasi");
                return File(memoryStream, "ReservasVuelos", nombreArchivo);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private XLWorkbook ExportarArchivoExcelCancelado(int numeroVuelo, DateTime fechaInicio, DateTime fechaFin)
        {
            using (var workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                using (var hojaReservasVuelo = workbook.Worksheets.Add("Datos ReservasVuelos Cancelados"))
                {
                    //var hojaParam = workbook.Worksheets.Add("Parametros").Hide();
                    //hojaParam.Columns().AdjustToContents();

                    hojaReservasVuelo.Cell(1, 1).Value = "N°";
                    hojaReservasVuelo.Cell(1, 1).Style.Font.FontColor = XLColor.White;
                    hojaReservasVuelo.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.Blue;
                    hojaReservasVuelo.Cell(1, 1).Style.Font.Bold = true;

                    hojaReservasVuelo.Cell(1, 2).Value = "RUT";
                    hojaReservasVuelo.Cell(1, 2).Style.Font.FontColor = XLColor.White;
                    hojaReservasVuelo.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.Blue;
                    hojaReservasVuelo.Cell(1, 2).Style.Font.Bold = true;

                    hojaReservasVuelo.Cell(1, 3).Value = "Cargo";
                    hojaReservasVuelo.Cell(1, 3).Style.Font.FontColor = XLColor.White;
                    hojaReservasVuelo.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.Blue;
                    hojaReservasVuelo.Cell(1, 3).Style.Font.Bold = true;

                    hojaReservasVuelo.Cell(1, 4).Value = "Nombre del Pasajero";
                    hojaReservasVuelo.Cell(1, 4).Style.Font.FontColor = XLColor.White;
                    hojaReservasVuelo.Cell(1, 4).Style.Fill.BackgroundColor = XLColor.Blue;
                    hojaReservasVuelo.Cell(1, 4).Style.Font.Bold = true;

                    hojaReservasVuelo.Cell(1, 5).Value = "Asiento";
                    hojaReservasVuelo.Cell(1, 5).Style.Font.FontColor = XLColor.White;
                    hojaReservasVuelo.Cell(1, 5).Style.Fill.BackgroundColor = XLColor.Blue;
                    hojaReservasVuelo.Cell(1, 5).Style.Font.Bold = true;

                    hojaReservasVuelo.Cell(1, 6).Value = "Fecha y Hora de Salida";
                    hojaReservasVuelo.Cell(1, 6).Style.Font.FontColor = XLColor.White;
                    hojaReservasVuelo.Cell(1, 6).Style.Fill.BackgroundColor = XLColor.Blue;
                    hojaReservasVuelo.Cell(1, 6).Style.Font.Bold = true;

                    hojaReservasVuelo.Cell(1, 7).Value = "Fecha y Hora de Cancelación";
                    hojaReservasVuelo.Cell(1, 7).Style.Font.FontColor = XLColor.White;
                    hojaReservasVuelo.Cell(1, 7).Style.Fill.BackgroundColor = XLColor.Blue;
                    hojaReservasVuelo.Cell(1, 7).Style.Font.Bold = true;

                    hojaReservasVuelo.Cell(1, 8).Value = "Número de Vuelo";
                    hojaReservasVuelo.Cell(1, 8).Style.Font.FontColor = XLColor.White;
                    hojaReservasVuelo.Cell(1, 8).Style.Fill.BackgroundColor = XLColor.Blue;
                    hojaReservasVuelo.Cell(1, 8).Style.Font.Bold = true;

                    hojaReservasVuelo.Cell(1, 9).Value = "Vuelo";
                    hojaReservasVuelo.Cell(1, 9).Style.Font.FontColor = XLColor.White;
                    hojaReservasVuelo.Cell(1, 9).Style.Fill.BackgroundColor = XLColor.Blue;
                    hojaReservasVuelo.Cell(1, 9).Style.Font.Bold = true;

                    hojaReservasVuelo.Cell(1, 10).Value = "Dirección";
                    hojaReservasVuelo.Cell(1, 10).Style.Font.FontColor = XLColor.White;
                    hojaReservasVuelo.Cell(1, 10).Style.Fill.BackgroundColor = XLColor.Blue;
                    hojaReservasVuelo.Cell(1, 10).Style.Font.Bold = true;

                    using (var entity = new DAPEntities())
                    {
                        //Generar query
                        var reservasvuelosBd = entity.ReservasVuelos.Where(r => r.ConfirmacionAsiento == false
                        && r.Vuelos.NumeroVuelo == numeroVuelo
                        && r.Vuelos.FechaHoraSalida >= fechaInicio
                        && r.Vuelos.FechaHoraSalida <= fechaFin
                        ).ToList();

                        var fila = 2;
                        var contadorReg = 1;
                        foreach (ReservasVuelos reserVItem in reservasvuelosBd.OrderBy(x => x.Pasajeros.PrimerApellido).ThenBy(y => y.Vuelos.FechaHoraSalida))
                        {
                            hojaReservasVuelo.Cell(fila, 1).DataType = XLCellValues.Number;
                            hojaReservasVuelo.Cell(fila, 1).Value = contadorReg;

                            hojaReservasVuelo.Cell(fila, 2).DataType = XLCellValues.Text;
                            hojaReservasVuelo.Cell(fila, 2).Value = reserVItem.Pasajeros.Rut;

                            hojaReservasVuelo.Cell(fila, 3).DataType = XLCellValues.Text;
                            hojaReservasVuelo.Cell(fila, 3).Value = reserVItem.Pasajeros.Cargo;

                            hojaReservasVuelo.Cell(fila, 4).DataType = XLCellValues.Text;
                            hojaReservasVuelo.Cell(fila, 4).Value = reserVItem.Pasajeros.PrimerApellido + " " + reserVItem.Pasajeros.SegundoApellido + ", " + reserVItem.Pasajeros.Nombres;

                            hojaReservasVuelo.Cell(fila, 5).DataType = XLCellValues.Text;
                            hojaReservasVuelo.Cell(fila, 5).Value = reserVItem.Asientos.Descripcion;

                            hojaReservasVuelo.Cell(fila, 6).DataType = XLCellValues.Text;
                            hojaReservasVuelo.Cell(fila, 6).Value = reserVItem.Tramos.FechaHoraSalida;

                            hojaReservasVuelo.Cell(fila, 7).DataType = XLCellValues.Text;
                            hojaReservasVuelo.Cell(fila, 7).Value = reserVItem.FechaHoraCancelacion;

                            hojaReservasVuelo.Cell(fila, 8).DataType = XLCellValues.Text;
                            hojaReservasVuelo.Cell(fila, 8).Value = reserVItem.Vuelos.NumeroVuelo;

                            hojaReservasVuelo.Cell(fila, 9).DataType = XLCellValues.Text;
                            hojaReservasVuelo.Cell(fila, 9).Value = reserVItem.Vuelos.Rutas1.Codigo + "-" + reserVItem.Vuelos.Rutas.Codigo;

                            hojaReservasVuelo.Cell(fila, 10).DataType = XLCellValues.Text;
                            hojaReservasVuelo.Cell(fila, 10).Value = reserVItem.Tramos.Rutas1.Ciudad + "-" + reserVItem.Tramos.Rutas.Ciudad;

                            fila++;
                            contadorReg++;
                        }
                    }
                    //Finalizar archivo Excel
                    hojaReservasVuelo.Columns().AdjustToContents();
                } 
                

                return workbook;
            }            
        }

        public ActionResult DescargarPlanillaCancelado()
        {
            ViewBag.processToken = Convert.ToInt64(DateTime.Now.Hour.ToString() + DateTime.Now.Minute + DateTime.Now.Second +
                                       DateTime.Now.Millisecond);
            return View(new ReservaVueloFiltroPlanillaVm { FechaHoraInicio = DateTime.Now.AddMonths(-1),
            FechaHoraFin = DateTime.Now.AddMonths(1)});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void MakeProcessTokenCookie(long processToken)
        {
            var cookie = new HttpCookie("processToken")
            {
                Value = processToken.ToString()
            };
            ControllerContext.HttpContext.Response.Cookies.Add(cookie);
        }
    }
}
