using MineralAirways.Models;
using System.Web.Mvc;

namespace MineralAirways.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            SessionViewModel.Usuario = null;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";            
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";            
            return View();
        }
    }
}