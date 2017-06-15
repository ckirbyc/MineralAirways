using Microsoft.AspNet.Identity.Owin;
using MineralAirways.Models;
using System.Linq;

namespace MineralAirways.Mapping.Factories
{
    public class AccountFactory
    {
        public SignInStatus ValidarCuentaUsuario(LoginViewModel model) {

            var rut = model.RutUsuario.ToLower().Trim();
            var pass = model.Password.ToLower().Trim();

            using (var entityContext = new DAPEntities())
            {
                var regBd = entityContext.Pasajeros.Where(x => x.Rut == rut && x.Pass == pass).FirstOrDefault();
                if (regBd != null) {
                    var pasajeroVm = new PasajerosViewModel {
                        Rut = regBd.Rut,
                        Pass = regBd.Pass,
                        PasajeroID = regBd.PasajeroID,
                        PrimerApellido = regBd.PrimerApellido,
                        SegundoApellido = regBd.SegundoApellido,
                        Nombre = regBd.Nombres,
                        AsientoAsignado = regBd.AsientoAsignado != null ? regBd.AsientoAsignado : string.Empty,
                        TipoUsuario= regBd.TipoUsuario,
                        EsEjecutivo = (bool)regBd.EsEjecutivo,
                        PrimerLogeo = regBd.PrimerLogeo
                    };
                    SessionViewModel.Usuario = pasajeroVm;
                    return SignInStatus.Success;
                }
            }

            return SignInStatus.Failure;
        }
    }
}