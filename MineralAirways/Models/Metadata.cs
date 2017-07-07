using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MineralAirways.Models
{
    [MetadataType(typeof(Metadata))]
    public partial class Vuelos
    { 
        class Metadata
        {
            [Display(Name ="Vuelo")]
            public object VueloID { get; set; }

            [Display(Name = "Origen")]
            public object OrigenID { get; set; }

            [Display(Name = "Destino")]
            public object DestinoID { get; set; }

            [Display(Name = "Fecha y Hora de Salida")]
            [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
            [DataType(DataType.DateTime)]
            public DateTime FechaHoraSalida { get; set; }

            [Display(Name = "Número del Vuelo")]
            public object NumeroVuelo { get; set; }

            [Display(Name = "Avión")]
            public object AvionID { get; set; }

        }
    }

    [MetadataType(typeof(Metadata))]
    public partial class Aviones
    {
        class Metadata
        {
            [Display(Name = "Avión")]
            public object AvionID { get; set; }

            [Display(Name = "Matrícula del Avión")]
            public object MatriculaDelAvion { get; set; }
           
        }
    }

    [MetadataType(typeof(Metadata))]
    public partial class Clientes
    {
        class Metadata
        {
            [Display(Name ="Cliente")]
            public object ClienteID { get; set; }
        }
    }

    [MetadataType(typeof(Metadata))]
    public partial class Rutas
    {
        class Metadata
        {
            [Display(Name ="Ruta")]
            public object RutaID { get; set; }
            public object Ciudad { get; set; }

            [Display(Name ="Región")]
            public object Region { get; set; }

            [Display(Name ="Código de Vuelo")]
            public object Codigo { get; set; }

            [Display(Name ="Descripción")]
            public object Descripcion { get; set; }
        }
    }

    [MetadataType(typeof(Metadata))]
    public partial class Tramos
    {
        class Metadata
        {
            [Display(Name = "Escala")]
            public object TramoID { get; set; }

            [Display(Name = "Vuelo")]
            public object VueloID { get; set; }

            [Display(Name = "Origen")]
            public object OrigenID { get; set; }

            [Display(Name = "Destino")]
            public object DestinoID { get; set; }
        }
    }

    [MetadataType(typeof(Metadata))]
    public  partial class Pasajeros
    {
        class Metadata
        {
            [Display(Name = "Pasajero")]
            public object PasajeroID { get; set; }
            public object Rut { get; set; }
            public object Nombres { get; set; }

            [Display(Name = "Apellido Paterno")]
            public object PrimerApellido { get; set; }

            [Display(Name = "Apellido Materno")]
            public object SegundoApellido { get; set; }
            public object Cargo { get; set; }

            [Display(Name = "E-Mail")]
            public object EMail { get; set; }

            [Display(Name = "Asiento Asignado")]
            public object AsientoAsignado { get; set; }

            [Display(Name = "Es Ejecutivo")]
            public object EsEjecutivo { get; set; }

            [Display(Name = "Cliente")]
            public object ClienteID { get; set; }

            [Display(Name = "Contraseña")]
            public object Pass { get; set; }

            [Display(Name = "Tipo de Usuario")]
            public object TipoUsuario { get; set; }
        }
    }

    [MetadataType(typeof(Metadata))]
    public partial class ReservasVuelos
    {
        class Metadata
        {
            [Display(Name = "Reserva de Vuelo")]
            public object ReservaVueloID { get; set; }

            [Display(Name = "Avión")]
            public object AvionID { get; set; }

            [Display(Name = "Pasajero")]
            public object PasajeroID { get; set; }

            [Display(Name = "Confirmación Asiento")]
            public object ConfirmacionAsiento { get; set; }

            [Display(Name = "Fecha y Hora de Confirmación")]
            public DateTime? FechaHoraConfirmación { get; set; }

            [Display(Name = "Fecha y Hora de Cancelación")]
            public DateTime? FechaHoraCancelacion { get; set; }


            public object VueloID { get; set; }
            public object AsientoID { get; set; }
        }
    }

    //[MetadataType(typeof(Metadata))]
    //public partial class Asientos
    //{
    //    class metadata
    //    {
    //        [Display(Name = "Identificador Asiento")]
    //        public object AsientoID { get; set; }

    //        [Display(Name = "Identificador Avión")]
    //        public object AvionID { get; set; }

    //        [Display(Name = "Fila")]
    //        public object FilaAsiento { get; set; }

    //        [Display(Name = "Columna")]
    //        public object ColumnaAsiento { get; set; }

    //        [Display(Name = "Asiento")]
    //        public object Descripcion { get; set; }

    //        [Display(Name = "Tipo de Asiento")]
    //        public object Tipo { get; set; }
    //    }
    //}
}