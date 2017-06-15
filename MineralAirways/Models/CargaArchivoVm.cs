using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MineralAirways.Models
{
    public class CargaArchivoVm
    {
        [Required(ErrorMessage = @"Archivo es requerido")]
        public HttpPostedFileBase Archivo { get; set; }
    }
}