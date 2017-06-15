using ClosedXML.Excel;
using MineralAirways.Enum;
using MineralAirways.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MineralAirways.Controllers
{
    public class PasajerosController : BaseController
    {
        private DAPEntities db = new DAPEntities();

        // GET: Pasajeros
        public ActionResult Index()
        {            
            var pasajeros = from s in db.Pasajeros                            
                           select s;                                  
            
            return View(pasajeros.ToList());
        }
                

        // GET: Pasajeros/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pasajeros pasajeros = db.Pasajeros.Find(id);
            if (pasajeros == null)
            {
                return HttpNotFound();
            }
            return View(pasajeros);
        }

        // GET: Pasajeros/Create
        public ActionResult Create()
        {
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Rut");
            ViewBag.TipoUsuario = GetSelectTipoUsuario();
            return View();
        }

        // POST: Pasajeros/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PasajeroID,Rut,Nombres,PrimerApellido,SegundoApellido,Cargo,EMail,AsientoAsignado,EsEjecutivo,ClienteID,Pass,TipoUsuario,PrimerLogeo")] Pasajeros pasajeros)
        {
            if (ModelState.IsValid)
            {
                //pasajeros.PrimerLogeo = false;
                db.Pasajeros.Add(pasajeros);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Rut", pasajeros.ClienteID);
            return View(pasajeros);
        }

        // GET: Pasajeros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pasajeros pasajeros = db.Pasajeros.Find(id);
            if (pasajeros == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Rut", pasajeros.ClienteID);
            return View(pasajeros);
        }

        // POST: Pasajeros/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PasajeroID,Rut,Nombres,PrimerApellido,SegundoApellido,Cargo,EMail,AsientoAsignado,EsEjecutivo,ClienteID,Pass,TipoUsuario,PrimerLogeo")] Pasajeros pasajeros)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pasajeros).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Rut", pasajeros.ClienteID);
            return View(pasajeros);
        }

        // GET: Pasajeros/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pasajeros pasajeros = db.Pasajeros.Find(id);
            if (pasajeros == null)
            {
                return HttpNotFound();
            }
            return View(pasajeros);
        }

        // POST: Pasajeros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pasajeros pasajeros = db.Pasajeros.Find(id);
            db.Pasajeros.Remove(pasajeros);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DescargarPlanilla() {
            return View();
        }

        public FileStreamResult ExportarExcel()
        {
            try
            {              
                var libro = ExportarArchivoExcel();
                var memoryStream = new MemoryStream();
                libro.SaveAs(memoryStream);
                memoryStream.Flush();
                memoryStream.Position = 0;

                var nombreArchivo = string.Format("SMA_Pasajeros_{0}.xlsx", "Collahuasi");
                return File(memoryStream, "Pasajeros", nombreArchivo);
            }
            catch (Exception ex)
            {                
                return null;
            }
        }

        private XLWorkbook ExportarArchivoExcel() {
            var workbook = new XLWorkbook();
            var hojaPasajero = workbook.Worksheets.Add("Datos Pasajeros");
            var hojaParam = workbook.Worksheets.Add("Parametros").Hide();

            var listaEsEjecutivo = new List<string>();            
            listaEsEjecutivo.Add("[0] - No es Ejecutivo");
            listaEsEjecutivo.Add("[1] - Es Ejecutivo");
            var totalEsEjecutivo = listaEsEjecutivo.Count();
            hojaParam.Cell(1, 1).InsertData(listaEsEjecutivo);
            var rangoListaEsEjec = hojaParam.Range("A1:A" + totalEsEjecutivo);
            hojaParam.Columns().AdjustToContents();

            hojaPasajero.Cell(1, 1).Value = "N°";
            hojaPasajero.Cell(1, 1).Style.Font.FontColor = XLColor.White;
            hojaPasajero.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.Blue;
            hojaPasajero.Cell(1, 1).Style.Font.Bold = true;

            hojaPasajero.Cell(1, 2).Value = "RUT (*)";
            hojaPasajero.Cell(1, 2).Style.Font.FontColor = XLColor.White;
            hojaPasajero.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.Blue;
            hojaPasajero.Cell(1, 2).Style.Font.Bold = true;

            hojaPasajero.Cell(1, 3).Value = "APELLIDO 1 (*)";
            hojaPasajero.Cell(1, 3).Style.Font.FontColor = XLColor.White;
            hojaPasajero.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.Blue;
            hojaPasajero.Cell(1, 3).Style.Font.Bold = true;

            hojaPasajero.Cell(1, 4).Value = "APELLIDO2";
            hojaPasajero.Cell(1, 4).Style.Font.FontColor = XLColor.White;
            hojaPasajero.Cell(1, 4).Style.Fill.BackgroundColor = XLColor.Blue;
            hojaPasajero.Cell(1, 4).Style.Font.Bold = true;

            hojaPasajero.Cell(1, 5).Value = "NOMBRES (*)";
            hojaPasajero.Cell(1, 5).Style.Font.FontColor = XLColor.White;
            hojaPasajero.Cell(1, 5).Style.Fill.BackgroundColor = XLColor.Blue;
            hojaPasajero.Cell(1, 5).Style.Font.Bold = true;

            hojaPasajero.Cell(1, 6).Value = "CARGO (*)";
            hojaPasajero.Cell(1, 6).Style.Font.FontColor = XLColor.White;
            hojaPasajero.Cell(1, 6).Style.Fill.BackgroundColor = XLColor.Blue;
            hojaPasajero.Cell(1, 6).Style.Font.Bold = true;

            hojaPasajero.Cell(1, 7).Value = "EMAIL";
            hojaPasajero.Cell(1, 7).Style.Font.FontColor = XLColor.White;
            hojaPasajero.Cell(1, 7).Style.Fill.BackgroundColor = XLColor.Blue;
            hojaPasajero.Cell(1, 7).Style.Font.Bold = true;

            hojaPasajero.Cell(1, 8).Value = "ASIENTO";
            hojaPasajero.Cell(1, 8).Style.Font.FontColor = XLColor.White;
            hojaPasajero.Cell(1, 8).Style.Fill.BackgroundColor = XLColor.Blue;
            hojaPasajero.Cell(1, 8).Style.Font.Bold = true;

            hojaPasajero.Cell(1, 9).Value = "EJECUTIVO (*)";
            hojaPasajero.Cell(1, 9).Style.Font.FontColor = XLColor.White;
            hojaPasajero.Cell(1, 9).Style.Fill.BackgroundColor = XLColor.Blue;
            hojaPasajero.Cell(1, 9).Style.Font.Bold = true;

            //hojaPasajero.Cell(fila, 9).SetDataValidation().List(rangoListaEsEjec);
            hojaPasajero.Range("I2:I10000").SetDataValidation().List(rangoListaEsEjec);

            using (var entity = new DAPEntities()) {
                var pasajeroBd = entity.Pasajeros.Where(x => x.TipoUsuario == 1).ToList();

                var fila = 2;
                var contadorReg = 1;                
                foreach (Pasajeros pasajItem in pasajeroBd) {
                    var asientoAsignado = pasajItem.AsientoAsignado != null ? pasajItem.AsientoAsignado.Trim() : string.Empty;
                    var esEjecutivo = (bool)pasajItem.EsEjecutivo ? "[1] - Es Ejecutivo" : "[0] - No es Ejecutivo";

                    hojaPasajero.Cell(fila, 1).DataType = XLCellValues.Number;
                    hojaPasajero.Cell(fila, 1).Value = contadorReg;

                    hojaPasajero.Cell(fila, 2).DataType = XLCellValues.Text;
                    hojaPasajero.Cell(fila, 2).Value = pasajItem.Rut;

                    hojaPasajero.Cell(fila, 3).DataType = XLCellValues.Text;
                    hojaPasajero.Cell(fila, 3).Value = pasajItem.PrimerApellido;

                    hojaPasajero.Cell(fila, 4).DataType = XLCellValues.Text;
                    hojaPasajero.Cell(fila, 4).Value = pasajItem.SegundoApellido;

                    hojaPasajero.Cell(fila, 5).DataType = XLCellValues.Text;
                    hojaPasajero.Cell(fila, 5).Value = pasajItem.Nombres;

                    hojaPasajero.Cell(fila, 6).DataType = XLCellValues.Text;
                    hojaPasajero.Cell(fila, 6).Value = pasajItem.Cargo;

                    hojaPasajero.Cell(fila, 7).DataType = XLCellValues.Text;
                    hojaPasajero.Cell(fila, 7).Value = pasajItem.EMail;

                    hojaPasajero.Cell(fila, 8).DataType = XLCellValues.Text;
                    hojaPasajero.Cell(fila, 8).Value = asientoAsignado;

                    hojaPasajero.Cell(fila, 9).DataType = XLCellValues.Text;                    
                    hojaPasajero.Cell(fila, 9).Value = esEjecutivo;

                    fila++;
                    contadorReg++;
                }
            }

            //Finalizar archivo Excel
            hojaPasajero.Columns().AdjustToContents();

            return workbook;
        }

        public ActionResult CargarPlanilla(string error)
        {
            ViewData["errorExcel"] = error;           
            return View();
        }

        [HttpPost]
        public ActionResult ImportarExcel(CargaArchivoVm model)
        {
            var archivo = model.Archivo;

            if (model == null) throw new ArgumentNullException("model");

            if (archivo != null && archivo.ContentLength > 0)
            {
                var rutaTemp = Server.MapPath("~/DctoExcel");//ConfigurationManager.AppSettings["RutaExcel"];
                var exiRut = Directory.Exists(rutaTemp);
                if (!exiRut)
                {
                    try
                    {
                        Directory.CreateDirectory(rutaTemp);
                    }
                    catch (Exception)
                    {
                        return RedirectToAction("CargarPlanilla", "Pasajeros", new { error = "Proceso carga planilla Excel de Pasajeros, terminado con errores (no se pudo crear el directorio). Intente nuevamente." });
                    }
                }

                var nombreArchivoNue = string.Format("SMA_Pasajeros_{0}{1}", DateTime.Now.ToString("HHmmss"), ".xlsx");
                rutaTemp = rutaTemp + "\\" + nombreArchivoNue;
                archivo.SaveAs(rutaTemp);

                try
                {
                    var listaPasajerosVm = ExcelToListaPasajeros(rutaTemp);

                    if (listaPasajerosVm.Any())
                    {
                        using (var entity = new DAPEntities())
                        {
                            var listaPasajBd = entity.Pasajeros.Where(x => x.TipoUsuario == 1).ToList();

                            foreach (Pasajeros item in listaPasajerosVm)
                            {
                                var pasBd = listaPasajBd.FirstOrDefault(x => x.Rut == item.Rut.Trim());
                                if (pasBd != null)
                                {
                                    pasBd.Rut = item.Rut.Trim();
                                    pasBd.Nombres = item.Nombres.Trim();
                                    pasBd.PrimerApellido = item.PrimerApellido.Trim();
                                    pasBd.SegundoApellido = item.SegundoApellido.Trim();
                                    pasBd.Cargo = item.Cargo.Trim();
                                    pasBd.EMail = item.EMail.Trim();
                                    pasBd.AsientoAsignado = item.AsientoAsignado.Trim();
                                    pasBd.EsEjecutivo = item.EsEjecutivo;
                                    pasBd.ClienteID = pasBd.ClienteID;
                                    pasBd.Pass = pasBd.Pass.Trim();
                                    pasBd.TipoUsuario = pasBd.TipoUsuario;
                                    pasBd.PrimerLogeo = pasBd.PrimerLogeo;
                                }
                                else
                                {
                                    item.PasajeroID = entity.Pasajeros.Max(x => x.PasajeroID) + 1;
                                    entity.Pasajeros.Add(item);
                                }
                                entity.SaveChanges();
                            }

                            using (var dbContextTransaction = entity.Database.BeginTransaction())
                            {
                                dbContextTransaction.Commit();
                            }
                        }

                        return RedirectToAction("CargarPlanilla", "Pasajeros", new { error = "Proceso carga planilla Excel de Pasajeros, terminado exitosamente." });
                    }

                    return RedirectToAction("CargarPlanilla", "Pasajeros", new { error = "Proceso carga planilla Excel de Pasajeros, terminado con errores. Archivo vacío." });
                }
                catch (FormatException msg)
                {
                    return RedirectToAction("CargarPlanilla", "Pasajeros", new { error = "Proceso carga planilla Excel de Pasajeros, terminado con errores. " + msg.Message });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("CargarPlanilla", "Pasajeros", new { error = "Proceso carga planilla Excel de Pasajeros, terminado con errores. " + ex.Message });
                }
            }

            return RedirectToAction("CargarPlanilla", "Pasajeros", new { error = "Proceso carga planilla Excel de Pasajeros, terminado con errores. Intente nuevamente." });
        }


        private List<Pasajeros> ExcelToListaPasajeros(string rutaArchivo) {
            var listaPasajerosVm = new List<Pasajeros>();

            var workbook = new XLWorkbook(rutaArchivo);           

            //hoja 1 es donde se encuentran los datos
            var hojaPasajero = workbook.Worksheet(1);
            var tabla = hojaPasajero.Range(hojaPasajero.Row(hojaPasajero.FirstRowUsed().RowUsed().RowNumber()).FirstCell().Address,
                hojaPasajero.LastCellUsed().Address).RangeUsed().AsTable();

            var numFila = 1;
            foreach (var reg in tabla.DataRange.Rows()) {
                var rut = reg.Field(1).GetString();
                var primerApellido = reg.Field(2).GetString();
                var segundoApellido = reg.Field(3).GetString();
                var nombres = reg.Field(4).GetString();
                var cargo = reg.Field(5).GetString();
                var email = reg.Field(6).GetString();
                var asiento = reg.Field(7).GetString();
                var ejecutivo = reg.Field(8).GetString().Split('-')[0].Replace("[", "").Replace("]", "").Trim();

                if (string.IsNullOrEmpty(rut)) {
                    var msgError = string.Format("Revisar en fila {0} con columna {1}. El RUT no debe estar vacío", numFila, "2");
                    throw new FormatException(msgError);
                }
                if (string.IsNullOrEmpty(primerApellido))
                {
                    var msgError = string.Format("Revisar en fila {0} con columna {1}. El PRIMER APELLIDO no debe estar vacío", numFila, "3");
                    throw new FormatException(msgError);
                }
                if (string.IsNullOrEmpty(nombres))
                {
                    var msgError = string.Format("Revisar en fila {0} con columna {1}. Los NOMBRES no debe estar vacío", numFila, "4");
                    throw new FormatException(msgError);
                }
                if (string.IsNullOrEmpty(cargo))
                {
                    var msgError = string.Format("Revisar en fila {0} con columna {1}. El CARGO no debe estar vacío", numFila, "5");
                    throw new FormatException(msgError);
                }
                if (string.IsNullOrEmpty(ejecutivo))
                {
                    var msgError = string.Format("Revisar en fila {0} con columna {1}. El EJECUTIVO no debe estar vacío", numFila, "8");
                    throw new FormatException(msgError);
                }

                var pasajVm = new Pasajeros {
                    PasajeroID = numFila,
                    Rut = rut,
                    Nombres = nombres.Trim(),
                    PrimerApellido = primerApellido,
                    SegundoApellido = segundoApellido,
                    Cargo = cargo,
                    EMail = email,
                    AsientoAsignado = asiento.Trim(),
                    EsEjecutivo = ejecutivo.Equals("1") ? true : false,
                    ClienteID = 1,
                    Pass = rut.Replace("-", ""),
                    TipoUsuario = 1
                };

                listaPasajerosVm.Add(pasajVm);

                reg.Clear();
                numFila++;
            }

            return listaPasajerosVm;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<SelectListItem> GetSelectTipoUsuario()
        {
            var lista = (from EnumTipoUsuario s in System.Enum.GetValues(typeof(EnumTipoUsuario))
                         select s).AsEnumerable()
                         .Select(s => new SelectListItem
                         {
                             Value = ((int)s).ToString(CultureInfo.InvariantCulture),
                             Text = s.ToString()
                         }).ToList();
            return lista;
        }
    }
}
