using MineralAirways.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MineralAirways.Controllers
{
    public class VuelosController : BaseController
    {
        private DAPEntities db = new DAPEntities();

        // GET: Vuelos
        public ActionResult Index()
        {
            var vuelos = db.Vuelos.Include(v => v.Aviones).Include(v => v.Rutas).Include(v => v.Rutas1).Where(x => x.Visible);
            return View(vuelos.ToList());
        }

        public ActionResult fechasdatapicker()
        {
            var vuelos = db.Vuelos.Include(v => v.Aviones).Include(v => v.Rutas).Include(v => v.Rutas1);
            return View(vuelos.ToList());
        }

        // GET: Vuelos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vuelos vuelos = db.Vuelos.Find(id);
            if (vuelos == null)
            {
                return HttpNotFound();
            }
            return View(vuelos);
        }

        // GET: Vuelos/Create
        public ActionResult Create()
        {
            ViewBag.AvionID = new SelectList(db.Aviones, "AvionID", "MatriculaDelAvion");
            ViewBag.DestinoID = new SelectList(db.Rutas.Where(x => x.Visible), "RutaID", "Codigo");
            ViewBag.OrigenID = new SelectList(db.Rutas.Where(x => x.Visible), "RutaID", "Codigo");
            return View();
        }

        // POST: Vuelos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VueloID,OrigenID,DestinoID,FechaHoraSalida,NumeroVuelo,AvionID")] Vuelos vuelos)
        {
            if (ModelState.IsValid)
            {
                vuelos.Visible = true;
                db.Vuelos.Add(vuelos);
                db.SaveChanges();

                var vueloBd = db.Vuelos.OrderByDescending(x => x.VueloID).FirstOrDefault();
                if (vueloBd != null) {
                    var tramoMaxId = db.Tramos.Any() ? db.Tramos.Max(x => x.TramoID) : 0;
                    var tramo = new Tramos
                    {
                        VueloID = vueloBd.VueloID,
                        DestinoID = vueloBd.DestinoID,
                        FechaHoraSalida = vueloBd.FechaHoraSalida,
                        OrigenID = vueloBd.OrigenID,
                        TramoID = tramoMaxId + 1
                    };

                    db.Tramos.Add(tramo);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            ViewBag.AvionID = new SelectList(db.Aviones, "AvionID", "MatriculaDelAvion", vuelos.AvionID);
            ViewBag.DestinoID = new SelectList(db.Rutas, "RutaID", "Ciudad", vuelos.DestinoID);
            ViewBag.OrigenID = new SelectList(db.Rutas, "RutaID", "Ciudad", vuelos.OrigenID);
            return View(vuelos);
        }

        // GET: Vuelos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vuelos vuelos = db.Vuelos.Find(id);
            if (vuelos == null)
            {
                return HttpNotFound();
            }
            ViewBag.AvionID = new SelectList(db.Aviones, "AvionID", "MatriculaDelAvion", vuelos.AvionID);
            ViewBag.DestinoID = new SelectList(db.Rutas, "RutaID", "Ciudad", vuelos.DestinoID);
            ViewBag.OrigenID = new SelectList(db.Rutas, "RutaID", "Ciudad", vuelos.OrigenID);
            return View(vuelos);
        }

        // POST: Vuelos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VueloID,OrigenID,DestinoID,FechaHoraSalida,NumeroVuelo,AvionID,Visible")] Vuelos vuelos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vuelos).State = EntityState.Modified;
                db.SaveChanges();

                var vueloBd = db.Vuelos.FirstOrDefault(x => x.VueloID == vuelos.VueloID);
                if (vueloBd != null) {
                    var tramoBd = db.Tramos.FirstOrDefault(x => x.VueloID == vueloBd.VueloID
                    && x.OrigenID == vueloBd.OrigenID && x.DestinoID == vueloBd.DestinoID);

                    if (tramoBd != null) {
                        tramoBd.DestinoID = vueloBd.DestinoID;
                        tramoBd.OrigenID = vueloBd.OrigenID;
                        tramoBd.FechaHoraSalida = vueloBd.FechaHoraSalida;
                        db.SaveChanges();
                    }

                }

                return RedirectToAction("Index");
            }
            ViewBag.AvionID = new SelectList(db.Aviones, "AvionID", "MatriculaDelAvion", vuelos.AvionID);
            ViewBag.DestinoID = new SelectList(db.Rutas, "RutaID", "Ciudad", vuelos.DestinoID);
            ViewBag.OrigenID = new SelectList(db.Rutas, "RutaID", "Ciudad", vuelos.OrigenID);
            return View(vuelos);
        }

        // GET: Vuelos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vuelos vuelos = db.Vuelos.Find(id);
            if (vuelos == null)
            {
                return HttpNotFound();
            }
            return View(vuelos);
        }

        // POST: Vuelos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vuelos vuelos = db.Vuelos.Find(id);
            vuelos.Visible = false;
            //db.Vuelos.Remove(vuelos);
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
