using MineralAirways.Mapping.Factories;
using MineralAirways.Models;
using Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MineralAirways.Controllers
{
    public class HomeLoginController : BaseController
    {
        // GET: HomeLogin
        public ActionResult Index()
        {            
            using (var entity = new DAPEntities())
            {
                var selectRuta = GetSelectRuta();

                var filtro = new ReservaVueloFiltroViewModel
                {
                    DestinoSelectListItem = selectRuta,
                    OrigenSelectListItem = selectRuta
                };

                        
                var format = "dd-MM-yyyy";
                var fechaDate = DateTime.ParseExact(DateTime.Now.ToShortDateString(), format, CultureInfo.InvariantCulture);
                var vueloBd = entity.Vuelos.Where(x => x.FechaHoraSalida >= fechaDate && x.Visible == true);

                var listaFecha = new StringBuilder(string.Empty);

                var i = 0;
                foreach (var fecha in vueloBd.OrderBy(x => x.FechaHoraSalida).ToList()) {
                    
                    var fechaFormato = string.Empty;
                    var dia = fecha.FechaHoraSalida.Day.ToString();
                    var mes = fecha.FechaHoraSalida.Month.ToString();
                    var ano = fecha.FechaHoraSalida.Year.ToString();
                    fechaFormato = dia + "/" + mes + "/" + ano;

                    if (!listaFecha.ToString().Contains(fechaFormato)) {
                        listaFecha.Append(fechaFormato)
                        .Append(";");
                    }                    
                    i++;
                }            

                ViewBag.FechaVueloArray = listaFecha.ToString();

                return View(filtro);
            }
        }
        public ActionResult Details()
        {
            try
            {
                //Filtro por fecha
                var format = "dd-MM-yyyy";
                var fechaDate = DateTime.ParseExact(DateTime.Now.ToShortDateString(), format, CultureInfo.InvariantCulture);

                var entity = new DAPEntities();
                List<ReservasVuelos> miReservaBd = entity.ReservasVuelos.Where(x => x.PasajeroID == SessionViewModel.Usuario.PasajeroID
                          && x.ConfirmacionAsiento == true
                          && x.Vuelos.Visible
                          && x.Tramos.FechaHoraSalida >= fechaDate).ToList();

                return View(miReservaBd);
            }
            catch (Exception)
            {
                return RedirectToAction("ReservaError", "HomeLogin", new { mensaje = "La carga del módulo Mis Reservas no se ha podido completar, intente nuevamente.", tipoMensaje = "D" });
            }
        }

        public ActionResult BusquedaVistaParcialReservaVuelo(int origen, int destino, string fecha)
        {
            //Filtro por fecha
            var format = "dd-MM-yyyy";
            var fechaConFormato = fecha.Replace("/", "-");
            var fechaDate = DateTime.ParseExact(fechaConFormato, format, CultureInfo.InvariantCulture);

            var entity = new DAPEntities();
            var vueloBd = (from vuelo in entity.Vuelos.ToList()
                           where vuelo.Visible == true
                           && vuelo.OrigenID == origen
                           && vuelo.DestinoID == destino
                           && DateTime.ParseExact(vuelo.FechaHoraSalida.ToShortDateString(), format, CultureInfo.InvariantCulture) == fechaDate
                           && (!entity.ReservasVuelos.Any(f => f.VueloID == vuelo.VueloID
                           && f.ConfirmacionAsiento == true
                           && f.PasajeroID == SessionViewModel.Usuario.PasajeroID
                           && f.AvionID == vuelo.AvionID
                           && ((f.Tramos.OrigenID >= origen
                           && f.Tramos.DestinoID <= destino)
                           || (f.Tramos.OrigenID <= origen
                           && f.Tramos.DestinoID >= destino))))
                           && entity.Tramos.Any(t => t.VueloID == vuelo.VueloID
                           && ((t.OrigenID >= origen
                           && t.DestinoID <= destino)
                           || (t.OrigenID <= origen
                           && t.DestinoID >= destino)))
                           select vuelo).ToList();           

            

            //var vueloFiltroBd = vueloBd.Where(x => DateTime.ParseExact(x.FechaHoraSalida.ToShortDateString(), format, CultureInfo.InvariantCulture) == fechaDate);

            if (!vueloBd.Any())
            {
                var msg = new MensajeErrorVm
                {
                    Mensaje = "¡Usted ya tiene una reserva para este vuelo !. Cambie el origen, el destino y fechas.",
                    TipoMensaje = "D"
                };
                return PartialView("_ReservaError", msg);
            }
            else
            {
                var datoVal = (from vuelo in vueloBd.ToList()
                               where (vuelo.OrigenID >= origen && vuelo.OrigenID <= origen)
                               && vuelo.Visible
                               && (vuelo.DestinoID >= destino && vuelo.DestinoID <= destino)
                               select vuelo).ToList();

                if (!datoVal.Any())
                {
                    var msg = new MensajeErrorVm
                    {
                        Mensaje = "¡Usted no tiene vuelo asociado en la ruta seleccionada!. Cambie el origen, el destino y fechas.",
                        TipoMensaje = "D"
                    };
                    return PartialView("_ReservaError", msg);
                }

                ViewBag.OrigenSeleccionado = origen;
                ViewBag.DestinoSeleccionado = destino;
                return PartialView("_ReservaVuelo", datoVal);
            }
        }

        public ActionResult ReservarVuelo(int vueloId, int origenSel, int destinoSel, string asientoStringId, string reservaAntId)
        {
            try
            {
                using (var entity = new DAPEntities())
                {
                    var vueloBd = entity.Vuelos.FirstOrDefault(x => x.VueloID == vueloId);
                    if (vueloBd != null)
                    {
                        Asientos asientoBd = null;

                        if (string.IsNullOrEmpty(asientoStringId))
                        {
                            asientoBd = entity.Asientos.FirstOrDefault(x => x.AvionID == vueloBd.AvionID && x.Descripcion.Trim() == SessionViewModel.Usuario.AsientoAsignado.Trim());
                        }
                        else {
                            var asientoId = Convert.ToInt32(asientoStringId);
                            asientoBd = entity.Asientos.FirstOrDefault(x => x.AvionID == vueloBd.AvionID && x.AsientoID == asientoId);
                        }                        

                        if (asientoBd != null)
                        {
                            //Validar que no exista reserva del asiento para el vuelo seleccionado por tipo de ejecutivo
                            var reservaValBd = entity.ReservasVuelos.Where(x => x.AsientoID == asientoBd.AsientoID
                                                && x.AvionID == vueloBd.AvionID
                                                && x.VueloID == vueloBd.VueloID
                                                && x.ConfirmacionAsiento == true
                                                && ((x.Tramos.OrigenID >= origenSel
                                                && x.Tramos.DestinoID <= destinoSel)
                                                || (x.Tramos.OrigenID <= origenSel
                                                && x.Tramos.DestinoID >= destinoSel)));

                            if (ValidarExistenciaAsiento(reservaValBd))
                            {
                                return RedirectToAction("ReservaVueloTomado", "HomeLogin", new { vueloId = vueloBd.VueloID, origenSel, destinoSel });
                            }

                            using (var dbContextTransaction = entity.Database.BeginTransaction())
                            {
                                if (!string.IsNullOrEmpty(reservaAntId)) {
                                    var reservaVueloAntId = Convert.ToInt32(reservaAntId);
                                    var reservaAntBd = entity.ReservasVuelos.FirstOrDefault(x => x.ReservaVueloID == reservaVueloAntId);

                                    reservaAntBd.FechaHoraConfirmación = null;
                                    reservaAntBd.FechaHoraCancelacion = DateTime.Now;
                                    reservaAntBd.ConfirmacionAsiento = false;
                                    entity.SaveChanges();

                                    var resultCorreo = EnviaCorreoCancelacionReservaVuelo(reservaAntBd.VueloID, reservaAntBd.Vuelos.OrigenID, reservaAntBd.Vuelos.DestinoID, reservaAntBd);
                                }                              

                                var listaTramos = entity.Tramos.Where(x => x.VueloID == vueloId
                                                                && ((x.OrigenID >= origenSel
                                                                && x.DestinoID <= destinoSel)
                                                                || (x.OrigenID <= origenSel
                                                                && x.DestinoID >= destinoSel))).ToList();

                                foreach (Tramos tramo in listaTramos)
                                {
                                    int reservaVueloIdUlt = entity.ReservasVuelos.Any() ? entity.ReservasVuelos.Max(x => x.ReservaVueloID) : 0;
                                    var reservaVueloId = reservaVueloIdUlt != 0 ? reservaVueloIdUlt + 1 : 1;
                                    var reservaBd = new ReservasVuelos
                                    {
                                        ReservaVueloID = reservaVueloId,
                                        AvionID = vueloBd.AvionID,
                                        ConfirmacionAsiento = true,
                                        FechaHoraConfirmación = DateTime.Now,
                                        FechaHoraCancelacion = null,
                                        PasajeroID = SessionViewModel.Usuario.PasajeroID,
                                        VueloID = vueloBd.VueloID,
                                        AsientoID = asientoBd.AsientoID,
                                        TramoID = tramo.TramoID
                                    };

                                    entity.ReservasVuelos.Add(reservaBd);
                                    entity.SaveChanges();
                                }
                                dbContextTransaction.Commit();
                            }

                            var idMaxReservaVuelo = entity.ReservasVuelos.Max(x => x.ReservaVueloID);
                            var reservaVuelo = entity.ReservasVuelos.FirstOrDefault(x => x.ReservaVueloID == idMaxReservaVuelo);
                            reservaVuelo.Pasajeros = entity.Pasajeros.FirstOrDefault(x => x.PasajeroID == reservaVuelo.PasajeroID);
                            GenerarCodigoQr(reservaVuelo);

                            if (EnviaCorreoReservaVuelo(vueloId, origenSel, destinoSel, null, reservaVuelo))
                            {
                                return RedirectToAction("ReservaOK", "HomeLogin", new { mensaje = "Reserva del vuelo realizado exitosamente." });
                            }
                            return RedirectToAction("ReservaOK", "HomeLogin", new { mensaje = "Reserva del vuelo realizado exitosamente. Pero no se pudo enviar el correo." });                            
                        }
                        else
                        {
                            return RedirectToAction("ReservaVueloTomado", "HomeLogin", new { vueloId = vueloBd.VueloID, origenSel, destinoSel });
                        }
                    }

                    return RedirectToAction("ReservaError", "HomeLogin", new { mensaje = "La reserva del vuelo no se ha podido completar, intente nuevamente.", tipoMensaje = "D" });
                }


            }
            catch (Exception ex)
            {
                return RedirectToAction("ReservaError", "HomeLogin", new { mensaje = "La reserva del vuelo no se ha podido completar, intente nuevamente.", tipoMensaje = "D" });
            }
        }

        private bool ValidarExistenciaAsiento(IQueryable<ReservasVuelos> reservaBd) {
            if (reservaBd.Count() > 0) {
                return true;
            }
            return false;
        }

        public ActionResult ReservarVueloAsiento(int vueloId, int asientoId, int origenSel, int destinoSel)
        {
            try
            {
                using (var entity = new DAPEntities())
                {
                    var vueloBd = entity.Vuelos.FirstOrDefault(x => x.VueloID == vueloId);
                    if (vueloBd != null)
                    {
                        var asientoBd = entity.Asientos.FirstOrDefault(x => x.AsientoID == asientoId);

                        if (asientoBd != null)
                        {
                            using (var dbContextTransaction = entity.Database.BeginTransaction())
                            {
                                var listaTramos = entity.Tramos.Where(x => x.VueloID == vueloId
                                                                && ((x.OrigenID >= origenSel
                                                                && x.DestinoID <= destinoSel)
                                                                || (x.OrigenID <= origenSel
                                                                && x.DestinoID >= destinoSel))).ToList();

                                foreach (Tramos tramo in listaTramos)
                                {
                                    int reservaVueloIdUlt = entity.ReservasVuelos.Any() ? entity.ReservasVuelos.Max(x => x.ReservaVueloID) : 0;
                                    var reservaVueloId = reservaVueloIdUlt != 0 ? reservaVueloIdUlt + 1 : 1;
                                    var reservaBd = new ReservasVuelos
                                    {
                                        ReservaVueloID = reservaVueloId,
                                        AvionID = vueloBd.AvionID,
                                        ConfirmacionAsiento = true,
                                        FechaHoraConfirmación = DateTime.Now,
                                        FechaHoraCancelacion = null,
                                        PasajeroID = SessionViewModel.Usuario.PasajeroID,
                                        VueloID = vueloBd.VueloID,
                                        AsientoID = asientoBd.AsientoID,
                                        TramoID = tramo.TramoID
                                    };

                                    entity.ReservasVuelos.Add(reservaBd);
                                    entity.SaveChanges();
                                }
                                dbContextTransaction.Commit();
                            }

                            var idMaxReservaVuelo = entity.ReservasVuelos.Max(x => x.ReservaVueloID);
                            var reservaVuelo = entity.ReservasVuelos.FirstOrDefault(x => x.ReservaVueloID == idMaxReservaVuelo);
                            reservaVuelo.Pasajeros = entity.Pasajeros.FirstOrDefault(x => x.PasajeroID == reservaVuelo.PasajeroID);
                            GenerarCodigoQr(reservaVuelo);

                            if (EnviaCorreoReservaVuelo(vueloId, origenSel, destinoSel, asientoId, reservaVuelo))
                            {
                                return RedirectToAction("ReservaOK", "HomeLogin", new { mensaje = "Reserva del vuelo realizado exitosamente." });
                            }
                            else
                            {
                                return RedirectToAction("ReservaOK", "HomeLogin", new { mensaje = "Reserva del vuelo realizado exitosamente. Pero no se pudo enviar el correo." });
                            }
                        }
                    }

                    return RedirectToAction("ReservaError", "HomeLogin", new { mensaje = "La reserva del vuelo no se ha podido completar, intente nuevamente.", tipoMensaje = "D" });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("ReservaError", "HomeLogin", new { mensaje = "La reserva del vuelo no se ha podido completar, intente nuevamente.", tipoMensaje = "D" });
            }
        }

        public ActionResult ReservaOK(string mensaje)
        {
            ViewBag.Mensaje = mensaje;
            return View();
        }

        public ActionResult ReservaError(string mensaje, string tipoMensaje)
        {
            ViewBag.Mensaje = mensaje;
            ViewBag.TipoMensaje = tipoMensaje;
            return View();
        }

        public ActionResult ReservaVueloTomado(int vueloId, int origenSel, int destinoSel)
        {
            var entity = new DAPEntities();

            var vueloBd = entity.Vuelos.FirstOrDefault(x => x.VueloID == vueloId);

            var listaVueloAsiento = new List<VueloAsientoVm>();
            var listaAsiento = new List<Asientos>();

            if (SessionViewModel.Usuario.EsEjecutivo)
            {
                listaAsiento = entity.Asientos.Where(x => x.AvionID == vueloBd.AvionID
                                            && x.FilaAsiento >= 1 && x.FilaAsiento <= 3
                                            && x.Tipo == 1).ToList();
            }
            else
            {
                listaAsiento = entity.Asientos.Where(x => x.AvionID == vueloBd.AvionID
                                            && x.FilaAsiento >= 4
                                            && x.Tipo == 1).ToList();
            }

            var listaAsientoSinReserva = (from asiento in listaAsiento.ToList()
                                          where !entity.ReservasVuelos.Any(x => x.AsientoID == asiento.AsientoID
                                          && x.AvionID == asiento.AvionID
                                          && x.VueloID == vueloBd.VueloID
                                          && x.ConfirmacionAsiento == true
                                          && ((x.Tramos.OrigenID >= origenSel
                                          && x.Tramos.DestinoID <= destinoSel)
                                          || (x.Tramos.OrigenID <= origenSel
                                          && x.Tramos.DestinoID >= destinoSel)))                                          
                                          && asiento.Tipo == 1
                                          select asiento).ToList();

            var listaAsientoPreAsignado = entity.Pasajeros.Where(x => x.AsientoAsignado.Trim() != null && x.AsientoAsignado.Trim() != "").ToList();

            foreach (var asiento in listaAsientoSinReserva)
            {
                if (asiento.Tipo == 1) {
                    if (!listaAsientoPreAsignado.Exists(x => x.AsientoAsignado.Trim() == asiento.Descripcion.Trim()))
                    {
                        var vueloAsiento = new VueloAsientoVm
                        {
                            Destino = vueloBd.Rutas.Ciudad,
                            Origen = vueloBd.Rutas1.Ciudad,
                            FechaVuelo = vueloBd.FechaHoraSalida,
                            NumeroVuelo = vueloBd.NumeroVuelo,
                            VueloId = vueloBd.VueloID,
                            Asiento = asiento.Descripcion,
                            AsientoId = asiento.AsientoID,
                            ListaTramos = vueloBd.Tramos.ToList()
                        };
                        listaVueloAsiento.Add(vueloAsiento);
                    }
                }                
            }

            ViewBag.OrigenSeleccionado = origenSel;
            ViewBag.DestinoSeleccionado = destinoSel;

            return View(listaVueloAsiento);
        }

        public ActionResult CancelarReservaVuelo(int reservaId)
        {
            try
            {
                using (var entity = new DAPEntities())
                {
                    var reservaBd = entity.ReservasVuelos.FirstOrDefault(x => x.ReservaVueloID == reservaId);                    
                    if (reservaBd != null)
                    {
                        using (var dbContextTransaction = entity.Database.BeginTransaction())
                        {
                            reservaBd.FechaHoraConfirmación = null;
                            reservaBd.FechaHoraCancelacion = DateTime.Now;
                            reservaBd.ConfirmacionAsiento = false;

                            entity.SaveChanges();
                            dbContextTransaction.Commit();
                        }

                        var origenSel = reservaBd.Tramos.OrigenID;
                        var destinoSel = reservaBd.Tramos.DestinoID;
                        if (EnviaCorreoCancelacionReservaVuelo(reservaBd.VueloID, origenSel, destinoSel, reservaBd))
                        {
                            return RedirectToAction("ReservaOK", "HomeLogin", new { mensaje = "Cancelación Reserva del vuelo realizado exitosamente." });
                        }
                        else
                        {
                            return RedirectToAction("ReservaOK", "HomeLogin", new { mensaje = "Cancelación Reserva del vuelo realizado exitosamente. Pero no se pudo enviar el correo." });
                        }

                        
                    }

                    return RedirectToAction("ReservaError", "HomeLogin", new { mensaje = "La cancelación de la reserva del vuelo no se ha podido completar, intente nuevamente.", tipoMensaje = "D" });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("ReservaError", "HomeLogin", new { mensaje = "La cancelación de la reserva del vuelo no se ha podido completar, intente nuevamente.", tipoMensaje = "D" });
            }
        }

        public ActionResult VerReservaVuelo(int reservaId)
        {
            try
            {
                var entity = new DAPEntities();
                var reservaBd = entity.ReservasVuelos.FirstOrDefault(x => x.ReservaVueloID == reservaId);
                var rutaImgQr = Helpers.ImageQr.ObtenerImagenQr(reservaBd, Server.MapPath("~/ImgQr"), Server.MapPath("~/img"));
                ViewBag.SrcImgQr = rutaImgQr;
                return View(reservaBd);
            }
            catch (Exception)
            {
                return RedirectToAction("ReservaError", "HomeLogin", new { mensaje = "La visualización de la reserva del vuelo no se ha podido completar, intente nuevamente.", tipoMensaje = "D" });
            }
        }

        public ActionResult CambiarVuelo(int reservaId) {
            var entity = new DAPEntities();
            var format = "dd-MM-yyyy";
            var fechaActualTresDias = DateTime.ParseExact(DateTime.Now.AddDays(3).ToShortDateString(), format, CultureInfo.InvariantCulture);
            
            var reservaBd = entity.ReservasVuelos.FirstOrDefault(x => x.ReservaVueloID == reservaId);
            
            var vueloBd = (from vuelo in entity.Vuelos.ToList()
                           where DateTime.ParseExact(vuelo.FechaHoraSalida.ToShortDateString(), format, CultureInfo.InvariantCulture) >= fechaActualTresDias
                           && vuelo.VueloID != reservaBd.VueloID
                           && vuelo.Visible
                           && (!entity.ReservasVuelos.Any(f => f.VueloID == vuelo.VueloID
                           && f.ConfirmacionAsiento == true
                           && f.PasajeroID == SessionViewModel.Usuario.PasajeroID
                           && f.AvionID == vuelo.AvionID))
                           select vuelo).ToList();

            ViewBag.AsientoId = reservaBd.AsientoID;
            ViewBag.NumeroAsiento = reservaBd.Asientos.Descripcion.Trim();
            ViewBag.ReservaAntId = reservaBd.ReservaVueloID;
            ViewBag.OrigenNombre = reservaBd.Vuelos.Rutas1.Ciudad;
            ViewBag.DestinoNombre = reservaBd.Vuelos.Rutas.Ciudad;
            ViewBag.NumeroVuelo = reservaBd.Vuelos.NumeroVuelo;
            ViewBag.FechaHoraSalida = reservaBd.Vuelos.FechaHoraSalida;

            return View(vueloBd);
        }

        private List<SelectListItem> GetSelectRuta()
        {
            using (var entity = new DAPEntities())
            {
                var rutaBd = entity.Rutas.ToList();

                var lista = (from a in rutaBd
                             where a.Visible
                             orderby a.RutaID
                             select a).AsEnumerable()
                        .Select(x => new SelectListItem
                        {
                            Value = x.RutaID.ToString(CultureInfo.InvariantCulture),
                            Text = x.Ciudad
                        }).ToList();

                return lista;
            }
        }

        private bool EnviaCorreoReservaVuelo(int vueloId, int origenSel, int destinoSel, int? asientoId, ReservasVuelos reserva)
        {
            try
            {
                using (var entity = new DAPEntities())
                {
                    var vueloBd = entity.Vuelos.FirstOrDefault(x => x.VueloID == vueloId);
                    var pasajeroBd = entity.Pasajeros.FirstOrDefault(x => x.Rut == SessionViewModel.Usuario.Rut);

                    if (vueloBd != null && pasajeroBd != null)
                    {
                        if (!string.IsNullOrEmpty(pasajeroBd.EMail))
                        {
                            var asientoBd = new Asientos();
                            if (asientoId == null)
                            {
                                asientoBd = entity.Asientos.FirstOrDefault(x => x.AvionID == vueloBd.AvionID && x.Descripcion == pasajeroBd.AsientoAsignado);
                            }
                            else
                            {
                                asientoBd = entity.Asientos.FirstOrDefault(x => x.AsientoID == asientoId);
                            }

                            var descripcionAsiento = string.Empty;
                            if (asientoBd != null)
                            {
                                descripcionAsiento = asientoBd.Descripcion;
                            }
                            else {
                                descripcionAsiento = reserva.Asientos != null ? reserva.Asientos.Descripcion : string.Empty;
                            }

                            var origenTramo = vueloBd.Tramos.FirstOrDefault(x => x.VueloID == vueloBd.VueloID && x.OrigenID == origenSel);
                            var destinoTramo = vueloBd.Tramos.FirstOrDefault(x => x.VueloID == vueloBd.VueloID && x.DestinoID == destinoSel);

                            var cuerpoCorreo = EmailMensajes.ReservarVuelo;
                            cuerpoCorreo = cuerpoCorreo.Replace("[NOMBRE_USUARIO]", SessionViewModel.Usuario.Nombre + " " + SessionViewModel.Usuario.PrimerApellido);
                            cuerpoCorreo = cuerpoCorreo.Replace("[FECHA_ENVIO]", DateTime.Now.ToString());
                            cuerpoCorreo = cuerpoCorreo.Replace("[NUMERO_VUELO]", vueloBd.NumeroVuelo.ToString());
                            cuerpoCorreo = cuerpoCorreo.Replace("[FECHA_SALIDA]", origenTramo.FechaHoraSalida.ToString("dd-MM-yyyy"));
                            cuerpoCorreo = cuerpoCorreo.Replace("[HORA_SALIDA]", origenTramo.FechaHoraSalida.ToString("HH:mm"));
                            cuerpoCorreo = cuerpoCorreo.Replace("[ORIGEN]", origenTramo.Rutas1.Ciudad);
                            cuerpoCorreo = cuerpoCorreo.Replace("[DESTINO]", destinoTramo.Rutas.Ciudad);
                            cuerpoCorreo = cuerpoCorreo.Replace("[ASIENTO]", descripcionAsiento);
                            cuerpoCorreo = cuerpoCorreo.Replace("[AVION]", vueloBd.Aviones.MatriculaDelAvion);

                            var asuntoCorreo = "Confirmación Reserva de Vuelo";

                            var modelo = new Dictionary<string, string>
                            {
                                {"CorreoDe", ConfigurationManager.AppSettings["ScpEmailDe"]},
                                {"CorreoPara", pasajeroBd.EMail},
                                {"CorreoCc", string.Empty},
                                {"CorreoCco", string.Empty},
                                {"CorreoAsunto", asuntoCorreo},
                                {"CorreoCuerpo", cuerpoCorreo}
                            };

                            var vmCorreo = new EnvioEmailViewModel
                            {
                                AttachmentList = null,
                                BlindCopyCarbon = modelo["CorreoCco"],
                                CopyCarbon = modelo["CorreoCc"],
                                FromEmail = modelo["CorreoDe"],
                                Subject = modelo["CorreoAsunto"],
                                To = modelo["CorreoPara"],
                                BodyEmail = modelo["CorreoCuerpo"]
                            };

                            var res = EnvioCorreoFactory.EnviarEmail(vmCorreo, reserva, Server.MapPath("~/ImgQr"), Server.MapPath("~/img"));

                            return res;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool EnviaCorreoCancelacionReservaVuelo(int vueloId, int origenSel, int destinoSel, ReservasVuelos reservaBd)
        {
            try
            {
                using (var entity = new DAPEntities())
                {
                    var vueloBd = entity.Vuelos.FirstOrDefault(x => x.VueloID == vueloId);
                    var pasajeroBd = entity.Pasajeros.FirstOrDefault(x => x.Rut == SessionViewModel.Usuario.Rut);

                    if (vueloBd != null && pasajeroBd != null)
                    {
                        if (!string.IsNullOrEmpty(pasajeroBd.EMail))
                        {                            
                            var asientoBd = reservaBd.Asientos;

                            var origenTramo = vueloBd.Tramos.FirstOrDefault(x => x.VueloID == vueloBd.VueloID && x.OrigenID == origenSel);
                            var destinoTramo = vueloBd.Tramos.FirstOrDefault(x => x.VueloID == vueloBd.VueloID && x.DestinoID == destinoSel);

                            var cuerpoCorreo = EmailMensajes.CancelacionReservaVuelo;
                            cuerpoCorreo = cuerpoCorreo.Replace("[NOMBRE_USUARIO]", SessionViewModel.Usuario.Nombre + " " + SessionViewModel.Usuario.PrimerApellido);
                            cuerpoCorreo = cuerpoCorreo.Replace("[FECHA_ENVIO]", DateTime.Now.ToString());
                            cuerpoCorreo = cuerpoCorreo.Replace("[NUMERO_VUELO]", vueloBd.NumeroVuelo.ToString());
                            cuerpoCorreo = cuerpoCorreo.Replace("[FECHA_SALIDA]", origenTramo.FechaHoraSalida.ToString("dd-MM-yyyy"));
                            cuerpoCorreo = cuerpoCorreo.Replace("[HORA_SALIDA]", origenTramo.FechaHoraSalida.ToString("HH:mm"));
                            cuerpoCorreo = cuerpoCorreo.Replace("[ORIGEN]", origenTramo.Rutas1.Ciudad);
                            cuerpoCorreo = cuerpoCorreo.Replace("[DESTINO]", destinoTramo.Rutas.Ciudad);
                            cuerpoCorreo = cuerpoCorreo.Replace("[ASIENTO]", asientoBd != null ? asientoBd.Descripcion : string.Empty);
                            cuerpoCorreo = cuerpoCorreo.Replace("[AVION]", vueloBd.Aviones.MatriculaDelAvion);

                            var asuntoCorreo = "Cancelación Reserva de Vuelo";

                            var modelo = new Dictionary<string, string>
                            {
                                {"CorreoDe", ConfigurationManager.AppSettings["ScpEmailDe"]},
                                {"CorreoPara", pasajeroBd.EMail},
                                {"CorreoCc", string.Empty},
                                {"CorreoCco", string.Empty},
                                {"CorreoAsunto", asuntoCorreo},
                                {"CorreoCuerpo", cuerpoCorreo}
                            };

                            var vmCorreo = new EnvioEmailViewModel
                            {
                                AttachmentList = null,
                                BlindCopyCarbon = modelo["CorreoCco"],
                                CopyCarbon = modelo["CorreoCc"],
                                FromEmail = modelo["CorreoDe"],
                                Subject = modelo["CorreoAsunto"],
                                To = modelo["CorreoPara"],
                                BodyEmail = modelo["CorreoCuerpo"]
                            };

                            var res = EnvioCorreoFactory.EnviarEmail(vmCorreo, reservaBd, Server.MapPath("~/ImgQr"), Server.MapPath("~/img"));

                            return res;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void GenerarCodigoQr(ReservasVuelos reserva) {
            var rutaTemp = Server.MapPath("~/ImgQr");
            //CREA DIRECTORIO SINO EXISTE
            var exiRut = Directory.Exists(rutaTemp);
            if (!exiRut)
            {
                try
                {
                    Directory.CreateDirectory(rutaTemp);
                }
                catch (Exception)
                {
                    
                }
            }

            var generQr = new GeneradorCodigoQrFactory();
            generQr.GenerarQr(rutaTemp, reserva);
        }        
    }
}