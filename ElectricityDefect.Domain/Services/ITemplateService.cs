using System;
using System.Collections.Generic;

namespace ElectricityDefect.Domain.Services
{
    public interface ITemplateService
    {
        string GetTemplateBody();
        string GetTemplateSubject();

        string[] GetTemplateRecipients();
        void SaveTemplate(string[] recipients,string subject, string text);
        string GetTemplateBody(object order);
        string GetTemplateSubject(object order);
        Dictionary<string, string> AvailableParameters(Type type);

    }
}
