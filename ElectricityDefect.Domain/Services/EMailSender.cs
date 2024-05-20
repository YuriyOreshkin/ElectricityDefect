﻿using ElectricityDefect.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDefect.Domain.Services
{
    public class EMailSender : ISender
    {
        private IMailServiceConfig config;
        
        public EMailSender(IMailServiceConfig _config)
        {
            config = _config;
            
        }


        public void SendEMail(MAILSERVICESETTINGS settings,string to, string subject, string body,string attachment=null)
        {
           if(settings ==null)
                settings = config.ReadSettings();

                   // адрес smtp-сервера и порт, с которого будем отправлять письмо
                SmtpClient smtp = new SmtpClient(settings.HOST, settings.PORT);
                // логин и пароль
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(settings.USER, settings.PASSWORD);
                //smtp.EnableSsl = true;

                // отправитель - устанавливаем адрес и отображаемое в письме имя
                //MailAddress from = new MailAddress(settings.USER);
                string from = settings.USER;
                // кому отправляем
                // MailAddress toadd = new MailAddress(to);
                // создаем объект сообщения
                MailMessage mail = new MailMessage(from, to);
                // тема письма
                mail.Subject = subject;
                // текст письма
                mail.Body = body;
                // письмо представляет код html
                mail.IsBodyHtml = true;

                //прикрепляем вложения
                if (!String.IsNullOrEmpty(attachment) & File.Exists(attachment))
                {
                    mail.Attachments.Add(new Attachment(attachment));
                }
           
            smtp.Send(mail);
           
            mail.Dispose();
        }
       
    }
}
