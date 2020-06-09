using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.IO;

namespace Microsoft.ApplicationBlocks.Data
{
    public class ConfigHelper
    {
        internal static string GetConString()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory;

            XElement config = XElement.Load(path + "\\UserConfig.xml");
            string type = GetElementValue(config, "DBConnect");

            config = XElement.Load(path + "\\LocalConifg.xml");
            XElement DBs = config.Element("DBConnect");
            foreach (XElement ele in DBs.Elements("DB"))
            {
                string name = GetAttributeValue(ele, "Name");
                if (type.Equals(name))
                {
                    string str = GetAttributeValue(ele, "Value");
                    config = null;
                    return DesDecrypt(str);
                }
            }
            config = null;
            return null;
        }

        public static string GetSqlType()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            XElement config = XElement.Load(path + "\\LocalConifg.xml");
            string type = GetElementValue(config, "SqlType");
            config = null;
            return type;
        }

        public static string GetElementValue(XElement xele, string name)
        {
            XName xname = XName.Get(name);
            XElement xElement = xele.Element(xname);
            if (xElement == null) return String.Empty;
            return xElement.Value;
        }

        private static string GetAttributeValue(XElement xele, string name)
        {
            XName xname = XName.Get(name);
            XAttribute xAttribute = xele.Attribute(xname);
            if (xAttribute == null) return String.Empty;
            return xAttribute.Value;
        }

        private static string DesDecrypt(string strtoDecrypt)
        {
            //string crykey = "12345678";
            string crykey = "!@#$%^&(";
            if (crykey.Trim().Length != 8)
            {
                throw new ArgumentException("the length of the key must equal 8");
            }
            //string key = ConfigurationManager.AppSettings[crykey].ToString();
            string key = crykey;
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] strbyte = new byte[strtoDecrypt.Length / 2];

            for (int x = 0; x < strtoDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(strtoDecrypt.Substring(x * 2, 2), 16));
                strbyte[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(strbyte, 0, strbyte.Length);
            cs.FlushFinalBlock();

            return Encoding.Default.GetString(ms.ToArray());
        }


    }
}
