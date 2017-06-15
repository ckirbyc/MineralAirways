namespace MineralAirways.Models
{
    public class PasajerosViewModel
    {
        public int PasajeroID { get; set; }
        public string Rut { get; set; }
        public string Pass { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string AsientoAsignado { get; set; }
        public int TipoUsuario { get; set; }
        public bool EsEjecutivo { get; set; }
        public bool PrimerLogeo { get; set; }
    }
    
}