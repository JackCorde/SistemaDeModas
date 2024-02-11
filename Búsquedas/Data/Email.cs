using System.Net;
using System.Net.Mail;

namespace Búsquedas.Data
{
    public class Email{
        public void Enviar(string correo, string newPass)
        {
            Correo(correo, newPass);
        }

        void Correo (string correo_receptor, string pass){
            string correo_emisor = "JackCorderod@outlook.com";
            string clave_emisor = "Bas2do.Guitar";

            MailAddress receptor = new(correo_receptor);
            MailAddress emisor = new(correo_emisor);

            MailMessage email = new(emisor, receptor);
            email.Subject = "BRENANT MODA: Recuperación de Contraseña";
            email.Body = @"<!DOCTYPE html>
                            <html>
                                <head>
                                    <title>BRENANT Moda</title>
                                </head>
                                <body>
                                    <h2>Recuperación de Contraseña de Usuario</h2>
                                    <br>
                                    <p>Su contraseña nueva de usuario BRENANT Moda es: </p>
                                    <p>"+pass+@"</p>
                                </body>
                            </html>";

            email.IsBodyHtml = true;

            SmtpClient smtp = new();
            smtp.Host = "smtp.office365.com"; //Dependiendo del correo
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential(correo_emisor, clave_emisor);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(email);
            }
            catch (System.Exception)
            {
                throw;
            }

        }
    }
}
