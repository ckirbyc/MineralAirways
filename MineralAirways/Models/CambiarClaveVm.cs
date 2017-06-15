using System.ComponentModel.DataAnnotations;

namespace MineralAirways.Models
{
    public class CambiarClaveVm
    {
        public int PasajeroId { get; set; }

        [Required(ErrorMessage = @"Clave Actual es requerido")]
        public string ClaveActual { get; set; }

        [Required(ErrorMessage = @"Clave Nueva es requerido")]
        public string ClaveNueva { get; set; }

        [Required(ErrorMessage = @"Clave Reingreso es requerido")]
        public string ClaveNuevaReingreso { get; set; }
    }
}