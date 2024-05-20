using ElectricityDefect.Domain.Entities;
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
    public class EMailAlert : IAlert
    {
        private ISender sender;
        private ITemplateService template;
        
        public EMailAlert(ISender _sender, ITemplateService _template)
        {
            sender = _sender;
            template = _template;
            
        }
      

        public void Alert(object alert, MAILSERVICESETTINGS config)
        {
            var to = String.Join(",", template.GetTemplateRecipients());
            var body = template.GetTemplateBody(alert);
            var subject = template.GetTemplateSubject(alert);
            sender.SendEMail(config,to, subject, body);
            
        }
       
    }
}
