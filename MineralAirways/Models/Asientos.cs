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
    
    public partial class Asientos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Asientos()
        {
            this.ReservasVuelos = new HashSet<ReservasVuelos>();
        }
    
        public int AsientoID { get; set; }
        public int AvionID { get; set; }
        public int FilaAsiento { get; set; }
        public string ColumnaAsiento { get; set; }
        public string Descripcion { get; set; }
        public int Tipo { get; set; }
    
        public virtual Aviones Aviones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReservasVuelos> ReservasVuelos { get; set; }
    }
}
