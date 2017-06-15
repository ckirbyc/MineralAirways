using MineralAirways.Models;
using Resources;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MineralAirways.Mapping.Factories
{
    public class EnvioCorreoFactory
    {
        public static bool EnviarEmail(EnvioEmailViewModel model, ReservasVuelos reserva, string rutaTemp, string rutaTempAux)
        {
            try
            {
                var envioCorreo = Convert.ToBoolean(ConfigurationManager.AppSettings["UtilizarEnvioCorreo"]);

                if (envioCorreo)
                {
                    var vistaPlana = RetornaPlantillaGeneralPlano(string.Empty, string.Empty);
                    var vistaHtml = RetornaPlantillaGeneralHtml(model.BodyEmail, string.Empty, reserva, rutaTemp, rutaTempAux);

                    var servidor = ConfigurationManager.AppSettings["smtpServer"];
                    var puerto = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
                    var userEmail = ConfigurationManager.AppSettings["UserEmail"];
                    var passEmail = ConfigurationManager.AppSettings["PassEmail"];

                    var m = new SmtpClient(servidor, puerto);
                    m.EnableSsl = true;
                    m.UseDefaultCredentials = false;
                    m.Credentials = new NetworkCredential(userEmail, passEmail);

                    var mail = new MailMessage();

                    SetCorreosDestinatarios(model.To, mail.To);

                    //Correos con copia deben separarse por una coma simple, aca se separan
                    if (model.CopyCarbon != null)
                    {
                        if (model.CopyCarbon.Trim() != string.Empty)
                        {
                            var copias = model.CopyCarbon.Split(',');
                            foreach (var item in copias.Where(item => !string.IsNullOrEmpty(item.Trim())))
                            {
                                mail.CC.Add(item.Trim());
                            }
                        }
                    }

                    //Correos con copia oculta deben separarse por una coma simple, aca se separan                        
                    if (model.BlindCopyCarbon != null)
                    {
                        if (model.BlindCopyCarbon.Trim() != string.Empty)
                        {
                            var copias = model.BlindCopyCarbon.Split(',');
                            foreach (var item in copias.Where(item => !string.IsNullOrEmpty(item.Trim())))
                            {
                                mail.Bcc.Add(item.Trim());
                            }
                        }
                    }

                    var desde = model.FromEmail;
                    mail.From = new MailAddress(desde, "Sistema Mineral Airways");
                    mail.Subject = model.Subject;
                    mail.BodyEncoding = Encoding.UTF8;
                    mail.IsBodyHtml = true;
                    mail.Priority = MailPriority.Normal;

                    mail.AlternateViews.Add(vistaPlana);
                    mail.AlternateViews.Add(vistaHtml);

                    //validate the certificate
                    ServicePointManager.ServerCertificateValidationCallback =
                    delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    { return true; };

                    m.DeliveryMethod = SmtpDeliveryMethod.Network;
                    m.Send(mail);
                    return true;
                }

                return true;
            }
            catch (SmtpFailedRecipientException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool EnviarEmail(EnvioEmailViewModel model) {
            try
            {
                var envioCorreo = Convert.ToBoolean(ConfigurationManager.AppSettings["UtilizarEnvioCorreo"]);

                if (envioCorreo)
                {
                    var vistaPlana = RetornaPlantillaGeneralPlano(string.Empty, string.Empty);
                    var vistaHtml = RetornaPlantillaGeneralHtml(model.BodyEmail, string.Empty);

                    var servidor = ConfigurationManager.AppSettings["smtpServer"];
                    var puerto = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
                    var userEmail = ConfigurationManager.AppSettings["UserEmail"];
                    var passEmail = ConfigurationManager.AppSettings["PassEmail"];

                    var m = new SmtpClient(servidor, puerto);
                    m.EnableSsl = true;
                    m.UseDefaultCredentials = false;
                    m.Credentials = new NetworkCredential(userEmail, passEmail);

                    var mail = new MailMessage();

                    SetCorreosDestinatarios(model.To, mail.To);

                    //Correos con copia deben separarse por una coma simple, aca se separan
                    if (model.CopyCarbon != null)
                    {
                        if (model.CopyCarbon.Trim() != string.Empty)
                        {
                            var copias = model.CopyCarbon.Split(',');
                            foreach (var item in copias.Where(item => !string.IsNullOrEmpty(item.Trim())))
                            {
                                mail.CC.Add(item.Trim());
                            }
                        }
                    }

                    //Correos con copia oculta deben separarse por una coma simple, aca se separan                        
                    if (model.BlindCopyCarbon != null)
                    {
                        if (model.BlindCopyCarbon.Trim() != string.Empty)
                        {
                            var copias = model.BlindCopyCarbon.Split(',');
                            foreach (var item in copias.Where(item => !string.IsNullOrEmpty(item.Trim())))
                            {
                                mail.Bcc.Add(item.Trim());
                            }
                        }
                    }

                    var desde = model.FromEmail;
                    mail.From = new MailAddress(desde, "Sistema Mineral Airways");
                    mail.Subject = model.Subject;
                    mail.BodyEncoding = Encoding.UTF8;
                    mail.IsBodyHtml = true;
                    mail.Priority = MailPriority.Normal;

                    mail.AlternateViews.Add(vistaPlana);
                    mail.AlternateViews.Add(vistaHtml);

                    //validate the certificate
                    ServicePointManager.ServerCertificateValidationCallback =
                    delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    { return true; };

                    m.DeliveryMethod = SmtpDeliveryMethod.Network;
                    m.Send(mail);
                    return true;
                }

                return true;
            }
            catch (SmtpFailedRecipientException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static AlternateView RetornaPlantillaGeneralPlano(string message, string pieMsj)
        {
            var plano = ConfiguraEmail.PlantillaGeneralTexto;

            plano = plano.Replace("[MENSAJE]", message);
            plano = plano.Replace("[PIE_PAGINA]", pieMsj);


            var retorno = AlternateView.CreateAlternateViewFromString(plano, Encoding.UTF8, MediaTypeNames.Text.Plain);
            return retorno;
        }

        private static AlternateView RetornaPlantillaGeneralHtml(string message, string msjPie, ReservasVuelos reserva, string rutaTemp, string rutaTempAux)
        {
            var html = ConfiguraEmail.PlantillaGeneralHtml;

            html = html.Replace("[MENSAJE]", message);
            html = html.Replace("[PIE_PAGINA]", msjPie);

            var retorno = AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8, MediaTypeNames.Text.Html);

            var pathLogo = AppDomain.CurrentDomain.BaseDirectory + @"img\mineral.png";
            var imgLogo = new LinkedResource(pathLogo, MediaTypeNames.Image.Gif) { ContentId = "IMGLOGO" };
            retorno.LinkedResources.Add(imgLogo);

            var pathSnoo = AppDomain.CurrentDomain.BaseDirectory + @"Img\Mineral_only.png";
            var imgSnoopy = new LinkedResource(pathSnoo, MediaTypeNames.Image.Gif) { ContentId = "IMGFOOTER" };
            retorno.LinkedResources.Add(imgSnoopy);

            //Obtiene src imagen QR            


            //var pathBullet = AppDomain.CurrentDomain.BaseDirectory + @"Img\CodigoQR.png";
            var pathBullet = Helpers.ImageQr.ObtenerRutaImagenQr(reserva, rutaTemp, rutaTempAux);
            var imgBullet = new LinkedResource(pathBullet, MediaTypeNames.Image.Jpeg) { ContentId = "IMGCODQR" };
            retorno.LinkedResources.Add(imgBullet);

            return retorno;
        }

        private static AlternateView RetornaPlantillaGeneralHtml(string message, string msjPie)
        {
            var html = ConfiguraEmail.PlantillaGeneralCorreoHtml;

            html = html.Replace("[MENSAJE]", message);
            html = html.Replace("[PIE_PAGINA]", msjPie);

            var retorno = AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8, MediaTypeNames.Text.Html);

            var pathLogo = AppDomain.CurrentDomain.BaseDirectory + @"img\mineral.png";
            var imgLogo = new LinkedResource(pathLogo, MediaTypeNames.Image.Gif) { ContentId = "IMGLOGO" };
            retorno.LinkedResources.Add(imgLogo);

            var pathSnoo = AppDomain.CurrentDomain.BaseDirectory + @"Img\Mineral_only.png";
            var imgSnoopy = new LinkedResource(pathSnoo, MediaTypeNames.Image.Gif) { ContentId = "IMGFOOTER" };
            retorno.LinkedResources.Add(imgSnoopy);            

            return retorno;
        }

        public static void SetCorreosDestinatarios(string cadenaCorreos, MailAddressCollection mailAddressCollection)
        {
            var listaDeCorreos = cadenaCorreos.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var correo in listaDeCorreos.Where(correo => !string.IsNullOrEmpty(correo.Trim())))
            {
                mailAddressCollection.Add(correo.Trim());
            }
        }
    }
}