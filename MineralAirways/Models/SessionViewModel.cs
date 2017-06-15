using System.Web;

namespace MineralAirways.Models
{
    public class SessionViewModel
    {
        public static PasajerosViewModel Usuario
        {
            get
            {
                return (PasajerosViewModel)HttpContext.Current.Session["UsuarioVM"];
            }
            set
            {
                HttpContext.Current.Session["UsuarioVM"] = value;
            }
        }
    }
}