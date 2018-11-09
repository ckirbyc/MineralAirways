using System;
using System.ComponentModel;

namespace MineralAirways.Models
{
    public class ReservaVueloFiltroPlanillaVm
    {
        [DisplayName(@"Número Vuelo")]        
        public int NumeroVuelo { get; set; }
        [DisplayName(@"Fecha Vuelo")]        
        public DateTime FechaHoraSalida { get; set; }
        [DisplayName(@"Fecha Inicio Vuelo")]
        public DateTime FechaHoraInicio { get; set; }
        [DisplayName(@"Fecha Fin Vuelo")]
        public DateTime FechaHoraFin { get; set; }
    }
}