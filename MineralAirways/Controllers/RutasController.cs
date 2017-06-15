using MineralAirways.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MineralAirways.Controllers
{
    public class RutasController : BaseController
    {
        private DAPEntities db = new DAPEntities();

        // GET: Rutas
        public ActionResult Index()
        {
            return View(db.Rutas.Where(x => x.Visible).ToList());
        }

        // GET: Rutas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rutas rutas = db.Rutas.Find(id);
            if (rutas == null)
            {
                return HttpNotFound();
            }
            return View(rutas);
        }

        // GET: Rutas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rutas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RutaID,Ciudad,Region,Codigo,Descripcion")] Rutas rutas)
        {
            if (ModelState.IsValid)
            {
                rutas.Visible = true;
                db.Rutas.Add(rutas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rutas);
        }

        // GET: Rutas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rutas rutas = db.Rutas.Find(id);
            if (rutas == null)
            {
                return HttpNotFound();
            }
            return View(rutas);
        }

        // POST: Rutas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RutaID,Ciudad,Region,Codigo,Descripcion,Visible")] Rutas rutas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rutas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rutas);
        }

        // GET: Rutas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rutas rutas = db.Rutas.Find(id);
            if (rutas == null)
            {
                return HttpNotFound();
            }
            return View(rutas);
        }

        // POST: Rutas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rutas rutas = db.Rutas.Find(id);
            rutas.Visible = false;
            //db.Rutas.Remove(rutas);
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
