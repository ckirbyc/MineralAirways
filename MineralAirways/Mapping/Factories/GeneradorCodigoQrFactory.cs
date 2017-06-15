using MessagingToolkit.QRCode.Codec;
using MineralAirways.Models;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MineralAirways.Mapping.Factories
{
    public class GeneradorCodigoQrFactory
    {
        public void GenerarQr(string rutaTemp, ReservasVuelos reserva ) {
            var numVuelo = reserva.Vuelos.NumeroVuelo;
            var fecha = reserva.Vuelos.FechaHoraSalida.ToString("dd-MM-yyyy");
            var hora = reserva.Vuelos.FechaHoraSalida.ToString("HH:mm");
            var rutPasajero = reserva.Pasajeros.Rut;
            var numAsiento = reserva.Asientos.Descripcion.Trim();
            var nombrePas = reserva.Pasajeros.Nombres;
            var apellidoPas = reserva.Pasajeros.PrimerApellido;

            var codigoDatoQr = numVuelo + ";" + fecha + ";" + hora + ";" + rutPasajero + ";" + numAsiento;
            var nombreArchivo = string.Format("Img_{0}_{1}_{2}.jpg", rutPasajero, numVuelo, numAsiento);

            var encoder = new QRCodeEncoder();            
            encoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
            Bitmap img = new Bitmap(encoder.Encode(codigoDatoQr), new Size(225, 225));            
            Image qr = img;            
            var fileName = Path.Combine(rutaTemp, nombreArchivo);
            //qr.Save(fileName, ImageFormat.Jpeg);
            var archivoExiste = File.Exists(fileName);
            if (!archivoExiste)
            {
                qr.Save(fileName, ImageFormat.Jpeg);
            }
        }
    }
}