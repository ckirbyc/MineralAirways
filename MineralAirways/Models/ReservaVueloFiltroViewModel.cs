using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MineralAirways.Models
{
    public class ReservaVueloFiltroViewModel
    {
        public int Origen { get; set; }
        public IList<SelectListItem> OrigenSelectListItem { get; set; }
        public int Destino { get; set; }
        public IList<SelectListItem> DestinoSelectListItem { get; set; }
        public string FechaSel { get; set; }
    }
}