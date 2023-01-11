using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System;
using System.Linq;

namespace k190144_Q2_ClassLibrary
{
    public static class ParsingLogic
    {
        private static void ParseData(string path, string filename)
        {
            var doc = new HtmlDocument();
            doc.Load(path + "\\" + filename);
            var tables = doc.DocumentNode.SelectNodes("//div[@class='table-responsive']");

            for (int i = 0; i < tables.Count; i++)
            {
                var category = tables[i].SelectSingleNode(".//th[@colspan='8']//h4");

                string directory = path + "\\" + category.InnerText;
                directory = Regex.Replace(directory, "/", @"");
                Directory.CreateDirectory(directory);

                var scripts = tables[i].SelectNodes(".//td[@class='dataportal']");
                var currentPrice = tables[i].SelectNodes(".//tr[@class='red-text-td' or @class='green-text-td' or @class='blue-text-td']//td[6]");

                string xmlFileName = directory + @"\" + DateTime.Now.ToString("dd-MMM-yyyy HH-mm-ss") + ".xml";
                CreateXmlFile(scripts, currentPrice, xmlFileName);
            }
        }

        private static void CreateXmlFile(HtmlNodeCollection scripts, HtmlNodeCollection currentPrice, string xmlFileName)
        {
            XmlTextWriter xmlWriter = new XmlTextWriter(xmlFileName, System.Text.Encoding.UTF8)
            {
                Formatting = Formatting.Indented
            };

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("xml");

            for (int j = 0; j < scripts.Count; j++)
            {
                xmlWriter.WriteStartElement("Scripts");
                xmlWriter.WriteElementString("Script", scripts[j].InnerText);
                xmlWriter.WriteElementString("Price", currentPrice[j].InnerText);
                xmlWriter.WriteEndElement();

            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            xmlWriter.Close();
        }

        public static void ParseHtmlFile(string path)
        {
            DirectoryInfo dri = new DirectoryInfo(path);
            var directory = new DirectoryInfo(dri.ToString());
            var fileName = directory.GetFiles()
             .OrderByDescending(f => f.LastWriteTime)
             .First();

            ParseData(path, fileName.ToString());

        }
    }
}