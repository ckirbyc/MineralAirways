using System;
using System.Collections.Generic;

namespace MineralAirways.Models
{
    public class VueloAsientoVm
    {
        public int VueloId { get; set; }
        public int AsientoId { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public int NumeroVuelo { get; set; }
        public DateTime FechaVuelo { get; set; }
        public string Asiento { get; set; }
        public List<Tramos> ListaTramos { get; set; }
    }
}