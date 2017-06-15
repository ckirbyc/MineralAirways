using System;
using System.ComponentModel;

namespace MineralAirways.Models
{
    public class ReservaVueloFiltroPlanillaVm
    {
        [DisplayName(@"Número del Vuelo")]        
        public int NumeroVuelo { get; set; }
        [DisplayName(@"Fecha del Vuelo")]        
        public DateTime FechaHoraSalida { get; set; }
    }
}