using System.Net.Mail;
using System.Net;
using System.Text;
using TalleresMillenium.Models;

namespace TalleresMillenium.Services
{
    public class EmailService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly EmailSettings _emailSettings;
        public EmailService(UnitOfWork unitOfWork,EmailSettings emailSettings)
        {
            _unitOfWork = unitOfWork;
            _emailSettings = emailSettings;
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
        {
            using SmtpClient client = new SmtpClient(_emailSettings.SmtpHost, _emailSettings.SmtpPort)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailSettings.EmailFrom, _emailSettings.PasswordEmailFrom)
            };

            MailMessage mail = new MailMessage(_emailSettings.EmailFrom, to, subject, body)
            {
                IsBodyHtml = isHtml,
            };

            await client.SendMailAsync(mail);
        }
        public async Task CreateEmailUser(string email,ICollection<Coche_Servicio> coche_Servicios,string matricula)
        {
            string to = email;
            string subject = $"Envio de la reserva realizada al vehiculo con matricula {matricula}";
            StringBuilder body = new StringBuilder();
            body.AppendLine($"<html> <h1 style='text-align: center;'>Gracias por reservar en Talleresmilemiun el mejor de toda España</h1> <h2 style='text-align: center;'>Matrícula: {matricula}</h2> <table style='border-collapse: collapse; width: 100%;'><tr style='text-align: center;'><td style=' border: solid 1px black; background-color: #808080; color: white'>Nombre</td><td style=' border: solid 1px black; background-color: #808080; color: white'>Imagen</td><td style=' border: solid 1px black; background-color: #808080; color: white'>Descripcion</td></tr>");

            foreach (Coche_Servicio coche_Servicio in coche_Servicios)
            {
                Servicio servicio = coche_Servicio.servicio;
                body.AppendLine($"<tr style='text-align: center;'><td>{servicio.Nombre}</td><td><img style='width: 100%; max-width: 100px; height: 100px; border-radius: 5px;' src='https://talleresmilemiun.runasp.net{servicio.Imagen}'></td><td>{servicio.Descripcion}</td></tr>");
            }
            body.AppendLine("</table>");
            body.AppendLine("</html>");
            await SendEmailAsync(to, subject, body.ToString(), true);
        }
        public async Task CreateDeleteEmail(string email,Servicio servicio,string matricula)
        {
            string to = email;
            string subject = $"Eliminacion de un servicio reservado al vehiculo con matricula {matricula}";
            StringBuilder body = new StringBuilder();
            body.AppendLine($"<html> <h1 style='text-align: center;'>Hemos Eliminado este servicio de tu vehiculo con matricula: {matricula}</h1><table style='border-collapse: collapse; width: 100%;'><tr style='text-align: center;'><td style=' border: solid 1px black; background-color: #808080; color: white'>Nombre</td><td style=' border: solid 1px black; background-color: #808080; color: white'>Imagen</td><td style=' border: solid 1px black; background-color: #808080; color: white'>Descripcion</td></tr>");
            body.AppendLine($"<tr style='text-align: center;'><td>{servicio.Nombre}</td><td><img style='width: 100%; max-width: 100px; height: 100px; border-radius: 5px;' src='https://talleresmilemiun.runasp.net{servicio.Imagen}'></td><td>{servicio.Descripcion}</td></tr>");
            body.AppendLine("</table>");
            body.AppendLine("</html>");
            await SendEmailAsync(to, subject, body.ToString(), true);
        }
        public async Task CreateEmailAceptarUser(string email, ICollection<Coche_Servicio> coche_Servicios,string fecha,string matricula)
        {
            string to = email;
            string fechabuena = DateTime.Parse(fecha).ToString("dd-MM-yyyy");
            string subject = $"Envio de la confirmacion de la reserva realizada al vehiculo con matricula: {matricula}";
            StringBuilder body = new StringBuilder();
            body.AppendLine($"<html> <h1 style='text-align: center;'>Hemos confirmado su reserva</h1> <h2 style='text-align: center;'>Matrícula: {matricula}</h2> <h3>Fecha de traida del coche: {fechabuena}</h3> <table style='border-collapse: collapse; width: 100%;'><tr style='text-align: center;'><td style=' border: solid 1px black; background-color: #808080; color: white'>Nombre</td><td style=' border: solid 1px black; background-color: #808080; color: white'>Imagen</td><td style=' border: solid 1px black; background-color: #808080; color: white'>Descripcion</td></tr>");
            foreach (Coche_Servicio coche_Servicio in coche_Servicios)
            {
                Servicio servicio = coche_Servicio.servicio;
                body.AppendLine($"<tr style='text-align: center;'><td>{servicio.Nombre}</td><td><img style='width: 100%; max-width: 100px; height: 100px; border-radius: 5px;' src='https://talleresmilemiun.runasp.net{servicio.Imagen}'></td><td>{servicio.Descripcion}</td></tr>");
            }
            body.AppendLine("</table>");
            body.AppendLine("</html>");
            await SendEmailAsync(to, subject, body.ToString(), true);
        }
        public async Task CreateEmailFinalizarUser(string email, ICollection<Coche_Servicio> coche_Servicios,string matricula)
        {
            string to = email;
            string subject = $"Envio de la finalizacion de los servicios dados al vehiculo con matricula: {matricula}";
            StringBuilder body = new StringBuilder();
            body.AppendLine($"<html> <h1 style='text-align: center;'>Hemos finalizado los servicios de su vehiculo</h1> <h2 style='text-align: center;'>Matrícula: {matricula}</h2> <table style='border-collapse: collapse; width: 100%;'><tr style='text-align: center;'><td style=' border: solid 1px black; background-color: #808080; color: white'>Nombre</td><td style=' border: solid 1px black; background-color: #808080; color: white'>Imagen</td><td style=' border: solid 1px black; background-color: #808080; color: white'>Descripcion</td></tr>");
            foreach (Coche_Servicio coche_Servicio in coche_Servicios)
            {
                Servicio servicio = coche_Servicio.servicio;
                body.AppendLine($"<tr style='text-align: center;'><td>{servicio.Nombre}</td><td><img style='width: 100%; max-width: 100px; height: 100px; border-radius: 5px;' src='https://talleresmilemiun.runasp.net{servicio.Imagen}'></td><td>{servicio.Descripcion}</td></tr>");
            }
            body.AppendLine("</table>");
            body.AppendLine("</html>");
            await SendEmailAsync(to, subject, body.ToString(), true);
        }
    }
}

