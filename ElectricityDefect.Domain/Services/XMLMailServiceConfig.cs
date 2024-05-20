using ElectricityDefect.Domain.Entities;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace ElectricityDefect.Domain.Services
{
    public class XMLMailServiceConfig : IMailServiceConfig
    {
        private string filename;
        private ICryptoService crypto;

        public XMLMailServiceConfig(string _filename, ICryptoService _crypto)
        {
            crypto = _crypto;
            filename = _filename;
        }
        public void SaveSettings(object settings, Type type, string filename)
        {
            XmlSerializer formatter = new XmlSerializer(type);
            TextWriter writer = new StreamWriter(filename, false, Encoding.GetEncoding(1251));

            formatter.Serialize(writer, settings);

            writer.Close();
        }
        public object ReadSettings(Type type, string filename)
        {
            XmlSerializer formatter = new XmlSerializer(type);

            using (StreamReader fs = new StreamReader(filename, Encoding.GetEncoding(1251), false))
            {
                var settings = formatter.Deserialize(fs);
                fs.Close();
                return settings;
            }
        }



        public void SaveSettings(MAILSERVICESETTINGS settings)
        {
           
            settings.PASSWORD = crypto.EncryptPassword(settings.PASSWORD);
            SaveSettings(settings, typeof(MAILSERVICESETTINGS), filename);

        }

        public MAILSERVICESETTINGS ReadSettings()
        {
            if (File.Exists(filename))
            {

                MAILSERVICESETTINGS settings = (MAILSERVICESETTINGS)ReadSettings(typeof(MAILSERVICESETTINGS), filename);
                settings.PASSWORD = crypto.DecryptPassword(settings.PASSWORD);
                return settings;
            }
            else
            {
                MAILSERVICESETTINGS settings = new MAILSERVICESETTINGS
                {
                    HOST = " smtp.yandex.ru",
                    PORT=25,
                    USER="somemail@yandex.ru",
                    PASSWORD= ""

                };

                SaveSettings(settings);

                return settings;
            }

        }
       
    }
}
