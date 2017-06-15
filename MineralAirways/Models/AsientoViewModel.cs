using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MineralAirways.Models
{
    public class AsientoViewModel
    {
        public int AsientoID { get; set; }

        public int AvionID { get; set; }
        public int FilaAsiento { get; set; }

        public string ColumnaAsiento { get; set; }

        public string Descripcion { get; set; }

        public int Tipo { get; set; }

    }
}