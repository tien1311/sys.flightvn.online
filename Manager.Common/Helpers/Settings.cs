using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Manager.Common.Helpers
{
    public static class Settings
    {
        public static string Get(string _key)
        {

            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            try
            {
                string pathFile = Environment.CurrentDirectory + $"/App_Data/config{envName}.xml";

                string text = System.IO.File.ReadAllText(pathFile);

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(text);

                XmlNodeList xnList = xml.SelectNodes("/config/row");
                foreach (XmlNode xn in xnList)
                {
                    if (xn["key"].InnerText == _key)
                    {
                        return xn["value"].InnerText;
                    }
                }
                return "";
            }
            catch
            {
                return "";
            }
        }
    }
}
