using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using MineralAirways.Models;
using PagedList;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Net;


namespace MineralAirways.Controllers
{
    public class PerfilPasajerosController : Controller
    {
        private DAPEntities db = new DAPEntities();
        // GET: PerfilPasajeros
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EditPerfil(int? id)
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
        public ActionResult EditPerfil([Bind(Include = "PasajeroID,Rut,Nombres,PrimerApellido,SegundoApellido,Cargo,EMail,AsientoAsignado,EsEjecutivo,ClienteID,Pass,TipoUsuario,PrimerLogeo")] Pasajeros pasajeros)
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
    }
}