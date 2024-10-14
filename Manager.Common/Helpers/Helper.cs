using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Manager.Common.Helpers
{
    public static class Helper
    {
        #region Các biến cần dùng
        private static readonly IHttpContextAccessor _httpContextAccessor;
        private static HttpContext httpContext => _httpContextAccessor.HttpContext;
        // SecretKey dùng để giải mã và validate Checksum
        private static string secretKey = "sF7g4vQYAcMH&5KChqpp4JmcA^sK?dWv";

        public enum LineSeperator
        {
            Before,
            After,
            SpaceBefore,
            SpaceAfter
        }
        #endregion

        /// <summary>
        /// Dùng để tạo Checksum dựa trên Content
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GenerateCheckSum(string toEncrypt)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(secretKey));
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string GetRequestHeaders()
        {
            string result = "";
            try
            {
                if(IsHttpContextAvailable())
                    result = JsonConvert.SerializeObject(httpContext.Request.Headers);
            }
            catch { }
            return result;
        }
        public static string GetResponseHeaders()
        {
            string result = "";
            try
            {
                if (IsHttpContextAvailable())
                    result = JsonConvert.SerializeObject(httpContext.Response.Headers);
            }
            catch { }
            return result;
        }
        public static string GetPath()
        {
            string result = "";
            if (IsHttpContextAvailable())
                result = httpContext.Request.Path;
            return result;
        }
        public static string GetParams()
        {
            string result = "";
            if (IsHttpContextAvailable())
                result = httpContext.Request.QueryString.ToString();
            return result;
        }
        public static string GetIP()
        {
            string result = "";
            if (IsHttpContextAvailable())
                result = httpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString();
            return result;
        }
        public static string GetMethod()
        {
            string result = "";
            if (IsHttpContextAvailable())
                result = httpContext.Request.Method;
            return result;
        }
        public static string GetSessionLog()
        {
            string result = "";
            if (IsHttpContextAvailable())
                if (httpContext.Items.ContainsKey("SessionLog"))
            {
                result = httpContext.Items["SessionLog"].ToString();
            }
            return result;
        }
        public static void SetSessionLog(string content, LineSeperator seperator = LineSeperator.SpaceAfter)
        {
            if (IsHttpContextAvailable())
            {
                string current = "";
                if (httpContext.Items.ContainsKey("SessionLog"))
                {
                    current = httpContext.Items["SessionLog"].ToString();
                    httpContext.Items.Remove("SessionLog");
                }
                if (seperator == LineSeperator.Before)
                {
                    content = $@"-----------------------------------------------------\n\n" + content;
                }
                if (seperator == LineSeperator.After)
                {
                    content = content + $@"\n\n-----------------------------------------------------";
                }
                if (seperator == LineSeperator.SpaceBefore)
                {
                    content = "\n\n" + content;
                }
                if (seperator == LineSeperator.SpaceAfter)
                {
                    content = content + "\n\n";
                }
                current = current + content;
                httpContext.Items.Add("SessionLog", current);
            }
        }
        public static string GetCustomLog()
        {
            string result = "";
            if (IsHttpContextAvailable())
                if (httpContext.Items.ContainsKey("CustomLog"))
                {
                    result = httpContext.Items["CustomLog"].ToString();
                }
            return result;
        }
        public static void SetCustomLog(string content, LineSeperator seperator = LineSeperator.SpaceAfter)
        {
            if (IsHttpContextAvailable())
            {
                string current = "";
                if (httpContext.Items.ContainsKey("CustomLog"))
                {
                    current = httpContext.Items["CustomLog"].ToString();
                    httpContext.Items.Remove("CustomLog");
                }
                if (seperator == LineSeperator.Before)
                {
                    content = $@"-----------------------------------------------------\n\n" + content;
                }
                if (seperator == LineSeperator.After)
                {
                    content = content + $@"\n\n-----------------------------------------------------";
                }
                if (seperator == LineSeperator.SpaceBefore)
                {
                    content = "\n\n" + content;
                }
                if (seperator == LineSeperator.SpaceAfter)
                {
                    content = content + "\n\n";
                }
                current = current + content;
                httpContext.Items.Add("CustomLog", current);
            }
        }

        private static bool IsHttpContextAvailable()
        {
            try
            {
                if (object.ReferenceEquals(null, httpContext))
                    return false;
                return true;
            }
            catch { return false; }
        }

        public static string GetStringLanguage(string _key, string[] _param = null)
        {
            try
            {
                string pathFile = Environment.CurrentDirectory + $"/App_Data/language.xml";

                string text = System.IO.File.ReadAllText(pathFile);

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(text);

                XmlNodeList xnList = xml.SelectNodes("/language/row");
                foreach (XmlNode xn in xnList)
                {
                    if (xn["key"].InnerText == _key)
                    {
                        return GetStringLanguageFormat(xn["value"].InnerText, _param);

                        //if (string.IsNullOrEmpty(xn["valueEn"].InnerText))
                        //    return GetStringLanguageFormat(xn["value"].InnerText, _param);
                        //else
                        //    return GetStringLanguageFormat(xn["value"].InnerText, _param) + "\n\n" + GetStringLanguageFormat(xn["valueEn"].InnerText, _param);
                    }
                }
                return _key;
            }
            catch (Exception ex)
            {
                
                return _key;
            }
        }

        private static string GetStringLanguageFormat(string _value, string[] _param = null)
        {
            if (_param == null)
                return _value;

            int count = _param.Length;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    _value = _value.Replace("{" + i + "}", _param[i]);
                }
            }
            return _value;
        }


    }
}
