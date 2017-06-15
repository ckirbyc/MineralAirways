using MineralAirways.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MineralAirways.Controllers
{
    public class TramosController : BaseController
    {
        private DAPEntities db = new DAPEntities();

        // GET: Tramos
        public ActionResult Index()
        {
            var tramos = db.Tramos.Include(t => t.Rutas).Include(t => t.Rutas1).Include(t => t.Vuelos);
            return View(tramos.ToList());
        }

        // GET: Tramos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tramos tramos = db.Tramos.Find(id);
            if (tramos == null)
            {
                return HttpNotFound();
            }
            return View(tramos);
        }

        // GET: Tramos/Create
        public ActionResult Create()
        {
            ViewBag.DestinoID = new SelectList(db.Rutas, "RutaID", "Ciudad");
            ViewBag.OrigenID = new SelectList(db.Rutas, "RutaID", "Ciudad");
            ViewBag.VueloID = new SelectList(db.Vuelos, "VueloID", "VueloID");
            return View();
        }

        // POST: Tramos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TramoID,VueloID,OrigenID,DestinoID,FechaHoraSalida")] Tramos tramos)
        {
            if (ModelState.IsValid)
            {
                db.Tramos.Add(tramos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DestinoID = new SelectList(db.Rutas, "RutaID", "Ciudad", tramos.DestinoID);
            ViewBag.OrigenID = new SelectList(db.Rutas, "RutaID", "Ciudad", tramos.OrigenID);
            ViewBag.VueloID = new SelectList(db.Vuelos, "VueloID", "VueloID", tramos.VueloID);
            return View(tramos);
        }

        // GET: Tramos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tramos tramos = db.Tramos.Find(id);
            if (tramos == null)
            {
                return HttpNotFound();
            }
            ViewBag.DestinoID = new SelectList(db.Rutas, "RutaID", "Ciudad", tramos.DestinoID);
            ViewBag.OrigenID = new SelectList(db.Rutas, "RutaID", "Ciudad", tramos.OrigenID);
            ViewBag.VueloID = new SelectList(db.Vuelos, "VueloID", "VueloID", tramos.VueloID);
            return View(tramos);
        }

        // POST: Tramos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TramoID,VueloID,OrigenID,DestinoID,FechaHoraSalida")] Tramos tramos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tramos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DestinoID = new SelectList(db.Rutas, "RutaID", "Ciudad", tramos.DestinoID);
            ViewBag.OrigenID = new SelectList(db.Rutas, "RutaID", "Ciudad", tramos.OrigenID);
            ViewBag.VueloID = new SelectList(db.Vuelos, "VueloID", "VueloID", tramos.VueloID);
            return View(tramos);
        }

        // GET: Tramos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tramos tramos = db.Tramos.Find(id);
            if (tramos == null)
            {
                return HttpNotFound();
            }
            return View(tramos);
        }

        // POST: Tramos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tramos tramos = db.Tramos.Find(id);
            db.Tramos.Remove(tramos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
