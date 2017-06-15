//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MineralAirways.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Vuelos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vuelos()
        {
            this.ReservasVuelos = new HashSet<ReservasVuelos>();
            this.Tramos = new HashSet<Tramos>();
        }
    
        public int VueloID { get; set; }
        public int OrigenID { get; set; }
        public int DestinoID { get; set; }
        public System.DateTime FechaHoraSalida { get; set; }
        public int NumeroVuelo { get; set; }
        public int AvionID { get; set; }
        public bool Visible { get; set; }
    
        public virtual Aviones Aviones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReservasVuelos> ReservasVuelos { get; set; }
        public virtual Rutas Rutas { get; set; }
        public virtual Rutas Rutas1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tramos> Tramos { get; set; }
    }
}
