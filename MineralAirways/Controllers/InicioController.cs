using System.Web.Mvc;

namespace MineralAirways.Controllers
{
    public class InicioController : BaseController
    {
        // GET: Inicio
        public ActionResult Index()
        {
            return View();
        }
    }
}