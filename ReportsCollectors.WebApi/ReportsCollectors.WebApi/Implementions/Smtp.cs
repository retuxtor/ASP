using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReportsCollectors.WebApi.Interfaces;
using ReportsCollectors.WebApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;


namespace ReportsCollectors.WebApi.Implementions
{
    public class Smtp : ISmtp
    {
        private readonly AppSettings _configuration;
        private readonly ILogger _logger;
        public Smtp(IOptions<AppSettings> options, ILogger<Smtp> logger)
        {
            _configuration = options.Value;
            _logger = logger;
        }
        public void SendExelToMail(byte[] fileReport, DataFromPoZaim dataFromPoZaim)
        {
            try
            {
                Send(fileReport, dataFromPoZaim);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Ошибка при отправке email сообщения:{topic} коллекторского агенства: {keyCA}, тип отчета: {reportTemplate}", 
                    dataFromPoZaim.Topic, 
                    dataFromPoZaim.KeyCA,
                    dataFromPoZaim.Template);
            }
        }

        private void Send(byte[] fileReport, DataFromPoZaim dataFromPoZaim)
        {
            string mainMailRecipient = dataFromPoZaim.ReceiverForSMPT.FirstOrDefault(e => !e.Copy).Email;
            var copyRecipient = dataFromPoZaim.ReceiverForSMPT.Where(e => e.Copy).ToList();
            var copyMailRecipient = new List<string>();

            copyRecipient.ForEach(rec => copyMailRecipient.Add(rec.Email));
            MailAddress from = new(_configuration.SendersMail, _configuration.OrganizationName);
            MailAddress to = new($"{mainMailRecipient}");
            using MailMessage message = new(from, to);
            using SmtpClient smtp = new(_configuration.MailServerURL, _configuration.MailServerPort);

            string documentName = string.IsNullOrEmpty(dataFromPoZaim.FileName) ? "document" : dataFromPoZaim.FileName;

            Attachment att = new Attachment(new MemoryStream(fileReport), $"{documentName}.xlsx");
            
            message.Attachments.Add(att);
            message.IsBodyHtml = false;
            message.Subject = dataFromPoZaim.Topic;
            
            smtp.Credentials = new NetworkCredential(_configuration.SendersMail, _configuration.MailClientPassword);
            smtp.EnableSsl = true;

            foreach (var emailCopy in copyMailRecipient) 
                message.CC.Add(emailCopy);

            smtp.Send(message);

            _logger.LogInformation("Сообщение {Topic} отправлено. email получателей: {@mainMailRecipient}", dataFromPoZaim.Topic, dataFromPoZaim.ReceiverForSMPT);
        }
    }
}
